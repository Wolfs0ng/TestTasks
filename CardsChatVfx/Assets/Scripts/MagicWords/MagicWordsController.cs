using System;
using System.Collections;
using System.Collections.Generic;
using CardsChatVfx.MainMenu;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace CardsChatVfx.MagicWords
{
    public sealed class MagicWordsController : MonoBehaviour
    {
        private const string DefaultEndpoint = "https://private-624120-softgamesassignment.apiary-mock.com/v3/magicwords";

        [Header("Controls")]
        [SerializeField] private Button backToMenuButton;
        
        [Header("API")]
        [SerializeField] private string endpoint = DefaultEndpoint;

        [Header("Dialogue")]
        [SerializeField] private RectTransform dialogueContent;
        [SerializeField] private DialogueElementView dialogueElementPrefab;
        [SerializeField] private ScrollRect dialogueScrollRect;

        [Header("Dialogue Pool")]
        [Tooltip("Optional pre-created dialogue views. They will be used before new views are instantiated.")]
        [SerializeField] private List<DialogueElementView> initialDialogueElements = new();

        [Header("Avatar")]
        [SerializeField] private Sprite fallbackAvatar;

        [Header("State")]
        [SerializeField] private GameObject loadingPanel;
        [SerializeField] private GameObject errorPanel;
        [SerializeField] private TextMeshProUGUI errorText;
        [SerializeField] private Button retryButton;
        
        private readonly SceneLoader sceneLoader = new();
        
        private readonly Queue<DialogueElementView> availableElements = new();
        private readonly List<DialogueElementView> allElements = new();
        private readonly List<DialogueElementView> activeElements = new();

        private AvatarImageLoader avatarImageLoader;
        private Coroutine dialogueLoadingCoroutine;

        private int contentGeneration;

        private void Awake()
        {
            avatarImageLoader = new AvatarImageLoader();

            InitializeDialoguePool();

            if (retryButton != null)
            {
                retryButton.onClick.AddListener(Reload);
                retryButton.gameObject.SetActive(false);
            }

            if (backToMenuButton != null)
            {
	            backToMenuButton.onClick.AddListener(HandleBackToMenuClicked);
            }
        }

        private void Start()
        {
            Reload();
        }

        private void OnDestroy()
        {
            if (retryButton != null)
            {
                retryButton.onClick.RemoveListener(Reload);
            }

            if (backToMenuButton != null)
            {
	            backToMenuButton.onClick.RemoveListener(HandleBackToMenuClicked);
            }

            avatarImageLoader?.Dispose();
            avatarImageLoader = null;
        }

        public void Reload()
        {
            contentGeneration++;

            if (dialogueLoadingCoroutine != null)
            {
                StopCoroutine(dialogueLoadingCoroutine);
                dialogueLoadingCoroutine = null;
            }

            ReleaseActiveDialogueElements();

            dialogueLoadingCoroutine = StartCoroutine(LoadDialogueRoutine(contentGeneration));
        }

        private IEnumerator LoadDialogueRoutine(int generation)
        {
            ShowLoadingState();

            using UnityWebRequest request = UnityWebRequest.Get(endpoint);

            request.timeout = 15;

            yield return request.SendWebRequest();

            dialogueLoadingCoroutine = null;

            if (generation != contentGeneration)
            {
                yield break;
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                ShowErrorState($"Unable to load dialogue.\n{request.error}");
                yield break;
            }

            MagicWordsResponseDto response;

            try
            {
                response = JsonUtility.FromJson<MagicWordsResponseDto>(request.downloadHandler.text);
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);

                ShowErrorState("The dialogue data could not be read.");
                yield break;
            }

            if (response == null)
            {
                ShowErrorState("The server returned no dialogue data.");
                yield break;
            }

            PopulateDialogue(response, generation);
        }

        private void PopulateDialogue(MagicWordsResponseDto response, int generation)
        {
            HideStatePanels();

            IReadOnlyList<DialogueEntryDto> dialogueEntries = response.Dialogue;

            if (dialogueEntries == null || dialogueEntries.Count == 0)
            {
                return;
            }

            Dictionary<string, List<AvatarEntryDto>> avatarLookup = BuildAvatarLookup(response.Avatars);

            for (int i = 0; i < dialogueEntries.Count; i++)
            {
                DialogueEntryDto entry = dialogueEntries[i];

                if (entry == null)
                {
                    continue;
                }

                CreateDialogueElement(entry, avatarLookup, generation);
            }

            Canvas.ForceUpdateCanvases();

            if (dialogueScrollRect != null)
            {
                dialogueScrollRect.verticalNormalizedPosition = 1f;
            }
        }

        private void CreateDialogueElement(DialogueEntryDto entry,
	        IReadOnlyDictionary<string, List<AvatarEntryDto>> avatarLookup, int generation)
        {
            string characterName = entry.Name?.Trim();

            string formattedText = EmojiTextFormatter.Format(entry.Text);

            List<AvatarEntryDto> avatarCandidates = FindAvatarCandidates(characterName, avatarLookup);
            AvatarEntryDto initialAvatarData = GetFirstUsableAvatarData(avatarCandidates);
            bool isInitiallyRight = IsRightPosition(initialAvatarData?.Position);
            DialogueElementView element = AcquireDialogueElement();

            element.Initialize(characterName, formattedText, isInitiallyRight, fallbackAvatar);

            if (avatarCandidates == null || avatarCandidates.Count == 0)
            {
                return;
            }

            TryLoadAvatarCandidate(element, avatarCandidates, 0, generation);
        }

        private void TryLoadAvatarCandidate(DialogueElementView element, IReadOnlyList<AvatarEntryDto> candidates,
	        int candidateIndex, int generation)
        {
	        if (generation != contentGeneration || element == null)
	        {
		        return;
	        }

	        if (candidates == null || candidateIndex >= candidates.Count)
	        {
		        return;
	        }

	        AvatarEntryDto candidate = candidates[candidateIndex];

	        if (candidate == null || string.IsNullOrWhiteSpace(candidate.Url))
	        {
		        TryLoadAvatarCandidate(element, candidates, candidateIndex + 1, generation);
		        return;
	        }

	        StartCoroutine(avatarImageLoader.Load(candidate.Url,
		        sprite =>
		        {
			        if (generation != contentGeneration || element == null ||
			            !element.gameObject.activeInHierarchy)
			        {
				        return;
			        }

			        if (sprite != null)
			        {
				        element.SetAvatar(sprite);
				        element.SetSide(IsRightPosition(candidate.Position));

				        return;
			        }

			        TryLoadAvatarCandidate(element, candidates, candidateIndex + 1, generation);
		        }));
        }

        private void InitializeDialoguePool()
        {
            availableElements.Clear();
            allElements.Clear();
            activeElements.Clear();

            HashSet<DialogueElementView> uniqueElements = new();

            for (int i = 0; i < initialDialogueElements.Count; i++)
            {
                DialogueElementView element = initialDialogueElements[i];

                if (element == null || !uniqueElements.Add(element))
                {
                    continue;
                }

                element.transform.SetParent(dialogueContent, false);

                element.ResetView();
                element.gameObject.SetActive(false);

                allElements.Add(element);
                availableElements.Enqueue(element);
            }
        }
        
        private async void HandleBackToMenuClicked()
        {
            if (sceneLoader.IsLoading)
            {
                return;
            }

            retryButton.interactable = false;
            backToMenuButton.interactable = false;
            
            await sceneLoader.LoadAsync(AppScene.MainMenu);
        }

        private DialogueElementView AcquireDialogueElement()
        {
            DialogueElementView element = null;

            while (availableElements.Count > 0 && element == null)
            {
                element = availableElements.Dequeue();
            }

            if (element == null)
            {
                element = Instantiate(dialogueElementPrefab, dialogueContent);
                allElements.Add(element);
            }
            else
            {
                element.transform.SetParent(dialogueContent, false);
            }

            element.gameObject.SetActive(true);
            activeElements.Add(element);

            return element;
        }

        private void ReleaseActiveDialogueElements()
        {
            for (int i = 0; i < activeElements.Count; i++)
            {
                DialogueElementView element = activeElements[i];

                if (element == null)
                {
                    continue;
                }

                element.ResetView();
                element.gameObject.SetActive(false);

                availableElements.Enqueue(element);
            }

            activeElements.Clear();
        }

        private static Dictionary<string, List<AvatarEntryDto>> BuildAvatarLookup(IReadOnlyList<AvatarEntryDto> avatars)
        {
            Dictionary<string, List<AvatarEntryDto>> lookup = new(StringComparer.OrdinalIgnoreCase);

            if (avatars == null)
            {
                return lookup;
            }

            for (int i = 0; i < avatars.Count; i++)
            {
                AvatarEntryDto avatar = avatars[i];

                if (avatar == null || string.IsNullOrWhiteSpace(avatar.Name))
                {
                    continue;
                }

                string characterName = avatar.Name.Trim();

                if (!lookup.TryGetValue(characterName, out List<AvatarEntryDto> characterAvatars))
                {
                    characterAvatars = new List<AvatarEntryDto>();

                    lookup.Add(characterName, characterAvatars);
                }

                characterAvatars.Add(avatar);
            }

            return lookup;
        }

        private static List<AvatarEntryDto> FindAvatarCandidates(string characterName,
            IReadOnlyDictionary<string, List<AvatarEntryDto>> avatarLookup)
        {
            if (string.IsNullOrWhiteSpace(characterName))
            {
                return null;
            }

            return avatarLookup.GetValueOrDefault(characterName);
        }

        private static AvatarEntryDto GetFirstUsableAvatarData(IReadOnlyList<AvatarEntryDto> candidates)
        {
            if (candidates == null)
            {
                return null;
            }

            for (int i = 0; i < candidates.Count; i++)
            {
                AvatarEntryDto candidate = candidates[i];

                if (candidate == null)
                {
                    continue;
                }

                if (!string.IsNullOrWhiteSpace(candidate.Url))
                {
                    return candidate;
                }
            }

            return null;
        }

        private static bool IsRightPosition(string position)
        {
            return string.Equals(position, "right", StringComparison.OrdinalIgnoreCase);
        }

        private void ShowLoadingState()
        {
            if (loadingPanel != null)
            {
                loadingPanel.SetActive(true);
            }

            if (errorPanel != null)
            {
                errorPanel.SetActive(false);
            }

            if (retryButton != null)
            {
                retryButton.gameObject.SetActive(false);
            }
        }

        private void ShowErrorState(string message)
        {
            if (loadingPanel != null)
            {
                loadingPanel.SetActive(false);
            }

            if (errorPanel != null)
            {
                errorPanel.SetActive(true);
            }

            if (retryButton != null)
            {
                retryButton.gameObject.SetActive(true);
            }

            if (errorText != null)
            {
                errorText.SetText(message);
            }
        }

        private void HideStatePanels()
        {
            if (loadingPanel != null)
            {
                loadingPanel.SetActive(false);
            }

            if (errorPanel != null)
            {
                errorPanel.SetActive(false);
            }

            if (retryButton != null)
            {
                retryButton.gameObject.SetActive(false);
            }
        }
    }
}