using System.Collections.Generic;
using MagicTower.Battle.Controllers;
using MagicTower.Battle.Projectiles;
using UnityEngine;

namespace MagicTower.Battle.Spells
{
	public class BarrageSpell : BaseSpell
	{
		private readonly List<EnemyController> selectedTargets = new();

		protected override bool TrySelectTargets(SpellCastContext context)
		{
			selectedTargets.Clear();

			if (context.EnemyProvider == null)
			{
				Debug.LogWarning($"{nameof(BarrageSpell)} on {name}: missing EnemyProvider.", this);
				return false;
			}

			if (context.CastOrigin == null)
			{
				Debug.LogWarning($"{nameof(BarrageSpell)} on {name}: missing CastOrigin.", this);
				return false;
			}

			List<EnemyController> visibleEnemies = context.EnemyProvider.GetVisibleEnemies();

			for (int i = 0; i < visibleEnemies.Count; i++)
			{
				EnemyController enemy = visibleEnemies[i];

				if (enemy == null || !enemy.IsAlive)
				{
					continue;
				}

				selectedTargets.Add(enemy);
			}

			return selectedTargets.Count > 0;
		}

		protected override bool ExecuteCast(SpellCastContext context)
		{
			if (SpellData.ProjectilePrefab == null)
			{
				Debug.LogWarning($"{nameof(BarrageSpell)} on {name}: missing projectile prefab in" +
				                 $" SpellDataSO.", this);
				return false;
			}

			int spawnedProjectiles = 0;

			for (int i = 0; i < selectedTargets.Count; i++)
			{
				EnemyController target = selectedTargets[i];

				if (target == null || !target.IsAlive)
				{
					continue;
				}

				Vector3 direction = target.Transform.position - context.CastOrigin.position;

				if (direction.sqrMagnitude <= 0.001f)
				{
					continue;
				}

				BaseProjectile projectile = Instantiate(SpellData.ProjectilePrefab, context.CastOrigin.position,
					Quaternion.LookRotation(direction.normalized, Vector3.up));

				if (projectile == null)
				{
					continue;
				}

				ProjectileContext projectileContext = new(context.CastOrigin.position, direction.normalized,
					target.Transform.position, target, SpellData.Damage, SpellData.ProjectileSpeed,
					SpellData.ExplosionRadius, null, 0f, SpellData.ArcHeight,
					context.EnemyProvider, SpellData.PostDamageEffects);

				projectile.Initialize(projectileContext);
				spawnedProjectiles++;
			}

			return spawnedProjectiles > 0;
		}
	}
}