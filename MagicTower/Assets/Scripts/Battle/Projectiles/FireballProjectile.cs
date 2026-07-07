using System.Collections.Generic;
using MagicTower.Battle.Abstract;
using MagicTower.Battle.Controllers;
using MagicTower.Battle.Data;
using UnityEngine;

namespace MagicTower.Battle.Projectiles
{
	public class FireballProjectile : BaseProjectile
	{
		private const float ArrivalDistance = 0.1f;

		private bool hasExploded;

		protected override void OnInitialized()
		{
			transform.position = Context.StartPosition;

			Vector3 direction = Context.TargetPosition - Context.StartPosition;

			if (direction.sqrMagnitude <= 0.001f)
			{
				Explode();
				return;
			}

			transform.rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
		}

		protected override void UpdateMovement(float deltaTime)
		{
			if (hasExploded)
			{
				return;
			}

			Vector3 previousPosition = transform.position;

			transform.position = Vector3.MoveTowards(
				transform.position,
				Context.TargetPosition,
				Context.Speed * deltaTime);

			RotateForwardToMovement(previousPosition, deltaTime);

			if (Vector3.Distance(transform.position, Context.TargetPosition) <= ArrivalDistance)
			{
				Explode();
			}
		}

		protected override bool TryHandleHit(Collider hitCollider)
		{
			if (hasExploded)
			{
				return false;
			}

			Explode();
			return true;
		}

		private void Explode()
		{
			if (hasExploded)
			{
				return;
			}

			hasExploded = true;

			SpawnExplosionVfx();
			DamageEnemiesInRadius();

			DestroyProjectile();
		}

		private void DamageEnemiesInRadius()
		{
			if (Context.EnemyProvider == null)
			{
				Debug.LogWarning($"{nameof(FireballProjectile)} on {name}: missing EnemyProvider.", this);
				return;
			}

			List<EnemyController> aliveEnemies = Context.EnemyProvider.GetAliveEnemies();
			float sqrRadius = Context.ExplosionRadius * Context.ExplosionRadius;
			Vector3 explosionPosition = transform.position;

			for (int i = 0; i < aliveEnemies.Count; i++)
			{
				EnemyController enemy = aliveEnemies[i];

				if (enemy == null || !enemy.IsAlive)
				{
					continue;
				}

				Vector3 offset = enemy.Transform.position - explosionPosition;

				if (offset.sqrMagnitude > sqrRadius)
				{
					continue;
				}

				ApplyDamage(enemy);
				ApplyPostDamageEffects(enemy);
			}
		}

		private void SpawnExplosionVfx()
		{
			if (Context.ExplosionVfxPrefab == null)
			{
				return;
			}

			GameObject explosionVfx = Instantiate(
				Context.ExplosionVfxPrefab,
				transform.position,
				Quaternion.identity);

			if (explosionVfx == null)
			{
				return;
			}

			float visualDiameter = Mathf.Max(0.01f, Context.ExplosionRadius * 2f);
			explosionVfx.transform.localScale = Vector3.one * visualDiameter;

			if (Context.ExplosionVfxLifetime > 0f)
			{
				Destroy(explosionVfx, Context.ExplosionVfxLifetime);
			}
		}
	}
}