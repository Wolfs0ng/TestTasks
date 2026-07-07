using MagicTower.Battle.Abstract;
using UnityEngine;

namespace MagicTower.Battle.Data
{
	public abstract class BaseDamageEffectSO : ScriptableObject
	{
		public abstract void Apply(IDamageable target);
	}
}