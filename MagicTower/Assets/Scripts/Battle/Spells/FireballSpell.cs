using MagicTower.Battle.Controllers;
using MagicTower.Battle.Projectiles;
using UnityEngine;

namespace MagicTower.Battle.Spells
{
	public class FireballSpell : BaseSpell
	{
		private Vector3 selectedTargetPosition;
		private bool hasSelectedTarget;

		protected override bool TrySelectTargets(SpellCastContext context)
		{
			hasSelectedTarget = false;
			selectedTargetPosition = Vector3.zero;

			if (context.EnemyProvider == null)
			{
				Debug.LogWarning($"{nameof(FireballSpell)} on {name}: missing EnemyProvider.", this);
				return false;
			}

			if (context.CastOrigin == null)
			{
				Debug.LogWarning($"{nameof(FireballSpell)} on {name}: missing CastOrigin.", this);
				return false;
			}

			if (!context.EnemyProvider.TryGetRandomVisibleEnemy(out EnemyController enemy))
			{
				return false;
			}

			if (enemy == null || !enemy.IsAlive || !enemy.IsVisible)
			{
				return false;
			}

			selectedTargetPosition = enemy.Transform.position;
			hasSelectedTarget = true;

			return true;
		}

		protected override bool ExecuteCast(SpellCastContext context)
		{
			if (!hasSelectedTarget)
			{
				return false;
			}

			if (SpellData.ProjectilePrefab == null)
			{
				Debug.LogWarning($"{nameof(FireballSpell)} on {name}: missing projectile" +
				                 $" prefab in SpellDataSO.", this);
				return false;
			}

			Vector3 direction = selectedTargetPosition - context.CastOrigin.position;

			if (direction.sqrMagnitude <= 0.001f)
			{
				return false;
			}

			BaseProjectile projectile = Instantiate(SpellData.ProjectilePrefab, context.CastOrigin.position,
				Quaternion.LookRotation(direction.normalized, Vector3.up));

			if (projectile == null)
			{
				return false;
			}

			ProjectileContext projectileContext = new(context.CastOrigin.position, direction.normalized,
				selectedTargetPosition, null, SpellData.Damage, SpellData.ProjectileSpeed,
				SpellData.ExplosionRadius, SpellData.ExplosionVfxPrefab, SpellData.ExplosionVfxLifetime,
				SpellData.ArcHeight, context.EnemyProvider, SpellData.PostDamageEffects);

			projectile.Initialize(projectileContext);
			return true;
		}
	}
}