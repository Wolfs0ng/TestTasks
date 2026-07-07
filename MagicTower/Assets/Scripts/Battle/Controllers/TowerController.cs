using System;
using MagicTower.Battle.Abstract;
using UnityEngine;

namespace MagicTower.Battle.Controllers
{
	[RequireComponent(typeof(HealthController))]
	public class TowerController : MonoBehaviour, IDamageable
	{
		public event Action OnTowerDestroyed;
		public event Action<int> OnDamageTaken;

		[SerializeField] private HealthController healthController;
		[SerializeField] private int maxHealth = 100;

		public bool IsAlive => healthController != null && healthController.IsAlive;
		public Transform Transform => transform;

		private void Awake()
		{
			if (healthController == null)
			{
				healthController = GetComponent<HealthController>();
			}
		}

		private void OnDestroy()
		{
			EventsUnsubscribe();
		}

		public void Initialize()
		{
			if (healthController == null)
			{
				Debug.LogError($"{nameof(TowerController)} on {name}: missing HealthController.", this);
				return;
			}

			EventsUnsubscribe();

			healthController.Initialize(maxHealth);

			EventsSubscribe();
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
			OnTowerDestroyed?.Invoke();
		}

		private void HandleDamageTaken(int amount)
		{
			OnDamageTaken?.Invoke(amount);
		}
	}
}