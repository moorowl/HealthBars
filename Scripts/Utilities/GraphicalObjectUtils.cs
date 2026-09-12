using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PugMod;
using UnityEngine;

namespace HealthBars.Utilities {
	public static class GraphicalObjectUtils {
		private static readonly MemberInfo MiPools = typeof(MemoryManager).GetMembersChecked().FirstOrDefault(x => x.GetNameChecked() == "_pools");
		private static readonly MemberInfo MiPrefab = typeof(PoolSystem).GetMembersChecked().FirstOrDefault(x => x.GetNameChecked() == "_prefab");
		private static readonly MemberInfo MiAutoParent = typeof(PoolSystem).GetMembersChecked().FirstOrDefault(x => x.GetNameChecked() == "_autoParent");

		public delegate void ModifierDelegate(EntityMonoBehaviour entityMono, GameObject root);
		
		public static void ModifyGraphicalObject(DataBlockAddress poolId, ModifierDelegate modifier) {
			Manager.RunAfterInitComplete(ModifyGraphicalObjectCoroutine(poolId, modifier));
		}

		private static IEnumerator ModifyGraphicalObjectCoroutine(DataBlockAddress poolId, ModifierDelegate modifier) {
			var pools = (Dictionary<DataBlockAddress, PoolSystem>) API.Reflection.GetValue(MiPools, Manager.memory);

			if (pools.TryGetValue(poolId, out var pool)) {
				var prefab = (GameObject) MiPrefab.GetValueChecked(pool);
				var autoParent = (Transform) MiAutoParent.GetValueChecked(pool);
			
				if (prefab != null && autoParent != null) {
					// Modify the main prefab
					if (prefab.TryGetComponent<EntityMonoBehaviour>(out var prefabEntityMono))
						modifier(prefabEntityMono, prefab);
			
					// Modify the objects that have already been allocated
					foreach (var allocatedEntityMono in autoParent.GetComponentsInChildren<EntityMonoBehaviour>(true))
						modifier(allocatedEntityMono, allocatedEntityMono.gameObject);	
				}	
			}

			yield return null;
		}
	}
}