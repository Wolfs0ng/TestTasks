using MagicTower.Battle.Abstract;
using MagicTower.Battle.Effects;
using UnityEngine;

namespace MagicTower.Battle.Data
{
	[CreateAssetMenu(fileName = "BurningEffect", menuName = "Magic Tower/Battle/Effects/Burning Effect")]
	public class BurningEffectSO : BaseDamageEffectSO
	{
		[SerializeField] private float duration = 3f;
		[SerializeField] private float tickInterval = 1f;
		[SerializeField] private int damagePerTick = 1;

		public override void Apply(IDamageable target)
		{
			if (target == null || !target.IsAlive || target.Transform == null)
			{
				return;
			}

			BurningStatus burningStatus = target.Transform.GetComponent<BurningStatus>();

			if (burningStatus != null)
			{
				burningStatus.Refresh(duration, tickInterval, damagePerTick);
				return;
			}

			burningStatus = target.Transform.gameObject.AddComponent<BurningStatus>();
			burningStatus.Initialize(target, duration, tickInterval, damagePerTick);
		}
	}
}