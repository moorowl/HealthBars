using UnityEngine;

namespace HealthBars.ResourceBars {
	public abstract class Resource {
		internal int Id { get; set; }

		public virtual int DisplayOrder => 0;
		public virtual bool LerpHealing => true;
		public virtual float HealingLerpSpeed => 5f;
		public virtual float HealingLerpThreshold => 0.15f;
		public virtual bool LerpDamage => true;
		public virtual float DamageLerpSpeed => 15f;
		public virtual float DamageLerpThreshold => 0.1f;
		public virtual float OpacityLerpSpeed => 25f;
		public virtual bool LerpLargeHits => true;
		public virtual float LargeHitLerpSpeed => 7.5f;
		public virtual float LargeHitHoldTime => 0.5f;

		public abstract bool AppliesTo(GameObject authoring, EntityMonoBehaviour entityMono, ObjectInfo objectInfo);
		
		public abstract void GetState(EntityMonoBehaviour entityMono, out float progress, out bool visible, out Color color);
	}
}