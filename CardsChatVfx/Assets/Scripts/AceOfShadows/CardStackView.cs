using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CardsChatVfx.AceOfShadows
{
    public sealed class CardStackView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform cardsRoot;
        [SerializeField] private TextMeshProUGUI counterText;
        [SerializeField] private List<CardView> cards = new();

        [Header("Layout")]
        [SerializeField] private Vector2 cardOffset = new(0f, 0.5f);

        public int Count => cards.Count;

        public void Initialize()
        {
            RemoveMissingCardReferences();

            for (int i = 0; i < cards.Count; i++)
            {
                cards[i].AttachTo(cardsRoot, GetCardPosition(i));
            }

            RefreshCounter();
        }

        public void AddCard(CardView card)
        {
            if (card == null)
            {
                return;
            }

            card.AttachTo(cardsRoot, GetNextCardPosition());

            cards.Add(card);

            RefreshCounter();
        }

        public CardView RemoveTopCard()
        {
            if (cards.Count == 0)
            {
                return null;
            }

            int topCardIndex = cards.Count - 1;
            CardView topCard = cards[topCardIndex];

            cards.RemoveAt(topCardIndex);

            RefreshCounter();

            return topCard;
        }

        public Vector3 GetNextCardWorldPosition()
        {
            return cardsRoot.TransformPoint(GetNextCardPosition());
        }

        private void RemoveMissingCardReferences()
        {
            for (int i = cards.Count - 1; i >= 0; i--)
            {
                if (cards[i] == null)
                {
                    cards.RemoveAt(i);
                }
            }
        }

        private Vector2 GetNextCardPosition()
        {
            return GetCardPosition(cards.Count);
        }

        private Vector2 GetCardPosition(int cardIndex)
        {
            return cardOffset * cardIndex;
        }

        private void RefreshCounter()
        {
            counterText.SetText("{0}", cards.Count);
        }
    }
}