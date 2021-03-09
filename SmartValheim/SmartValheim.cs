using System;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace SmartValheim
{
    [BepInPlugin("jp.refactor.valheim.plugins.SmartValheim", "SmartValheim Plug-In", "1.0.0.0")]
    public class SmartValheim : BaseUnityPlugin
    {
        public static ManualLogSource MyLogSource;

        void Awake()
        {
            MyLogSource = new ManualLogSource("SmartValheim");
            BepInEx.Logging.Logger.Sources.Add(MyLogSource);

            Harmony.CreateAndPatchAll(typeof(SmartValheim));

            MyLogSource.LogInfo("Awaked.");
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Player), "ActivateGuardianPower")]
        private static void ActivateGuardianPower_Prefix(Player __instance)
        {
            MyLogSource.LogInfo("ActivateGuardianPower_Prefix");
            // クールダウンの時間をパワーの効果時間と同じにする
            __instance.m_guardianSE.m_cooldown = __instance.m_guardianSE.m_ttl;
        }
    }
}
