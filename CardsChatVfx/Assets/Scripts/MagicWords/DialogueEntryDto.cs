using System;
using UnityEngine;

namespace CardsChatVfx.MagicWords
{
	[Serializable]
	public sealed class DialogueEntryDto
	{
		[SerializeField] private string name;
		[SerializeField] private string text;

		public string Name => name;
		public string Text => text;
	}
}