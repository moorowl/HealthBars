using HarmonyLib;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace HealthBars {
	[HarmonyPatch]
	public static class OverrideHealthBarColor {
		[HarmonyPatch(typeof(HealthBar), "UpdateHealthBar")]
		[HarmonyPrefix]
		public static void HealthBar_UpdateHealthBar(HealthBar __instance, float value, int protectiveArmorValue, int maxProtectiveArmorValue) {
			if (IsHealthColorDefault(__instance.healthColor))
				__instance.healthColor = Options.Instance.ColorHealth.Rgba;

			if (__instance.armorBar != null)
				__instance.armorBar.color = Options.Instance.ColorShield.Rgba;
		}

		private static bool IsHealthColorDefault(Color color) {
			return Mathf.RoundToInt(color.r * 255f) == 255 && Mathf.RoundToInt(color.g * 255f) == 61 && Mathf.RoundToInt(color.b * 255f) == 61;
		}
	}
}