using TMPro;
using UnityEngine;

namespace CardsChatVfx.Common
{
	public sealed class FpsCounterView : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI fpsText;
		[SerializeField, Min(0.05f)] private float refreshInterval = 0.25f;

		private float elapsedTime;
		private int frameCount;

		private void Update()
		{
			elapsedTime += Time.unscaledDeltaTime;
			frameCount++;

			if (elapsedTime < refreshInterval)
			{
				return;
			}

			int fps = Mathf.RoundToInt(frameCount / elapsedTime);

			fpsText.SetText($"{fps}");

			elapsedTime = 0f;
			frameCount = 0;
		}
	}
}