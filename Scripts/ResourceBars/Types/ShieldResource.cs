using HealthBars.Utilities;
using UnityEngine;

namespace HealthBars.ResourceBars.Types {
	public class ShieldResource : Resource {
		public override int DisplayOrder => 1;
		
		public override bool AppliesTo(GameObject authoring, EntityMonoBehaviour entityMono, ObjectInfo objectInfo) {
			return entityMono is CrystalBigSnail;
		}

		public override void GetState(EntityMonoBehaviour entityMono, out float progress, out bool visible, out Color color) {
			progress = 0f;
			visible = false;
			color = Options.Instance.ColorShield.Rgba;

			if (!Options.Instance.ShowCreatureHealth || !EntityMonoUtils.TryGetNormalizedShield(entityMono, out var shield))
				return;

			progress = shield;
			visible = progress > 0f && progress < 1f && !EntityMonoUtils.IsHidden(entityMono);	
		}
	}
}