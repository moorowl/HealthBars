using HealthBars.Utilities;
using UnityEngine;

namespace HealthBars.ResourceBars.Types {
	public class PlayerHealthResource : Resource {
		public override int DisplayOrder => 0;
		
		public override bool AppliesTo(GameObject authoring, EntityMonoBehaviour entityMono, ObjectInfo objectInfo) {
			return entityMono is PlayerController;
		}

		public override void GetState(EntityMonoBehaviour entityMono, out float progress, out bool visible, out Color color) {
			progress = 0f;
			visible = false;
			color = Options.Instance.ColorHealth.Rgba;

			if (entityMono is not PlayerController player)
				return;

			if ((!Options.Instance.ShowOtherPlayerHealth && !player.isLocal) || (!Options.Instance.ShowLocalPlayerHealth && player.isLocal))
				return;

			if (!player.isLocal && !player.IsPlayersOfSamePvPTeam(Manager.main.player))
				return;

			if (!EntityMonoUtils.TryGetNormalizedHealth(entityMono, out var health))
				return;

			progress = health;
			visible = progress > 0f && progress < 1f && !EntityMonoUtils.IsHidden(entityMono);	
		}
	}
}