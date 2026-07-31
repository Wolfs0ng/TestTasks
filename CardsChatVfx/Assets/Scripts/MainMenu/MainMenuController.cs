using UnityEngine;
using UnityEngine.UI;

namespace CardsChatVfx.MainMenu
{
    public sealed class MainMenuController : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button aceOfShadowsButton;
        [SerializeField] private Button magicWordsButton;
        [SerializeField] private Button phoenixFlameButton;

        private readonly SceneLoader sceneLoader = new();

        private void Awake()
        {
            EventsSubscribe();
        }

        private void EventsSubscribe()
        {
	        aceOfShadowsButton.onClick.AddListener(OpenAceOfShadows);
	        magicWordsButton.onClick.AddListener(OpenMagicWords);
	        phoenixFlameButton.onClick.AddListener(OpenPhoenixFlame);

	        sceneLoader.LoadStarted += HandleLoadStarted;
        }

        private void OnDestroy()
        {
            EventsUnsubscribe();
        }
        
        private void EventsUnsubscribe()
        {
	        aceOfShadowsButton.onClick.RemoveListener(OpenAceOfShadows);
	        magicWordsButton.onClick.RemoveListener(OpenMagicWords);
	        phoenixFlameButton.onClick.RemoveListener(OpenPhoenixFlame);

	        sceneLoader.LoadStarted -= HandleLoadStarted;
        }

        private async void OpenAceOfShadows()
        {
            await sceneLoader.LoadAsync(AppScene.AceOfShadows);
        }

        private async void OpenMagicWords()
        {
            await sceneLoader.LoadAsync(AppScene.MagicWords);
        }

        private async void OpenPhoenixFlame()
        {
            await sceneLoader.LoadAsync(AppScene.PhoenixFlame);
        }

        private void HandleLoadStarted(AppScene scene)
        {
            SetButtonsInteractable(false);
        }

        private void SetButtonsInteractable(bool isInteractable)
        {
            aceOfShadowsButton.interactable = isInteractable;
            magicWordsButton.interactable = isInteractable;
            phoenixFlameButton.interactable = isInteractable;
        }
    }
}