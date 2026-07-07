using MagicTower.Battle.Controllers;
using MagicTower.Battle.Enums;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MagicTower.Battle.UI
{
	public class GameEndView  : MonoBehaviour
	{
		[SerializeField] private GameObject root;
		[SerializeField] private Button startButton;
		[SerializeField] private string menuScene = "Menu";

		[SerializeField] private GameController gameController;
		
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
			startButton.onClick.AddListener(ExitToMenu);
			gameController.OnGameStateChanged += OnGameStateChanged;
		}

		private void OnGameStateChanged(GameState newGameState)
		{
			if (newGameState == GameState.GameOver)
			{
				root.SetActive(true);
			}
		}

		private void ExitToMenu()
		{
			SceneManager.LoadScene(menuScene);
		}

		private void EventsUnsubscribe()
		{
			startButton.onClick.RemoveAllListeners();
			gameController.OnGameStateChanged -= OnGameStateChanged;
		}
	}
}