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

        [HarmonyPrefix]
        [HarmonyPatch(typeof(SE_Stats), "ModifyMaxCarryWeight")]
        private static void ModifyMaxCarryWeight_Prefix(SE_Stats __instance, float baseLimit, ref float limit)
        {
            //MyLogSource.LogInfo("ModifyMaxCarryWeight_Prefix");   // ここ毎フレ呼ばれる
            // 重量上限の効果をさらに追加
            limit += __instance.m_addMaxCarryWeight;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Player), "UpdateMovementModifier")]
        private static void UpdateMovementModifier_Postfix(Player __instance)
        {
            // 移動速度がマイナス補正されていて、メギンギョルズを装備している場合は、補正無しにする
            if (__instance.m_equipmentMovementModifier < 0 && __instance.GetSEMan().HaveStatusEffect("BeltStrength"))
            {
                __instance.m_equipmentMovementModifier = 0;
            }
        }
    }
}
