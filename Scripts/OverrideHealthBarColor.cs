using HarmonyLib;
// ReSharper disable InconsistentNaming

namespace HealthBars {
	[HarmonyPatch]
	public static class OverrideHealthBarColor {
		[HarmonyPatch(typeof(HealthBar), "UpdateHealthBar")]
		[HarmonyPrefix]
		public static void HealthBar_UpdateHealthBar(HealthBar __instance, float value, int protectiveArmorValue, int maxProtectiveArmorValue) {
			__instance.healthColor = Options.Instance.ColorHealth.Rgba;
			
			if (__instance.armorBar != null)
				__instance.armorBar.color = Options.Instance.ColorShield.Rgba;
		}
	}
}