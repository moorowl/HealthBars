using HealthBars.Utilities;
using Pug.UnityExtensions;
using PugTilemap;
using Unity.Mathematics;
using UnityEngine;

namespace HealthBars.ResourceBars.Types {
	public class HealthResource : Resource {
		private static readonly Color ImmuneColor = new(0.55f, 0.55f, 0.55f, 1f);
		private const float HideAfterDuration = 0.1f;

		private float _hideTime;

		public override int DisplayOrder => 0;

		public override bool AppliesTo(GameObject authoring, EntityMonoBehaviour entityMono, ObjectInfo objectInfo) {
			if (authoring.TryGetComponent<HealthAuthoring>(out var healthAuthoring) && healthAuthoring.maxHealth > 1) {
				return objectInfo.objectType == ObjectType.Creature
				                   || objectInfo.objectID == ObjectID.BirdBossStone
				                   || entityMono is OreBoulder;
			}

			return false;
		}

		public override void GetState(EntityMonoBehaviour entityMono, out float progress, out bool visible, out Color color) {
			progress = 0f;
			visible = false;
			color = EntityMonoUtils.IsImmuneToDamage(entityMono) ? ImmuneColor : Options.Instance.ColorHealth.Rgba;

			// TODO Ore boulder hp should probably be moved into its own resource bar
			if ((entityMono is not OreBoulder && !Options.Instance.ShowCreatureHealth) || (entityMono is OreBoulder && !Options.Instance.ShowOreBoulderProgress))
				return;
			
			if (!EntityUtility.TryGetComponentData<HealthCD>(entityMono.entity, entityMono.world, out var healthCD))
				return;
			
			var currentHealth = entityMono.GetCurrentHealth(in healthCD);
			var maxHealth = entityMono.GetMaxHealth();

			// Cocoons
			if (EntityUtility.HasComponentData<HatchWhenPlayerNearbyStateCD>(entityMono.entity, entityMono.world) && currentHealth == 1)
				currentHealth = 0;
			
			var isHidden = ShouldBeHidden(entityMono);
			if (isHidden) {
				_hideTime += Time.deltaTime;
				isHidden = _hideTime > HideAfterDuration;
			} else {
				_hideTime = 0f;
			}

			progress = math.clamp(currentHealth / (float) maxHealth, 0f, 1f);
			visible = progress > 0f && progress < 1f && !isHidden && !EntityMonoUtils.IsHidden(entityMono);
		}

		private static bool ShouldBeHidden(EntityMonoBehaviour entityMono) {
			if (EntityMonoUtils.IsHiddenInsideWalls(entityMono)) {
				var tileLookup = Manager.multiMap.GetTileLayerLookup();
				return tileLookup.HasTile(entityMono.WorldPosition.RoundToInt2(), TileType.wall);
			}

			if (EntityUtility.TryGetComponentData<StateInfoCD>(entityMono.entity, entityMono.world, out var stateInfoCD))
				return stateInfoCD.IsCurrentState(StateID.Bush);

			return false;
		}
	}
}