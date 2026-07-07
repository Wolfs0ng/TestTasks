using MagicTower.Battle.Abstract;
using UnityEngine;

namespace MagicTower.Battle.Effects
{
	public class BurningStatus : MonoBehaviour
	{
		private IDamageable target;
		private float remainingDuration;
		private float tickInterval;
		private float tickTimer;
		private int damagePerTick;

		public void Initialize(IDamageable target, float duration, float tickInterval, int damagePerTick)
		{
			this.target = target;

			Refresh(duration, tickInterval, damagePerTick);
		}

		public void Refresh(float duration, float tickInterval, int damagePerTick)
		{
			this.tickInterval = Mathf.Max(0.01f, tickInterval);
			this.damagePerTick = damagePerTick;

			remainingDuration = Mathf.Max(0f, duration);
			tickTimer = this.tickInterval;

			if (!IsStatusValid())
			{
				Destroy(this);
			}
		}

		private void Update()
		{
			if (!IsStatusValid())
			{
				Destroy(this);
				return;
			}

			remainingDuration -= Time.deltaTime;
			tickTimer -= Time.deltaTime;

			if (tickTimer <= 0f)
			{
				target.TakeDamage(damagePerTick);
				tickTimer = tickInterval;
			}

			if (remainingDuration <= 0f)
			{
				Destroy(this);
			}
		}

		private bool IsStatusValid()
		{
			if (target == null || !target.IsAlive)
			{
				return false;
			}

			if (remainingDuration <= 0f)
			{
				return false;
			}

			if (damagePerTick <= 0)
			{
				return false;
			}

			return true;
		}
	}
}