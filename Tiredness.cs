using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using UnityEngine;

namespace Subnautica_Enhanced_Sleep
{
    public class Tiredness : MonoBehaviour
    {
        public static Dictionary<Player, float> tirednessDict;
        public static Dictionary<Player, float> tirednessLastUpdateH;
        public static Dictionary<Player, float> tirednessLastUpdateD;
        public static bool isIngame = false;
        public static bool wasSleepIntroSent = false;

        public static void onEnable()
        {
            isIngame = true;
            tirednessDict = new Dictionary<Player, float>();
            tirednessLastUpdateH = new Dictionary<Player, float>();
            tirednessLastUpdateD = new Dictionary<Player, float>();
            TirednessGui.Load();
            /*
            if (!wasSleepIntroSent)
            {
                gameStartSleepIntroNotification01.Play();
                gameStartSleepIntroNotification02.Play();
                gameStartSleepIntroNotification03.Play();
            }
            */
        }

        public static void onDisable()
        {
            isIngame = false;
            tirednessDict = null;
            tirednessLastUpdateH = null;
            tirednessLastUpdateD = null;
        }

        [HarmonyPatch(typeof(Player))]
        [HarmonyPatch("Update")]
        public class PlayerTirednessPatch
        {
            public static void Postfix(Player __instance)
            {
                if (isIngame && !uGUI.main.loading.IsLoading && !uGUI.main.intro.showing && tirednessDict != null && tirednessLastUpdateH != null && tirednessLastUpdateD != null)
                {
                    if (tirednessLastUpdateH.ContainsKey(__instance) && tirednessLastUpdateD.ContainsKey(__instance) && tirednessDict.ContainsKey(__instance))
                    {
                        /*
                        tirednessDict.TryGetValue(__instance, out float playerTiredness);
                        tirednessLastUpdateH.TryGetValue(__instance, out float oldHour);
                        tirednessLastUpdateD.TryGetValue(__instance, out float oldDay);
                        float newHour = DayNightCycle.main.GetDayNightCycleTime();
                        float newDay = (float) Math.Floor(DayNightCycle.main.GetDay());
                        float newHour2 = newHour;
                        if (oldDay != newDay)
                        {
                            newHour2 += (newDay - oldDay);
                        }
                        float timePassed = newHour2 - oldHour;
                        float timePassedHours = (float)(timePassed / (1d / 24));
                        float timePassedMinutes = (float) (timePassedHours * 60);
                        float looseFactor = 4;
                        if (timePassedMinutes >= 1)
                        {
                            float tirednessAdded = (float)(timePassedMinutes / (43.2 / looseFactor));
                            float tirednessOut = (float) (playerTiredness + tirednessAdded);
                            tirednessDict[__instance] = tirednessOut;
                            tirednessLastUpdateH[__instance] = newHour;
                            tirednessLastUpdateD[__instance] = newDay;
                            float tirednessNotifyValue = 25f;
                            float tirednessWarningNotifyValue = 50f;
                            if (playerTiredness < tirednessNotifyValue && tirednessOut >= tirednessNotifyValue && tirednessOut < tirednessWarningNotifyValue)
                            {
                                tiredNotification.Play();
                            }
                            else if (playerTiredness < tirednessWarningNotifyValue && tirednessOut >= tirednessNotifyValue)
                            {
                                tiredWarningNotification.Play();
                            }
                        }
                        */
                        // NEW VARIANT

                        tirednessDict.TryGetValue(__instance, out float playerTiredness);
                        tirednessLastUpdateH.TryGetValue(__instance, out float oldTime);
                        float newTime = (float)DayNightCycle.main.GetDayNightCycleTime() + (float)Math.Floor(DayNightCycle.main.GetDay());
                        float timePassed = newTime - oldTime;
                        float timePassedHours = (float)(timePassed / (1d / 24));
                        float timePassedMinutes = (float)(timePassedHours * 60);
                        float looseFactor = 4;
                        if (timePassedMinutes >= 2)
                        {
                            float tirednessAdded = (float)(timePassedMinutes / (43.2 / looseFactor));
                            float tirednessOut = (float)(playerTiredness + tirednessAdded);
                            if (tirednessOut > 100)
                            {
                                tirednessOut = 100;
                            }
                            tirednessDict[__instance] = tirednessOut;
                            tirednessLastUpdateH[__instance] = newTime;
                            float tirednessNotifyValue = 25f;
                            float tirednessWarningNotifyValue = 50f;
                            if (playerTiredness < tirednessNotifyValue && tirednessOut >= tirednessNotifyValue && tirednessOut < tirednessWarningNotifyValue)
                            {
                                //tiredNotification.Play();
                            }
                            else if (playerTiredness < tirednessWarningNotifyValue && tirednessOut >= tirednessNotifyValue)
                            {
                                //tiredWarningNotification.Play();
                            }
                        }
                    }
                    else
                    {
                        if (tirednessDict.ContainsKey(__instance))
                        {
                            //tirednessDict.TryGetValue(__instance, out float playerTiredness);
                            //tirednessDict[__instance] = playerTiredness;
                            tirednessDict[__instance] = 0;
                            if (tirednessLastUpdateH.ContainsKey(__instance))
                            {
                                //tirednessLastUpdateH[__instance] = DayNightCycle.main.GetDayNightCycleTime();
                                tirednessLastUpdateH[__instance] = (float) DayNightCycle.main.GetDayNightCycleTime() + (float) Math.Floor(DayNightCycle.main.GetDay());
                            }
                            else
                            {
                                //tirednessLastUpdateH.Add(__instance, DayNightCycle.main.GetDayNightCycleTime());
                                tirednessLastUpdateH.Add(__instance, (float)DayNightCycle.main.GetDayNightCycleTime() + (float)Math.Floor(DayNightCycle.main.GetDay()));
                            }
                            if (tirednessLastUpdateD.ContainsKey(__instance))
                            {
                                tirednessLastUpdateD[__instance] = (float)Math.Floor(DayNightCycle.main.GetDay());
                            }
                            else
                            {
                                tirednessLastUpdateD.Add(__instance, (float)Math.Floor(DayNightCycle.main.GetDay()));
                            }
                        }
                        else
                        {
                            tirednessDict.Add(__instance, 0);
                            if (tirednessLastUpdateH.ContainsKey(__instance))
                            {
                                //tirednessLastUpdateH[__instance] = DayNightCycle.main.GetDayNightCycleTime();
                                tirednessLastUpdateH[__instance] = (float)DayNightCycle.main.GetDayNightCycleTime() + (float)Math.Floor(DayNightCycle.main.GetDay());
                            }
                            else
                            {
                                //tirednessLastUpdateH.Add(__instance, DayNightCycle.main.GetDayNightCycleTime());
                                tirednessLastUpdateH.Add(__instance, (float)DayNightCycle.main.GetDayNightCycleTime() + (float)Math.Floor(DayNightCycle.main.GetDay()));
                            }
                            if (tirednessLastUpdateD.ContainsKey(__instance))
                            {
                                tirednessLastUpdateD[__instance] = (float)Math.Floor(DayNightCycle.main.GetDay());
                            }
                            else
                            {
                                tirednessLastUpdateD.Add(__instance, (float)Math.Floor(DayNightCycle.main.GetDay()));
                            }
                        }
                    }
                    
                }
            }
        }

