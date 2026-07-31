using System.Collections;
using UnityEngine;

namespace CardsChatVfx.AceOfShadows
{
    public sealed class CardView : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;

        public void AttachTo(RectTransform parent, Vector2 anchoredPosition)
        {
            rectTransform.SetParent(parent, false);
            rectTransform.anchoredPosition = anchoredPosition;
            rectTransform.localScale = Vector3.one;
            rectTransform.localRotation = Quaternion.identity;
            rectTransform.SetAsLastSibling();
        }

        public IEnumerator MoveTo(RectTransform animationLayer, Vector3 destinationWorldPosition,
	        float duration, float arcHeight, AnimationCurve movementCurve)
        {
            Vector3 startWorldPosition = rectTransform.position;

            rectTransform.SetParent(animationLayer, true);
            rectTransform.SetAsLastSibling();

            Vector3 startPosition = animationLayer.InverseTransformPoint(startWorldPosition);
            Vector3 destinationPosition = animationLayer.InverseTransformPoint(destinationWorldPosition);

            rectTransform.localPosition = startPosition;

            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.unscaledDeltaTime;

                float normalizedTime = Mathf.Clamp01(elapsedTime / duration);
                float evaluatedTime = movementCurve.Evaluate(normalizedTime);
                Vector3 position = Vector3.LerpUnclamped(startPosition, destinationPosition, evaluatedTime);

                position.y += Mathf.Sin(normalizedTime * Mathf.PI) * arcHeight;
                rectTransform.localPosition = position;

                yield return null;
            }

            rectTransform.localPosition = destinationPosition;
        }
    }
}