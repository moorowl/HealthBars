using System.Collections.Generic;
using PlayerState;
using Unity.Mathematics;
using UnityEngine;

namespace HealthBars.Utilities {
	public static class EntityMonoUtils {
		private static readonly Vector3 BaseBottomOffset = new(0, 0f, -13f / 16f);
		private static readonly Dictionary<ObjectID, Vector3> CreatureSpecificBottomOffsets = new() {
			{ ObjectID.CrystalBigSnail, new Vector3(0f, 0f, -0.75f) },
			{ ObjectID.SmallTentacle, new Vector3(0f, 0f, -0.275f) },
			{ ObjectID.MoldTentacle, new Vector3(0f, 0f, -0.45f) },
			{ ObjectID.BombScarab, new Vector3(0f, 0f, 0.1f) },
			{ ObjectID.GoldenBombScarab, new Vector3(0f, 0f, 0.1f) },
			{ ObjectID.LavaButterfly, new Vector3(0f, 0f, 0.45f) },
			{ ObjectID.Larva, new Vector3(0f, 0f, -0.3f) },
			{ ObjectID.VoidLarva, new Vector3(0f, 0f, -0.3f) },
			{ ObjectID.OrbitalTurret, new Vector3(0f, 0f, 0.1f) },
			{ ObjectID.CavelingMerchant, new Vector3(-0.15f, 0f, 0f) },
			{ ObjectID.WormSegment, new Vector3(0f, 0f, -0.6f) },
			{ ObjectID.ClayWormSegment, new Vector3(0f, 0f, -0.4f) },
			{ ObjectID.NatureWormSegment, new Vector3(0f, 0f, -0.4f) },
			{ ObjectID.AmoebaWormSegment, new Vector3(0f, 0f, -0.4f) },
			{ ObjectID.CrystalMerchant, new Vector3(0f, 0f, 2f / 16f) },
			{ ObjectID.CrabEnemy, new Vector3(0f, 0f, 3f / 16f) },
			{ ObjectID.RobotPatroller, new Vector3(0f, 0f, 3f / 16f) },
			{ ObjectID.AFPestElectric, new Vector3(0f, 0f, -6f / 16f) },
			{ ObjectID.SnarePlant, new Vector3(0f, 0f, -4f / 16f) },
			{ ObjectID.CicadaNymph, new Vector3(0f, 0f, -4f / 16f) },
			{ ObjectID.BatMinion, new Vector3(0f, 0f, 3f / 16f) },
			{ ObjectID.PickaxeMinion, new Vector3(0f, 0f, -2f / 16f) }
		};
		
		public static Vector3 GetBottomOffset(EntityMonoBehaviour entityMono) {
			var bottom = Vector3.up * 0.5f;
			var objectInfo = entityMono.objectInfo;
			
			bottom += CreatureSpecificBottomOffsets.GetValueOrDefault(objectInfo.objectID);
			bottom += BaseBottomOffset;
			
			if (objectInfo.centerIsAtEntityPosition)
				return bottom;

			EntityUtility.GetPrefabSizeAndOffset(entityMono.entity, objectInfo, out var size, out var offset);
			var vector2Int = size - offset;
			if (vector2Int.x > 1)
				bottom.x += (vector2Int.x - 1) / 2f;
			
			return bottom;
		}

		public static bool IsHidden(EntityMonoBehaviour entityMono) {
			if (entityMono.isHidden || (entityMono.XScaler != null && !entityMono.XScaler.gameObject.activeSelf))
				return true;

			if (EntityUtility.TryGetComponentData<PlayerStateCD>(entityMono.entity, entityMono.world, out var playerStateCD) && PlayerController.IsDyingOrDead(playerStateCD))
				return true;

			return false;
		}
		
		public static bool TryGetNormalizedHealth(EntityMonoBehaviour entityMono, out float health) {
			health = 0f;
			if (!EntityUtility.TryGetComponentData<HealthCD>(entityMono.entity, entityMono.world, out var healthCD))
				return false;

			var currentHealth = entityMono.GetCurrentHealth(in healthCD);
			var maxHealth = entityMono.GetMaxHealth();

			health = math.clamp(currentHealth / (float) maxHealth, 0f, 1f);
			return true;
		}

		public static bool TryGetNormalizedMana(EntityMonoBehaviour entityMono, out float mana) {
			mana = 0f;
			if (!EntityUtility.TryGetComponentData<ManaCD>(entityMono.entity, entityMono.world, out var manaCD))
				return false;

			mana = math.clamp(manaCD.mana / (float) manaCD.maxMana, 0f, 1f);
			return true;
		}
		
		public static bool TryGetNormalizedShield(EntityMonoBehaviour entityMono, out float shield) {
			shield = 0f;
			
			if (!EntityUtility.TryGetComponentData<HealthCD>(entityMono.entity, entityMono.world, out var healthCD))
				return false;

			if (!EntityUtility.TryGetComponentData<EnemyActAsDestructibleCD>(entityMono.entity, entityMono.world, out var enemyActAsDestructibleCD))
				return false;

			var num = (int) (enemyActAsDestructibleCD.healthThreshold * healthCD.maxHealth);
			var num2 = healthCD.maxHealth - num;
			var num3 = (float) (healthCD.health - num) / num2;

			shield = math.clamp(num3, 0f, 1f);
			return true;
		}
		
		public static bool IsHiddenInsideWalls(EntityMonoBehaviour entityMono) {
			return entityMono is WormSegment or OreBoulder;
		}

		public static bool IsImmuneToDamage(EntityMonoBehaviour entityMono) {
			return (EntityUtility.TryGetComponentData<ImmuneToDamageCD>(entityMono.entity, entityMono.world, out var immuneToDamageCD) && immuneToDamageCD.Value == ImmuneToDamageState.Immune)
			       || TryGetNormalizedShield(entityMono, out var shield) && shield > 0f;
		}
	}
}