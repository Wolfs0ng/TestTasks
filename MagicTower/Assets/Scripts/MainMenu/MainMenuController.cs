using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MagicTower.MainMenu
{
	public class MainMenuController : MonoBehaviour
	{
		[SerializeField] private Button startButton;
		[SerializeField] private string battleScene = "Battle";
		
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
			startButton.onClick.AddListener(StartGame);
		}

		private void StartGame()
		{
			SceneManager.LoadScene(battleScene);
		}

		private void EventsUnsubscribe()
		{
			startButton.onClick.RemoveAllListeners();
		}
	}
}