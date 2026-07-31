using UnityEngine;

namespace CardsChatVfx.AceOfShadows
{
	public sealed class RectTransformScaleFitter : MonoBehaviour
	{
		[SerializeField] private RectTransform container;
		[SerializeField] private RectTransform scalableContent;
		[SerializeField] private Vector2 referenceSize = new(500f, 830);
		[SerializeField] private bool preventUpscaling = true;

		private Vector2 previousContainerSize;

		private void OnEnable()
		{
			RefreshScale();
		}

		private void OnRectTransformDimensionsChange()
		{
			RefreshScale();
		}

		private void RefreshScale()
		{
			if (container == null || scalableContent == null)
			{
				return;
			}

			Vector2 containerSize = container.rect.size;

			if (containerSize == previousContainerSize)
			{
				return;
			}

			previousContainerSize = containerSize;

			float widthScale = containerSize.x / referenceSize.x;
			float heightScale = containerSize.y / referenceSize.y;
			float scale = Mathf.Min(widthScale, heightScale);

			if (preventUpscaling)
			{
				scale = Mathf.Min(scale, 1f);
			}

			scalableContent.localScale = new Vector3(scale, scale, 1f);
		}
	}
}