using CardsChatVfx.MainMenu;
using UnityEngine;
using UnityEngine.UI;

namespace CardsChatVfx.Common
{
	[RequireComponent(typeof(Button))]
	public sealed class BackToMenuButton : MonoBehaviour
	{
		[SerializeField] private Button backToMenuButton;

		private readonly SceneLoader sceneLoader = new();
		
		private void Awake()
		{
			backToMenuButton.onClick.AddListener(HandleBackToMenuClicked);
		}

		private void OnDestroy()
		{
			backToMenuButton.onClick.RemoveListener(HandleBackToMenuClicked);
		}

		private async void HandleBackToMenuClicked()
		{
			if (sceneLoader.IsLoading)
			{
				return;
			}

			backToMenuButton.interactable = false;

			await sceneLoader.LoadAsync(AppScene.MainMenu);
		}
	}
}