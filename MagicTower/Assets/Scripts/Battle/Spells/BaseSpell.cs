using MagicTower.Battle.Data;
using UnityEngine;

namespace MagicTower.Battle.Spells
{
	public abstract class BaseSpell : MonoBehaviour
	{
		[SerializeField] private SpellDataSO spellData;

		private float cooldownTimer;

		protected SpellDataSO SpellData => spellData;

		public bool IsReady => cooldownTimer <= 0f;

		public void TickCooldown(float deltaTime)
		{
			if (cooldownTimer <= 0f)
			{
				return;
			}

			cooldownTimer -= deltaTime;
		}

		public bool TryCast(SpellCastContext context)
		{
			if (!IsReady)
			{
				return false;
			}

			if (spellData == null)
			{
				Debug.LogWarning($"{nameof(BaseSpell)} on {name}: missing SpellDataSO.", this);
				return false;
			}

			if (!context.IsValid)
			{
				Debug.LogWarning($"{nameof(BaseSpell)} on {name}: invalid spell cast context.", this);
				return false;
			}

			if (context.GameController != null && !context.GameController.IsPlaying)
			{
				return false;
			}

			if (!TrySelectTargets(context))
			{
				return false;
			}

			if (!ExecuteCast(context))
			{
				return false;
			}

			StartCooldown();
			return true;
		}

		protected abstract bool TrySelectTargets(SpellCastContext context);
		protected abstract bool ExecuteCast(SpellCastContext context);

		private void StartCooldown()
		{
			cooldownTimer = Mathf.Max(0.01f, spellData.Cooldown);
		}
	}
}