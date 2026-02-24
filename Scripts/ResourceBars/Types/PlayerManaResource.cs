using HealthBars.Utilities;
using UnityEngine;

namespace HealthBars.ResourceBars.Types {
	public class PlayerManaResource : Resource {
		public override int DisplayOrder => 1;
		
		public override bool AppliesTo(GameObject authoring, EntityMonoBehaviour entityMono, ObjectInfo objectInfo) {
			return entityMono is PlayerController;
		}

		public override void GetState(EntityMonoBehaviour entityMono, out float progress, out bool visible, out Color color) {
			progress = 0f;
			visible = false;
			color = Options.Instance.ColorMana.Rgba;

			if (entityMono is not PlayerController player)
				return;

			if ((!Options.Instance.ShowOtherPlayerHealth && !player.isLocal) || (!Options.Instance.ShowLocalPlayerHealth && player.isLocal))
				return;
			
			if (!player.isLocal && !player.IsPlayersOfSamePvPTeam(Manager.main.player))
				return;

			if (!EntityMonoUtils.TryGetNormalizedMana(entityMono, out var mana))
				return;

			progress = mana;
			visible = PugDatabase.HasComponent<ConsumesManaCD>(player.GetHeldObject().objectID) && progress < 1f && !EntityMonoUtils.IsHidden(entityMono);
		}
	}
}