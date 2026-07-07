using System.Collections.Generic;
using MagicTower.Battle.Spells;
using UnityEngine;

namespace MagicTower.Battle.Controllers
{
	public class TowerSpellController : MonoBehaviour
	{
		[SerializeField] private GameController gameController;
		[SerializeField] private EnemyManager enemyProvider;
		[SerializeField] private Transform castOrigin;
		[SerializeField] private List<BaseSpell> spells = new();

		private SpellCastContext context;

		private void Start()
		{
			if (gameController == null)
			{
				Debug.LogWarning($"{nameof(TowerSpellController)} on {name}: missing GameController.", this);
				return;
			}

			if (enemyProvider == null)
			{
				Debug.LogWarning($"{nameof(TowerSpellController)} on {name}: missing EnemyManager.", this);
				return;
			}

			if (castOrigin == null)
			{
				Debug.LogWarning($"{nameof(TowerSpellController)} on {name}: missing CastOrigin.", this);
				return;
			}

			context = new SpellCastContext(castOrigin, enemyProvider, gameController);
		}

		private void Update()
		{
			if (gameController == null || !gameController.IsPlaying)
			{
				return;
			}

			if (!context.IsValid)
			{
				return;
			}

			for (int i = 0; i < spells.Count; i++)
			{
				BaseSpell spell = spells[i];

				if (spell == null)
				{
					continue;
				}

				spell.TickCooldown(Time.deltaTime);

				if (spell.IsReady)
				{
					spell.TryCast(context);
				}
			}
		}
	}
}