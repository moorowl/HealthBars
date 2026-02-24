using System;
using System.Collections.Generic;
using System.Linq;
using HealthBars.ResourceBars;
using PugMod;
using UnityEngine;
using Object = UnityEngine.Object;

namespace HealthBars {
	public class Main : IMod {
		public const string Version = "1.4";
		public const string InternalName = "HealthBars";
		public const string DisplayName = "Enemy Health Bars";
		
		internal static AssetBundle AssetBundle { get; private set; }
		internal static ResourceBar ResourceBarPrefab { get; private set; }
		
		internal static List<Resource> Resources { get; private set; }
		internal static Dictionary<int, Resource> ResourceById { get; private set; }

		public void EarlyInit() {
			Debug.Log($"[{DisplayName}]: Mod version: {Version}");

			var modInfo = API.ModLoader.LoadedMods.FirstOrDefault(modInfo => modInfo.Handlers.Contains(this));
			AssetBundle = modInfo!.AssetBundles[0];
			
			Options.Instance.Init();
			
			InitResourceLookups();
		}

		public void Init() { }

		public void Shutdown() { }

		public void ModObjectLoaded(Object obj) {
			if (obj is GameObject gameObject && gameObject.TryGetComponent<ResourceBar>(out var resourceBar))
				ResourceBarPrefab = resourceBar;
		}

		public void Update() {
			Options.Instance.Update();
		}
		
		private static void InitResourceLookups() {
			Resources = new List<Resource>();
			ResourceById = new Dictionary<int, Resource>();

			// The modId parameter of GetTypes currently does nothing. This might change in the future?
			foreach (var type in API.Reflection.GetTypes(0)) {
				if (!type.IsAbstract && typeof(Resource).IsAssignableFrom(type)) {
					var resource = (Resource) Activator.CreateInstance(type);
					resource.Id = ResourceById.Count;
					
					Resources.Add(resource);
					ResourceById.TryAdd(resource.Id, resource);

					Debug.Log($"[{DisplayName}]: Added resource {resource.Id} {resource.GetType().GetNameChecked()}");
				}
			}
		}
	}
}