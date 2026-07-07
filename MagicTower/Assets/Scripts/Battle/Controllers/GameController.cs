using System;
using MagicTower.Battle.Enums;
using UnityEngine;

namespace MagicTower.Battle.Controllers
{
	public class GameController : MonoBehaviour
	{
		public event Action<GameState> OnGameStateChanged;

		[SerializeField] private TowerController towerController;

		public GameState CurrentState { get; private set; }
		public bool IsPlaying => CurrentState == GameState.Battle;

		private void Awake()
		{
			Initialize();
		}

		private void OnDestroy()
		{
			EventsUnsubscribe();
		}

		private void Initialize()
		{
			SetState(GameState.Battle);

			if (towerController == null)
			{
				Debug.LogWarning($"{nameof(GameController)} on {name}: missing TowerController.", this);
				return;
			}

			towerController.Initialize();
			EventsSubscribe();
		}

		private void EventsSubscribe()
		{
			if (towerController == null)
			{
				return;
			}

			towerController.OnTowerDestroyed += OnTowerDestroyed;
		}

		private void EventsUnsubscribe()
		{
			if (towerController == null)
			{
				return;
			}

			towerController.OnTowerDestroyed -= OnTowerDestroyed;
		}

		private void OnTowerDestroyed()
		{
			if (CurrentState == GameState.GameOver)
			{
				return;
			}

			SetState(GameState.GameOver);
			Debug.Log("Game Over: Tower destroyed.");
		}

		private void SetState(GameState newState)
		{
			if (CurrentState == newState)
			{
				return;
			}

			CurrentState = newState;
			OnGameStateChanged?.Invoke(CurrentState);
		}
	}
}