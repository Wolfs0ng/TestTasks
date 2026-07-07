using System;
using UnityEngine;

namespace MagicTower.Battle.Controllers
{
	public class HealthController : MonoBehaviour
	{
		public event Action<int, int> OnHealthChanged;
		public event Action<int> OnDamageTaken;
		public event Action OnDeath;

		private int maxHealth;
		private int currentHealth;
		
		public bool IsAlive => currentHealth > 0;

		public void Initialize(int maxHealth)
		{
			this.maxHealth = Mathf.Max(1, maxHealth);
			currentHealth = this.maxHealth;

			OnHealthChanged?.Invoke(currentHealth, this.maxHealth);
		}

		public void TakeDamage(int amount)
		{
			if (!IsAlive)
			{
				return;
			}

			if (amount <= 0)
			{
				return;
			}

			currentHealth = Mathf.Max(0, currentHealth - amount);

			OnHealthChanged?.Invoke(currentHealth, maxHealth);
			OnDamageTaken?.Invoke(amount);

			if (!IsAlive)
			{
				OnDeath?.Invoke();
			}
		}
	}
}