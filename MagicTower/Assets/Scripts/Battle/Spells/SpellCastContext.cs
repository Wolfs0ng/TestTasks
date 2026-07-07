using MagicTower.Battle.Controllers;
using UnityEngine;

namespace MagicTower.Battle.Spells
{
	public readonly struct SpellCastContext
	{
		public readonly Transform CastOrigin;
		public readonly EnemyManager EnemyProvider;
		public readonly GameController GameController;

		public SpellCastContext(Transform castOrigin, EnemyManager enemyProvider, GameController gameController)
		{
			CastOrigin = castOrigin;
			EnemyProvider = enemyProvider;
			GameController = gameController;
		}

		public bool IsValid => CastOrigin != null && EnemyProvider != null;
	}
}