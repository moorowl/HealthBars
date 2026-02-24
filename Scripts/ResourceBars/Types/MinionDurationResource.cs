using HealthBars.Utilities;
using UnityEngine;

namespace HealthBars.ResourceBars.Types {
	public class MinionDurationResource : Resource {
		public override int DisplayOrder => 1;
		
		public override bool AppliesTo(GameObject authoring, EntityMonoBehaviour entityMono, ObjectInfo objectInfo) {
			return entityMono is MinionBase;
		}

		public override void GetState(EntityMonoBehaviour entityMono, out float progress, out bool visible, out Color color) {
			progress = 0f;
			visible = false;
			color = Options.Instance.ColorDuration.Rgba;

			if (!Options.Instance.ShowMinionDuration || !EntityUtility.TryGetComponentData<MinionCD>(entityMono.entity, entityMono.world, out var minionCD))
				return;

			if (!EntityUtility.TryGetComponentData<OwnerReferenceCD>(entityMono.entity, entityMono.world, out var ownerReferenceCD))
				return;

			if (Manager.memory.GetEntityMono(ownerReferenceCD.owner) is not PlayerController { isLocal: true } || entityMono.isHidden)
				return;

			progress = minionCD.normalizedLifespanTimer;
			visible = progress > 0f && progress < 1f && !EntityMonoUtils.IsHidden(entityMono);
		}
	}
}