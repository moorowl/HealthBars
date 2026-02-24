using System;
using System.Collections.Generic;
using System.Linq;
using HealthBars.Utilities;
using Pug.Conversion;
using UnityEngine;

namespace HealthBars.ResourceBars {
	public class ResourceBarAssignmentConverter : Converter {
		private static readonly HashSet<Type> AlreadyModifiedGraphicalObjects = new();

		public override void Convert(GameObject authoring) {
			if (IsServer || Manager.main.currentSceneHandler == null || !authoring.TryGetComponent<IEntityMonoBehaviourData>(out var entityMonoBehaviourData))
				return;
			
			var objectInfo = entityMonoBehaviourData.ObjectInfo;
			var graphicalObject = objectInfo.prefabInfos[0].prefab?.gameObject;
			if (graphicalObject == null)
				return;

			var entityMono = graphicalObject.GetComponent<EntityMonoBehaviour>();
			if (entityMono == null || entityMono.optionalHealthBar != null)
				return;

			var entityMonoType = entityMono.GetType();
			if (AlreadyModifiedGraphicalObjects.Contains(entityMonoType))
				return;
			
			var resourceIds = Main.Resources.Where(resource => resource.AppliesTo(authoring, entityMono, objectInfo)).OrderBy(resource => resource.DisplayOrder).Select(resource => resource.Id).ToArray();
			if (resourceIds.Length == 0)
				return;
			
			GraphicalObjectUtils.ModifyGraphicalObject(entityMonoType, (_, root) => {
				var container = root.AddComponent<ResourceBarContainer>();
				container.resourceIds = resourceIds;
			});
			AlreadyModifiedGraphicalObjects.Add(entityMonoType);
		}
	}
}