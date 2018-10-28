using System;
using System.IO;
using System.Reflection;
using System.Threading;
using Harmony;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Subnautica_Enhanced_Sleep
{
    public class Main : MonoBehaviour
    {
        public static HarmonyInstance hinstance;
        
        public static string fileName;
        public static string logDir;

        public static void Patch()
        {
            fileName = "" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + "--" +
                       DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second + ".log";
            try
            {
                hinstance = HarmonyInstance.Create("subnauticaenhancedsleep");
                SleepPatcher.invokeAssets();
                Tiredness.initAssets();
                SceneManager.sceneLoaded += OnSceneLoaded;
                SceneManager.sceneUnloaded += OnSceneUnloaded;
                hinstance.PatchAll(Assembly.GetExecutingAssembly());
                Log("Patched Successfully");
            }
            catch (Exception ex)
            {
                Log("[FATAL CRITICAL ERROR] Could not Patch: " + ex.Message);
                Log("[FATAL CRITICAL ERROR] Stack Trace: " + ex.StackTrace);
            }
        }

        public static void Log(string message)
        {
            Console.WriteLine("[Enhanced Sleep] <" + DateTime.Now.ToString("HH:mm:ss") + "> " + message);
            logDir = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            File.AppendAllText(logDir + "/" + fileName, "<" + DateTime.Now.ToString("HH:mm:ss") + "> " + message + "\n");
        }

        public static string getLog()
        {
            string s = "";
            if ((logDir != null && !logDir.Equals("")) && File.Exists(logDir + "/" + fileName))
            {
                s += File.ReadAllText(logDir + "/" + fileName);
            }

            return s;
        }

        
        

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Log("[DEBUG] Scene loaded: " + scene.name);
            if (scene.name == "Main")
            {
                Log("Initiating Tiredness for current world.");
                Tiredness.onEnable();
            }
        }

        private static void OnSceneUnloaded(Scene scene)
        {
            Log("[DEBUG] Scene unloaded: " + scene.name);
            if (scene.name == "Main")
            {
                Log("Unloading Tiredness.");
                Tiredness.onDisable();
            }
        }
    }
}
