using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using Harmony;

namespace Subnautica_Enhanced_Sleep
{
    class SleepPatcher
    {
        public static FMODAsset FMA_nosleep_tooThirsty = new FMODAsset();


        public static float wentToSleep = 0;

        public static void invokeAssets()
        {

        }

        [HarmonyPatch(typeof(Bed))]
        [HarmonyPatch("EnterInUseMode")]
        private class EnterPatch
        {
            private static bool Prefix(Bed __instance)
            {
                wentToSleep = DayNightCycle.main.GetDayNightCycleTime();
                return true;
            }
        }

        [HarmonyPatch(typeof(Bed))]
        [HarmonyPatch("OnHandClick")]
        private class HandPatch
        {
            private static bool Prefix(Bed __instance, GUIHand hand)
            {
                if (hand.player.GetComponent<Survival>().food <= 25 || Player.main.GetComponent<Survival>().water <= 25)
                {
                    if (hand.player.GetComponent<Survival>().food <= 25 &&
                        hand.player.GetComponent<Survival>().water <= 25)
                    {
                        ErrorMessage.AddWarning("You can't sleep now because you are too hungry and thirsty! Drink and eat something before you go to bed.");
                    } else if (hand.player.GetComponent<Survival>().food <= 25 ||
                               !(hand.player.GetComponent<Survival>().water <= 25))
                    {
                        ErrorMessage.AddWarning("You can't sleep now because you are too hungry! Go eat something before you go to bed.");
                    } else if (!(hand.player.GetComponent<Survival>().food <= 25) ||
                               hand.player.GetComponent<Survival>().water <= 25)
                    {
                        ErrorMessage.AddWarning("You can't sleep now because you are too thirsty! Drink something before you go to bed.");
                    }
                    else
                    {
                        ErrorMessage.AddWarning("You can't sleep now! Try to drink and eat something and try again!");
                    }
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(Bed))]
        [HarmonyPatch("ExitInUseMode")]
        private class ExitPatch
        {
            private static void Postfix(Bed __instance, Player player)
            {
                player.liveMixin.health = player.liveMixin.maxHealth;
                player.liveMixin.health = player.liveMixin.maxHealth;
                Survival sv = player.GetComponent<Survival>();
                float stoodUp = DayNightCycle.main.GetDayNightCycleTime();
                float foodBefore = sv.food;
                float waterBefore = sv.water;
                float timeSlept = 0;
                if (wentToSleep > 0.5)
                {
                    timeSlept = (((DayNightCycle.main.GetDayNightCycleTime() + 1) - wentToSleep));
                }
                else
                {
                    timeSlept = (((DayNightCycle.main.GetDayNightCycleTime() + 0) - wentToSleep));
                }

                double hourTimeSlept = timeSlept / (1d / 24);
                double minuteTimeSlept = hourTimeSlept * 60;

                double looseFactor = 0.5;
                double foodLost = minuteTimeSlept / (25.20 / looseFactor); // factor * looseFactor : looseFactor would be 2 to halfe the time.
                double waterLost = minuteTimeSlept / (18.00 / looseFactor);
                float foodAfter = foodBefore - (float)foodLost;
                float waterAfter = waterBefore - (float)waterLost;
                if (foodAfter > 5 ) {sv.food = foodAfter;} else {sv.food = 5;}
                if (waterAfter > 5) { sv.water = waterAfter; } else { sv.water = 5; }
                Main.Log("!!Left Bed:\nWent to Bed Time: " + wentToSleep + "\nWoke up: " + stoodUp + "\nDuration: " + timeSlept + "\nDuration in IGHours: " + hourTimeSlept + "\nDuration in IGMinutes: " + minuteTimeSlept + "\nFood Before: " + sv.food + "\nFood Lost: " + foodLost + "\nFood After: " + sv.food + "\nWater Before: " + waterBefore + "\nWater Lost: " + waterLost + "\nWater After: " + sv.water);
                
                
            }
        }
    }
}
