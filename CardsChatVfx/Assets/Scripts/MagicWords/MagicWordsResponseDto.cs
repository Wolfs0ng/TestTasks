using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardsChatVfx.MagicWords
{
	[Serializable]
	public sealed class MagicWordsResponseDto
	{
		[SerializeField] private List<DialogueEntryDto> dialogue = new();
		[SerializeField] private List<AvatarEntryDto> avatars = new();

		public IReadOnlyList<DialogueEntryDto> Dialogue => dialogue;
		public IReadOnlyList<AvatarEntryDto> Avatars => avatars;
	}
}