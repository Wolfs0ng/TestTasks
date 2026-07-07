using MagicTower.Battle.Abstract;
using UnityEngine;

namespace MagicTower.Battle.Projectiles
{
	public class BarrageProjectile : BaseProjectile
	{
		private const float ParabolaHeightMultiplier = 4f;

		private Vector3 startPosition;
		private float travelDistance;
		private float progress;

		protected override void OnInitialized()
		{
			startPosition = Context.StartPosition;
			transform.position = startPosition;
			progress = 0f;

			if (Context.Target == null || Context.Target.Transform == null)
			{
				DestroyProjectile();
				return;
			}

			travelDistance = Vector3.Distance(startPosition, Context.Target.Transform.position);

			if (travelDistance <= 0.001f)
			{
				HitTarget();
			}
		}

		protected override void UpdateMovement(float deltaTime)
		{
			if (Context.Target == null || !Context.Target.IsAlive || Context.Target.Transform == null)
			{
				DestroyProjectile();
				return;
			}

			Vector3 previousPosition = transform.position;

			float travelTime = travelDistance / Mathf.Max(0.01f, Context.Speed);
			progress += deltaTime / Mathf.Max(0.01f, travelTime);
			progress = Mathf.Clamp01(progress);

			Vector3 targetPosition = Context.Target.Transform.position;
			Vector3 basePosition = Vector3.Lerp(startPosition, targetPosition, progress);

			float heightOffset = ParabolaHeightMultiplier * Context.ArcHeight * progress * (1f - progress);
			basePosition.y += heightOffset;

			transform.position = basePosition;

			RotateForwardToMovement(previousPosition, deltaTime);

			if (progress >= 1f)
			{
				HitTarget();
			}
		}

		protected override bool TryHandleHit(Collider hitCollider)
		{
			if (Context.Target == null || !Context.Target.IsAlive)
			{
				DestroyProjectile();
				return true;
			}

			IDamageable hitDamageable = hitCollider.GetComponentInParent<IDamageable>();

			if (hitDamageable == null)
			{
				return false;
			}

			if (hitDamageable != Context.Target)
			{
				return false;
			}

			HitTarget();
			return true;
		}

		private void HitTarget()
		{
			if (HasHit)
			{
				return;
			}

			if (Context.Target == null || !Context.Target.IsAlive)
			{
				DestroyProjectile();
				return;
			}

			MarkHit();

			ApplyDamage(Context.Target);
			ApplyPostDamageEffects(Context.Target);

			DestroyProjectile();
		}
	}
}