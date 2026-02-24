using HealthBars.Utilities;
using Unity.Mathematics;
using UnityEngine;

namespace HealthBars.ResourceBars.Types {
	public class OreBoulderProgressResource : Resource {
		public override int DisplayOrder => 1;
		public override bool LerpLargeHits => false;

		public override bool AppliesTo(GameObject authoring, EntityMonoBehaviour entityMono, ObjectInfo objectInfo) {
			return entityMono is OreBoulder;
		}

		public override void GetState(EntityMonoBehaviour entityMono, out float progress, out bool visible, out Color color) {
			progress = 0f;
			visible = false;
			color = Options.Instance.ColorDuration.Rgba;

			if (!Options.Instance.ShowOreBoulderProgress || !EntityUtility.TryGetComponentData<HealthCD>(entityMono.entity, entityMono.world, out var healthCD))
				return;

			if (!EntityUtility.TryGetComponentData<DropsLootWhenDamagedCD>(entityMono.entity, entityMono.world, out var dropsLootWhenDamagedCD))
				return;
			
			var damageToDeal = (float) dropsLootWhenDamagedCD.damageToDealToDropLoot;

			progress = math.clamp(1f - (healthCD.health % damageToDeal / damageToDeal), 0f, 1f);
			visible = healthCD.health > 0 && healthCD.health < healthCD.maxHealth && !EntityMonoUtils.IsHidden(entityMono);
		}
	}
}