        public static PDANotification gameStartSleepIntroNotification01 = new PDANotification();
        public static PDANotification gameStartSleepIntroNotification02 = new PDANotification();
        public static PDANotification gameStartSleepIntroNotification03 = new PDANotification();
        public static PDANotification tiredNotification = new PDANotification();
        public static PDANotification tiredWarningNotification = new PDANotification();

        public static void initAssets()
        {
            gameStartSleepIntroNotification01 = new PDANotification();
            gameStartSleepIntroNotification01.sound = null;
            gameStartSleepIntroNotification01.text = "For your safety, you have been injected with a sleep suppressing serum.";
            gameStartSleepIntroNotification02 = new PDANotification();
            gameStartSleepIntroNotification02.sound = null;
            gameStartSleepIntroNotification02.text = "Your need for sleep has been removed for 15 days.";
            gameStartSleepIntroNotification03 = new PDANotification();
            gameStartSleepIntroNotification03.sound = null;
            gameStartSleepIntroNotification03.text = "Please try to find a way to sleep as soon as possible.";
            tiredNotification = new PDANotification();
            tiredNotification.sound = null;
            tiredNotification.text = "You are getting tired. Getting some sleep is highly recommended.";
            tiredWarningNotification = new PDANotification();
            tiredWarningNotification.sound = null;
            tiredWarningNotification.text = "You are really tired. Find some sleep immediately.";
        }

        public class TirednessSOHData
        {
            public Dictionary<Player, float> tirednessDict;

            public TirednessSOHData()
            {
                if (Tiredness.tirednessDict != null)
                {
                    this.tirednessDict = Tiredness.tirednessDict;
                }
                else
                {
                    this.tirednessDict = new Dictionary<Player, float>();
                }
            }
        }

        public static Dictionary<Player,float> getTirednessDict()
        {
            if (Tiredness.tirednessDict != null)
            {
                return Tiredness.tirednessDict;
            }
            else
            {
                return new Dictionary<Player, float>();
            }
        }
    }
}
