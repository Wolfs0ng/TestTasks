using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardsChatVfx.AceOfShadows
{
    public sealed class AceOfShadowsController : MonoBehaviour
    {
        [Header("Stacks")]
        [SerializeField] private CardStackView leftStack;
        [SerializeField] private CardStackView rightStack;
        [SerializeField] private RectTransform transferLayer;

        [Header("Cards")]
        [SerializeField] private CardView cardPrefab;
        [SerializeField, Min(1)] private int requiredCardCount = 144;

        [Header("Controls")]
        [SerializeField] private Button moveLeftButton;
        [SerializeField] private Button stopButton;
        [SerializeField] private Button moveRightButton;

        [Header("Status")]
        [SerializeField] private GameObject statusMessage;
        [SerializeField] private TextMeshProUGUI statusMessageText;

        [Header("Animation")]
        [Tooltip("Interval between the starts of consecutive card transfers.")]
        [SerializeField, Min(0.01f)] private float transferInterval = 1f;
        [SerializeField, Min(0.01f)] private float movementDuration = 0.65f;
        [SerializeField, Min(0f)] private float arcHeight = 120f;
        [SerializeField] private AnimationCurve movementCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        private Coroutine transferRoutine;
        private bool stopRequested;

        private bool IsTransferring => transferRoutine != null;

        private void Awake()
        {
            EventsSubscribe();
            InitializeScene();
        }

        private void OnDestroy()
        {
            EventsUnsubscribe();
        }

        private void EventsSubscribe()
        {
            moveLeftButton.onClick.AddListener(HandleMoveLeftClicked);
            stopButton.onClick.AddListener(HandleStopClicked);
            moveRightButton.onClick.AddListener(HandleMoveRightClicked);
        }

        private void EventsUnsubscribe()
        {
            moveLeftButton.onClick.RemoveListener(HandleMoveLeftClicked);
            stopButton.onClick.RemoveListener(HandleStopClicked);
            moveRightButton.onClick.RemoveListener(HandleMoveRightClicked);
        }

        private void InitializeScene()
        {
            leftStack.Initialize();
            rightStack.Initialize();

            SpawnMissingCards();

            statusMessage.SetActive(false);

            RefreshControls();
        }

        private void SpawnMissingCards()
        {
            int existingCardCount = leftStack.Count + rightStack.Count;
            int missingCardCount = requiredCardCount - existingCardCount;

            if (missingCardCount <= 0)
            {
                return;
            }

            for (int i = 0; i < missingCardCount; i++)
            {
                CardView card = Instantiate(cardPrefab);
                leftStack.AddCard(card);
            }
        }

        private void HandleMoveLeftClicked()
        {
            StartTransfer(sourceStack: rightStack, destinationStack: leftStack);
        }

        private void HandleMoveRightClicked()
        {
            StartTransfer(sourceStack: leftStack, destinationStack: rightStack);
        }

        private void HandleStopClicked()
        {
            if (!IsTransferring)
            {
                return;
            }

            stopRequested = true;
            stopButton.interactable = false;

            ShowStatus("Stopping...");
        }

        private void StartTransfer(CardStackView sourceStack, CardStackView destinationStack)
        {
            if (IsTransferring || sourceStack.Count == 0)
            {
                return;
            }

            stopRequested = false;
            statusMessage.SetActive(false);

            transferRoutine = StartCoroutine(TransferCardsRoutine(sourceStack, destinationStack));

            RefreshControls();
        }

        private IEnumerator TransferCardsRoutine(CardStackView sourceStack, CardStackView destinationStack)
        {
            while (sourceStack.Count > 0 && !stopRequested)
            {
                float transferStartedAt = Time.unscaledTime;

                yield return TransferTopCardRoutine(sourceStack, destinationStack);

                if (sourceStack.Count == 0 || stopRequested)
                {
                    break;
                }

                float nextTransferTime = transferStartedAt + transferInterval;

                while (!stopRequested && Time.unscaledTime < nextTransferTime)
                {
                    yield return null;
                }
            }

            bool completed = sourceStack.Count == 0;

            transferRoutine = null;
            stopRequested = false;

            if (completed)
            {
                ShowCompletionMessage(destinationStack);
            }
            else
            {
                ShowStatus("Transfer stopped");
            }

            RefreshControls();
        }

        private IEnumerator TransferTopCardRoutine(CardStackView sourceStack, CardStackView destinationStack)
        {
            CardView card = sourceStack.RemoveTopCard();

            if (card == null)
            {
                yield break;
            }

            Vector3 destinationWorldPosition = destinationStack.GetNextCardWorldPosition();

            yield return card.MoveTo(transferLayer, destinationWorldPosition, movementDuration,
	            arcHeight, movementCurve);

            destinationStack.AddCard(card);
        }

        private void ShowCompletionMessage(CardStackView destinationStack)
        {
            string destinationName = ReferenceEquals(destinationStack, leftStack) ? "left" : "right";
            ShowStatus($"All cards moved to the {destinationName} stack!");
        }

        private void ShowStatus(string message)
        {
            statusMessageText.SetText(message);
            statusMessage.SetActive(true);
        }

        private void RefreshControls()
        {
            moveLeftButton.interactable = !IsTransferring && rightStack.Count > 0;
            moveRightButton.interactable = !IsTransferring && leftStack.Count > 0;
            stopButton.interactable = IsTransferring && !stopRequested;
        }
    }
}