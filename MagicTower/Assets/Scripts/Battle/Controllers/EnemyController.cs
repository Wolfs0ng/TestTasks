using System;
using MagicTower.Battle.Abstract;
using MagicTower.Battle.Data;
using UnityEngine;

namespace MagicTower.Battle.Controllers
{
	[RequireComponent(typeof(HealthController))]
	[RequireComponent(typeof(EnemyMovement))]
	[RequireComponent(typeof(EnemyAttack))]
	public class EnemyController : MonoBehaviour, IDamageable
	{
		public event Action<int> OnDamageTaken;

		[SerializeField] private EnemyDataSO enemyData;
		[SerializeField] private HealthController healthController;
		[SerializeField] private EnemyMovement enemyMovement;
		[SerializeField] private EnemyAttack enemyAttack;
		[SerializeField] private TowerController target;
		[SerializeField] private MonoBehaviour targetDamageableSource;
		[SerializeField] private GameController gameController;
		[SerializeField] private EnemyManager ownerManager;

		private Renderer[] renderers;
		private bool isRegistered;

		public bool IsAlive => healthController != null && healthController.IsAlive;
		public Transform Transform => transform;
		public bool IsVisible
		{
			get
			{
				if (renderers == null || renderers.Length == 0)
				{
					return false;
				}

				for (int i = 0; i < renderers.Length; i++)
				{
					if (renderers[i] != null && renderers[i].isVisible)
					{
						return true;
					}
				}

				return false;
			}
		}

		private void Awake()
		{
			if (healthController == null)
			{
				healthController = GetComponent<HealthController>();
			}

			if (enemyMovement == null)
			{
				enemyMovement = GetComponent<EnemyMovement>();
			}

			if (enemyAttack == null)
			{
				enemyAttack = GetComponent<EnemyAttack>();
			}

			renderers = GetComponentsInChildren<Renderer>();
		}

		private void OnDestroy()
		{
			UnregisterFromManager();
			EventsUnsubscribe();
		}

		public void Initialize(EnemyDataSO enemyData, TowerController target, GameController gameController,
			EnemyManager ownerManager)
		{
			this.enemyData = enemyData;
			this.target = target;
			this.gameController = gameController;
			this.ownerManager = ownerManager;

			if (this.enemyData == null)
			{
				Debug.LogError($"{nameof(EnemyController)} on {name} has no EnemyData assigned.", this);
				return;
			}

			if (healthController == null)
			{
				Debug.LogError($"{nameof(EnemyController)} on {name} has no HealthController assigned.", this);
				return;
			}

			if (enemyMovement == null)
			{
				Debug.LogError($"{nameof(EnemyController)} on {name} has no EnemyMovement assigned.", this);
				return;
			}

			if (enemyAttack == null)
			{
				Debug.LogError($"{nameof(EnemyController)} on {name} has no EnemyAttack assigned.", this);
				return;
			}

			if (this.target == null)
			{
				Debug.LogError($"{nameof(EnemyController)} on {name} has no target Transform assigned.", this);
				return;
			}

			EventsUnsubscribe();

			healthController.Initialize(enemyData.MaxHealth);
			enemyMovement.Initialize(target.Transform, enemyData.MoveSpeed, enemyData.AttackRange,
				gameController);
			enemyAttack.Initialize(target, enemyMovement, gameController, healthController, enemyData.AttackDamage,
				enemyData.AttackInterval);

			EventsSubscribe();
			RegisterInManager();
		}

		public void TakeDamage(int amount)
		{
			if (healthController == null)
			{
				return;
			}

			healthController.TakeDamage(amount);
		}

		private void EventsSubscribe()
		{
			if (healthController == null)
			{
				return;
			}

			healthController.OnDeath += HandleDeath;
			healthController.OnDamageTaken += HandleDamageTaken;
		}

		private void EventsUnsubscribe()
		{
			if (healthController == null)
			{
				return;
			}

			healthController.OnDeath -= HandleDeath;
			healthController.OnDamageTaken -= HandleDamageTaken;
		}

		private void HandleDeath()
		{
			UnregisterFromManager();
			Destroy(gameObject);
		}

		private void HandleDamageTaken(int amount)
		{
			OnDamageTaken?.Invoke(amount);
		}

		private void RegisterInManager()
		{
			if (ownerManager == null || isRegistered)
			{
				return;
			}

			ownerManager.RegisterEnemy(this);
			isRegistered = true;
		}

		private void UnregisterFromManager()
		{
			if (ownerManager == null || !isRegistered)
			{
				return;
			}

			ownerManager.UnregisterEnemy(this);
			isRegistered = false;
		}
	}
}