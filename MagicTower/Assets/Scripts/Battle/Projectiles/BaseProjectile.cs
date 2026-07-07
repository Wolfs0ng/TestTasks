using MagicTower.Battle.Abstract;
using MagicTower.Battle.Data;
using UnityEngine;

namespace MagicTower.Battle.Projectiles
{
	public abstract class BaseProjectile : MonoBehaviour
	{
		[SerializeField] private float lifetime = 5f;
		[SerializeField] private float movementRotationSpeed = 720f;

		private const float MinMovementDeltaSqrMagnitude = 0.000001f;

		private float lifeTimer;
		private bool isInitialized;
		private bool hasHit;

		protected ProjectileContext Context { get; private set; }
		protected bool HasHit => hasHit;

		public void Initialize(ProjectileContext context)
		{
			Context = context;
			lifeTimer = Mathf.Max(0.01f, lifetime);
			hasHit = false;
			isInitialized = true;

			OnInitialized();
		}

		private void Update()
		{
			if (!isInitialized || hasHit)
			{
				return;
			}

			UpdateMovement(Time.deltaTime);

			lifeTimer -= Time.deltaTime;

			if (lifeTimer <= 0f)
			{
				DestroyProjectile();
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!isInitialized || hasHit)
			{
				return;
			}

			if (TryHandleHit(other))
			{
				hasHit = true;
			}
		}

		protected virtual void OnInitialized()
		{
		}

		protected void RotateForwardToMovement(Vector3 previousPosition, float deltaTime)
		{
			Vector3 movementDelta = transform.position - previousPosition;

			if (movementDelta.sqrMagnitude <= MinMovementDeltaSqrMagnitude)
			{
				return;
			}

			Vector3 movementDirection = movementDelta.normalized;
			Quaternion targetRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

			float maxRotationDelta = movementRotationSpeed * deltaTime;
			transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxRotationDelta);
		}

		protected void ApplyDamage(IDamageable target)
		{
			if (target == null || !target.IsAlive)
			{
				return;
			}

			if (Context.Damage <= 0)
			{
				return;
			}

			target.TakeDamage(Context.Damage);
		}

		protected void ApplyPostDamageEffects(IDamageable target)
		{
			if (target == null || Context.PostDamageEffects == null)
			{
				return;
			}

			for (int i = 0; i < Context.PostDamageEffects.Count; i++)
			{
				BaseDamageEffectSO effect = Context.PostDamageEffects[i];

				if (effect == null)
				{
					continue;
				}

				effect.Apply(target);
			}
		}

		protected void MarkHit()
		{
			hasHit = true;
		}

		protected void DestroyProjectile()
		{
			hasHit = true;
			Destroy(gameObject);
		}

		protected abstract void UpdateMovement(float deltaTime);
		protected abstract bool TryHandleHit(Collider hitCollider);
	}
}