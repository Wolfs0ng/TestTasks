// /* Carnage Club – Proprietary Software
//  *
//  *
//  * Copyright (c) 2025 Dmytro Tarasenko
//  * Author identity:
//  * - GitHub: https://github.com/Wolfs0ng
//  * - Contact: nemesidik@gmail.com
//  *
//  * Viewing permitted. Reuse or redistribution prohibited.
//  * See LICENSE file for full terms.
//  */

using MagicTower.Battle.Abstract;
using UnityEngine;

namespace MagicTower.Battle.Controllers
{
	public class EnemyAttack : MonoBehaviour
	{
		private IDamageable target;
		private EnemyMovement enemyMovement;
		private GameController gameController;
		private HealthController healthController;
		private int attackDamage;
		private float attackInterval;
		private float attackTimer;
		private bool isInitialized;

		private void Update()
		{
			if (!CanAttack())
			{
				return;
			}

			attackTimer -= Time.deltaTime;

			if (attackTimer > 0f)
			{
				return;
			}

			Attack();
			attackTimer = attackInterval;
		}

		public void Initialize(IDamageable target, EnemyMovement enemyMovement, GameController gameController,
			HealthController healthController, int attackDamage, float attackInterval)
		{
			this.target = target;
			this.enemyMovement = enemyMovement;
			this.gameController = gameController;
			this.healthController = healthController;
			this.attackDamage = attackDamage;
			this.attackInterval = attackInterval;

			attackTimer = 0f;
			isInitialized = true;

			if (this.target == null)
			{
				Debug.LogWarning($"{nameof(EnemyAttack)} on {name} has no target assigned.", this);
			}

			if (this.enemyMovement == null)
			{
				Debug.LogWarning($"{nameof(EnemyAttack)} on {name} has no EnemyMovement assigned.", this);
			}

			if (this.healthController == null)
			{
				Debug.LogWarning($"{nameof(EnemyAttack)} on {name} has no HealthController assigned.", this);
			}
		}

		private bool CanAttack()
		{
			if (!isInitialized)
			{
				return false;
			}

			if (target == null)
			{
				return false;
			}

			if (!target.IsAlive)
			{
				return false;
			}

			if (healthController == null || !healthController.IsAlive)
			{
				return false;
			}

			if (enemyMovement == null || !enemyMovement.IsInRange)
			{
				return false;
			}

			if (gameController != null && !gameController.IsPlaying)
			{
				return false;
			}

			if (attackDamage <= 0)
			{
				return false;
			}

			return true;
		}

		private void Attack()
		{
			target.TakeDamage(attackDamage);
		}
	}
}