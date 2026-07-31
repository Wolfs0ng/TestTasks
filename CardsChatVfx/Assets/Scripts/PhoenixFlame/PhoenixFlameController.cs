using UnityEngine;
using UnityEngine.UI;

namespace CardsChatVfx.PhoenixFlame
{
	public sealed class PhoenixFlameController : MonoBehaviour
	{
		private string ChangeColorTrigger = "ChangeColor";

		[SerializeField] private Animator flameAnimator;
		[SerializeField] private Button changeColorButton;

		private void Awake()
		{
			changeColorButton.onClick.AddListener(ChangeColor);
		}

		private void OnDestroy()
		{
			changeColorButton.onClick.RemoveListener(ChangeColor);
		}

		private void ChangeColor()
		{
			flameAnimator.SetTrigger(Animator.StringToHash(ChangeColorTrigger));
		}
	}
}