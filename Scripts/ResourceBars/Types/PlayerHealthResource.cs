using HealthBars.Utilities;
using PlayerState;
using PugMod;
using Unity.NetCode;
using UnityEngine;

namespace HealthBars.ResourceBars.Types {
	public class PlayerHealthResource : Resource {
		private static NetworkTick _clientTick;
		private static int _lastGotClientTick;
		
		public override int DisplayOrder => 0;
		
		public override bool AppliesTo(GameObject authoring, EntityMonoBehaviour entityMono, ObjectInfo objectInfo) {
			return entityMono is PlayerController;
		}

		public override void GetState(EntityMonoBehaviour entityMono, out float progress, out bool visible, out Color color) {
			progress = 0f;
			visible = false;
			color = HealthResource.ImmuneColor;

			if (entityMono is not PlayerController player)
				return;

			if ((!Options.Instance.ShowOtherPlayerHealth && !player.isLocal) || (!Options.Instance.ShowLocalPlayerHealth && player.isLocal))
				return;

			// Only show other player's health if we're on the same team
			if (player.pvpMode && !player.isLocal && !player.IsPlayersOfSamePvPTeam(Manager.main.player))
				return;

			if (!EntityMonoUtils.TryGetNormalizedHealth(entityMono, out var health))
				return;
			
			if (!EntityMonoUtils.IsImmuneToDamage(entityMono) && !IsParrying(player))
				color = Options.Instance.ColorHealth.Rgba;

			progress = health;
			visible = progress > 0f && progress < 1f && !EntityMonoUtils.IsHidden(entityMono);	
		}

		private static bool IsParrying(PlayerController player) {
			if (EntityUtility.TryGetComponentData<UseOffHandStateCD>(player.entity, player.world, out var useOffHandStateCD)) {
				if (_lastGotClientTick != Time.frameCount) {
					_clientTick = API.Client.GetEntityQuery(typeof(NetworkTime)).GetSingleton<NetworkTime>().InterpolationTick;
					_lastGotClientTick = Time.frameCount;
				}

				if (useOffHandStateCD.IsParrying(_clientTick))
					return true;
			}

			return false;
		}
	}
}