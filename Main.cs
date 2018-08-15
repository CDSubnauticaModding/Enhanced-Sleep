using System;
using System.IO;
using System.Reflection;
using Harmony;

namespace Subnautica_Enhanced_Sleep
{
    public class Main
    {
        public static HarmonyInstance hinstance;
        
        public static string fileName;

        public static void Patch()
        {
            fileName = "" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + "--" +
                       DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second + ".log";
            try
            {
                hinstance = HarmonyInstance.Create("subnauticaenhancedsleep");
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
            string logDir = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            File.AppendAllText(logDir + "/" + fileName, "<" + DateTime.Now.ToString("HH:mm:ss") + "> " + message + "\n");
        }
    }
}
