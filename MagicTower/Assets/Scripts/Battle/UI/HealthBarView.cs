using MagicTower.Battle.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace MagicTower.Battle.UI
{
	public class HealthBarView : MonoBehaviour
	{
		[SerializeField] private HealthController targetHealth;
		[SerializeField] private Image fillImage;
		
		private void Awake()
		{
			EventsSubscribe();
		}
		
		private void OnDestroy()
		{
			EventsUnsubscribe();
		}

		private void EventsSubscribe()
		{
			if (targetHealth != null)
			{
				targetHealth.OnHealthChanged += HandleHealthChanged;
			}
		}

		private void HandleHealthChanged(int currentHealth, int maxHealth)
		{
			UpdateFill(currentHealth, maxHealth);
		}

		private void UpdateFill(int currentHealth, int maxHealth)
		{
			if (fillImage == null)
			{
				return;
			}

			fillImage.fillAmount = Mathf.Clamp01((float)currentHealth / maxHealth);
		}

		private void EventsUnsubscribe()
		{
			if (targetHealth != null)
			{
				targetHealth.OnHealthChanged -= HandleHealthChanged;
			}
		}
	}
}