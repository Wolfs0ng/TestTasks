using UnityEngine;

namespace MagicTower.Battle.Abstract
{
	public interface IDamageable
	{
		bool IsAlive { get; }
		Transform Transform { get; }

		void TakeDamage(int amount);
	}
}