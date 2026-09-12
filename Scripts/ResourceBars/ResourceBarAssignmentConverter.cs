using System;
using System.Collections.Generic;
using System.Linq;
using HealthBars.Utilities;
using Pug.Conversion;
using UnityEngine;

namespace HealthBars.ResourceBars {
	public class ResourceBarAssignmentConverter : Converter {
		private static readonly HashSet<DataBlockAddress> AlreadyModifiedGraphicalObjects = new();

		public override void Convert(GameObject authoring) {
			if (IsServer || Manager.main.currentSceneHandler == null || !authoring.TryGetComponent<IEntityMonoBehaviourData>(out var entityMonoBehaviourData))
				return;
			
			var objectInfo = entityMonoBehaviourData.ObjectInfo;
			var graphicalObjectRef = objectInfo.prefabInfo.graphicalRef;

			if (!graphicalObjectRef.hasAddress)
				return;

			var entityMono = objectInfo.prefabInfo.GetGraphical()?.GetComponent<EntityMonoBehaviour>();
			if (entityMono == null || entityMono.optionalHealthBar != null || AlreadyModifiedGraphicalObjects.Contains(graphicalObjectRef.address))
				return;

			var resourceIds = Main.Resources.Where(resource => resource.AppliesTo(authoring, entityMono, objectInfo)).OrderBy(resource => resource.DisplayOrder).Select(resource => resource.Id).ToArray();
			if (resourceIds.Length == 0)
				return;
			
			GraphicalObjectUtils.ModifyGraphicalObject(graphicalObjectRef.address, (_, root) => {
				var container = root.AddComponent<ResourceBarContainer>();
				container.resourceIds = resourceIds;
			});
			AlreadyModifiedGraphicalObjects.Add(graphicalObjectRef.address);
		}
	}
}