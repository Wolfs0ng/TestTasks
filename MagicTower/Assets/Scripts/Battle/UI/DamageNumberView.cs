using TMPro;
using UnityEngine;

namespace MagicTower.Battle.UI
{
	[RequireComponent(typeof(RectTransform))]
	[RequireComponent(typeof(CanvasGroup))]
	public class DamageNumberView : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI damageText;
		[SerializeField] private float lifetime = 0.8f;
		[SerializeField] private float upwardDistance = 60f;
		[SerializeField] private float randomHorizontalOffset = 20f;

		private RectTransform rectTransform;
		private CanvasGroup canvasGroup;
		private Vector2 startPosition;
		private Vector2 endPosition;
		private float timer;
		private bool isInitialized;

		private void Awake()
		{
			rectTransform = GetComponent<RectTransform>();
			canvasGroup = GetComponent<CanvasGroup>();
		}

		private void Update()
		{
			if (!isInitialized)
			{
				return;
			}

			timer += Time.deltaTime;

			float normalizedTime = Mathf.Clamp01(timer / Mathf.Max(0.01f, lifetime));

			rectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, normalizedTime);
			canvasGroup.alpha = 1f - normalizedTime;

			if (normalizedTime >= 1f)
			{
				Destroy(gameObject);
			}
		}

		public void Initialize(int damageAmount, Vector3 worldPosition, RectTransform canvasContainer,
			Camera worldCamera, Color color)
		{
			if (damageText != null)
			{
				damageText.text = damageAmount.ToString();
				damageText.color = color;
			}

			Vector3 screenPosition = worldCamera.WorldToScreenPoint(worldPosition);

			if (screenPosition.z < 0f)
			{
				Destroy(gameObject);
				return;
			}

			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasContainer, screenPosition,
				    null, out Vector2 localPosition))
			{
				Destroy(gameObject);
				return;
			}

			float horizontalOffset = Random.Range(-randomHorizontalOffset, randomHorizontalOffset);

			startPosition = localPosition + new Vector2(horizontalOffset, 0f);
			endPosition = startPosition + new Vector2(0f, upwardDistance);

			rectTransform.anchoredPosition = startPosition;
			canvasGroup.alpha = 1f;

			timer = 0f;
			isInitialized = true;
		}
	}
}