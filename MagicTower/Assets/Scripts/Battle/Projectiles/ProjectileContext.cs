using System.Collections.Generic;
using MagicTower.Battle.Abstract;
using MagicTower.Battle.Controllers;
using MagicTower.Battle.Data;
using UnityEngine;

namespace MagicTower.Battle.Projectiles
{
	public readonly struct ProjectileContext
	{
		public readonly Vector3 StartPosition;
		public readonly Vector3 Direction;
		public readonly Vector3 TargetPosition;
		public readonly IDamageable Target;
		public readonly int Damage;
		public readonly float Speed;
		public readonly float ExplosionRadius;
		public readonly GameObject ExplosionVfxPrefab;
		public readonly float ExplosionVfxLifetime;
		public readonly float ArcHeight;
		public readonly EnemyManager EnemyProvider;
		public readonly IReadOnlyList<BaseDamageEffectSO> PostDamageEffects;

		public ProjectileContext(Vector3 startPosition, Vector3 direction, Vector3 targetPosition,
			IDamageable target, int damage, float speed, float explosionRadius, GameObject explosionVfxPrefab,
			float explosionVfxLifetime, float arcHeight, EnemyManager enemyProvider,
			IReadOnlyList<BaseDamageEffectSO> postDamageEffects)
		{
			StartPosition = startPosition;
			Direction = direction.normalized;
			TargetPosition = targetPosition;
			Target = target;
			Damage = damage;
			Speed = speed;
			ExplosionRadius = explosionRadius;
			ExplosionVfxPrefab = explosionVfxPrefab;
			ExplosionVfxLifetime = explosionVfxLifetime;
			ArcHeight = arcHeight;
			EnemyProvider = enemyProvider;
			PostDamageEffects = postDamageEffects;
		}
	}
}