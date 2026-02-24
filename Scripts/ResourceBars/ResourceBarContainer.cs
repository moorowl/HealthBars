using HealthBars.Utilities;
using UnityEngine;

namespace HealthBars.ResourceBars {
	public class ResourceBarContainer : MonoBehaviour {
		private static readonly Vector3 OffsetDisplayOver = new(0f, 1.625f, -1.625f);
		private static readonly Vector3 OffsetDisplayBehind = new(0f, 0.1f, -0.1f);

		public int[] resourceIds;

		private EntityMonoBehaviour _entityMono;
		private ResourceBar[] _bars;
		private Transform _transform;
		
		private void Awake() {
			_entityMono = GetComponentInParent<EntityMonoBehaviour>();
		}
		
		private void Start() {
			_bars = new ResourceBar[resourceIds.Length];

			var container = new GameObject("ResourceBarContainer");
			container.transform.parent = transform;
			_transform = container.transform;

			var top = 0f;
			for (var i = 0; i < resourceIds.Length; i++) {
				if (!Main.ResourceById.TryGetValue(resourceIds[i], out var resource))
					continue;
				
				var bar = Instantiate(Main.ResourceBarPrefab, _transform);
				bar.SetResource(resource);
				bar.transform.localPosition = new Vector3(0f, 0f, top);
				top -= bar.barHeight / 16f;

				_bars[i] = bar;
			}
		}
		
		private void Update() {
			if (_entityMono == null || !_entityMono.entityExist)
				return;
			
			for (var i = _bars.Length - 1; i >= 0; i--) {
				var bar = _bars[i];
				bar.ForceVisible = i < _bars.Length - 1 && _bars[i + 1].Visible;
				bar.UpdateState(_entityMono);
				bar.UpdateVisuals(_entityMono);
			}
			
			var isHidden = Manager.prefs.hideInGameUI || (Manager.main.player != null && Manager.main.player.guestMode);
			var displayOverObjects = Options.Instance.DisplayOverObjects || EntityMonoUtils.IsHiddenInsideWalls(_entityMono);
			var offset = EntityMonoUtils.GetBottomOffset(_entityMono) + (displayOverObjects ? OffsetDisplayOver : OffsetDisplayBehind);
			
			_transform.localPosition = offset;
			_transform.localScale = isHidden ? Vector3.zero : new Vector3(1f / _transform.parent.localScale.x, 1f / _transform.parent.localScale.y, 1f / _transform.parent.localScale.z);
		}
	}
}