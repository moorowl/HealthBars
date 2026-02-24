using Pug.UnityExtensions;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace HealthBars.ResourceBars {
	public class ResourceBar : MonoBehaviour {
		private const float FillOpacityMultiplier = 0.9f;
		private const float BackgroundOpacityMultiplier = 0.4f;
		private const float LargeHitFillOpacityMultiplier = 0f;

		public GameObject barRoot;
		public GameObject barMaskPivot;
		public SpriteRenderer bar;
		public GameObject largeHitBarMaskPivot;
		public SpriteRenderer largeHitBar;
		public SpriteRenderer barBackground;
		public int barWidth = 20;
		public int barHeight = 4;

		private Resource _resource;
		private Entity _lastEntity;
		
		private float _opacity;
		private float _progress;
		private Color _color;
		private float _largeHitProgress;
		private float _timeLastDamaged;

		public bool Visible => barRoot.activeSelf;
		public bool ForceVisible { get; set; }

		private void Awake() {
			barRoot.SetActive(false);	
		}
		
		public void SetResource(Resource resource) {
			_resource = resource;
		}
		
		public void UpdateState(EntityMonoBehaviour entityMono) {
			_resource.GetState(entityMono, out var progress, out var visible, out var color);
			
			// Update progress
			var progressLerpSpeed = 0f;
			if (_lastEntity == entityMono.entity) {
				var progressChange = _progress - progress;
				if (_resource.LerpHealing && progressChange < 0f && math.abs(progressChange) >= _resource.HealingLerpThreshold)
					progressLerpSpeed = _resource.HealingLerpSpeed;
				if (_resource.LerpDamage && progressChange > 0f && math.abs(progressChange) >= _resource.DamageLerpThreshold)
					progressLerpSpeed = _resource.DamageLerpSpeed;
			}
			
			_progress = progressLerpSpeed > 0f ? math.lerp(_progress, progress, progressLerpSpeed * Time.deltaTime) : progress;
			if (math.distance(_progress, progress) < 0.05f)
				_progress = progress;
			
			if (!Mathf.Approximately(progress, _progress))
				_timeLastDamaged = Time.time;

			if (Options.Instance.EmphasizeLargeHits && _resource.LerpLargeHits && Time.time >= _timeLastDamaged + _resource.LargeHitHoldTime && progress < _largeHitProgress)
				_largeHitProgress = math.lerp(_largeHitProgress, progress, _resource.LargeHitLerpSpeed * Time.deltaTime);
			else
				_largeHitProgress = _progress;

			// Update opacity
			_opacity = math.lerp(_opacity, visible || ForceVisible ? 1f : 0f, _resource.OpacityLerpSpeed * Time.deltaTime);

			_color = color;
			_lastEntity = entityMono.entity;
		}
		
		public void UpdateVisuals(EntityMonoBehaviour entityMono) {
			bar.color = _color.ColorWithNewAlpha(_opacity * FillOpacityMultiplier);
			barBackground.color = barBackground.color.ColorWithNewAlpha(_opacity * BackgroundOpacityMultiplier);
			barMaskPivot.transform.localScale = new Vector3(_progress, 1f, 1f);
			barRoot.SetActive(_opacity > 0.05f);

			if (largeHitBar != null && largeHitBarMaskPivot != null) {
				largeHitBar.color = largeHitBar.color.ColorWithNewAlpha(_opacity * LargeHitFillOpacityMultiplier);
				largeHitBarMaskPivot.transform.localScale = new Vector3(_largeHitProgress, 1f, 1f);
			}
		}

		public void SetWidth(int width) {
			var backgroundSize = new Vector2(width / 16f, barHeight / 16f);
			var fillSize = new Vector2(backgroundSize.x - (2f / 16f), backgroundSize.y - (2f / 16f));
			if (bar == null || bar.size == fillSize)
				return;

			bar.size = fillSize;
			barBackground.size = backgroundSize;
			barMaskPivot.transform.localPosition = new Vector3(((0f - width) * 0.0625f * 0.5f) + 0.05f, 0f, 0f);
			bar.transform.localPosition = new Vector3((backgroundSize.x * 0.5f) - 0.05f, 0f, 0f);

			if (largeHitBar != null) {
				largeHitBar.size = bar.size;
				largeHitBar.transform.localPosition = bar.transform.localPosition;
			}
			
			if (largeHitBarMaskPivot != null)
				largeHitBarMaskPivot.transform.localPosition = barMaskPivot.transform.localPosition;
		}
		
		private void OnValidate() {
			SetWidth(barWidth);
		}
	}
}