using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardsChatVfx.MagicWords
{
    public sealed class DialogueElementView : MonoBehaviour
    {
        [Header("Layout")]
        [SerializeField] private RectTransform avatarTransform;
        [SerializeField] private RectTransform characterNameTransform;
        [SerializeField] private RectTransform dialogueTextTransform;

        [Header("Content")]
        [SerializeField] private Image avatarImage;
        [SerializeField] private TextMeshProUGUI characterNameText;
        [SerializeField] private TextMeshProUGUI dialogueText;

        private string characterName = string.Empty;
        private bool isRight;

        public void Initialize(string newCharacterName, string text, bool newIsRight, Sprite fallbackAvatar)
        {
            characterName = newCharacterName?.Trim() ?? string.Empty;

            SetSide(newIsRight);
            SetCharacterName();
            SetDialogueText(text);
            SetAvatar(fallbackAvatar);
        }

        public void SetSide(bool newIsRight)
        {
            isRight = newIsRight;

            if (isRight)
            {
                dialogueTextTransform.SetSiblingIndex(0);
                characterNameTransform.SetSiblingIndex(1);
                avatarTransform.SetSiblingIndex(2);

                dialogueText.alignment = TextAlignmentOptions.Right;
            }
            else
            {
                avatarTransform.SetSiblingIndex(0);
                characterNameTransform.SetSiblingIndex(1);
                dialogueTextTransform.SetSiblingIndex(2);

                dialogueText.alignment = TextAlignmentOptions.Left;
            }

            SetCharacterName();
        }

        public void SetAvatar(Sprite avatar)
        {
            avatarImage.sprite = avatar;
            avatarImage.enabled = avatar != null;
        }

        public void ResetView()
        {
            characterName = string.Empty;
            isRight = false;

            avatarImage.sprite = null;
            avatarImage.enabled = false;

            characterNameText.SetText(string.Empty);
            characterNameText.gameObject.SetActive(false);

            dialogueText.SetText(string.Empty);

            SetSide(false);
        }

        private void SetCharacterName()
        {
            bool hasName = !string.IsNullOrWhiteSpace(characterName);

            characterNameText.gameObject.SetActive(hasName);

            if (!hasName)
            {
                characterNameText.SetText(string.Empty);
                return;
            }

            characterNameText.text = isRight ? $":{characterName}" : $"{characterName}:";
        }

        private void SetDialogueText(string text)
        {
            dialogueText.SetText(text ?? string.Empty);
        }
    }
}