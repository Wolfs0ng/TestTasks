using System;
using UnityEngine;

namespace CardsChatVfx.MagicWords
{
	[Serializable]
	public sealed class AvatarEntryDto
	{
		[SerializeField] private string name;
		[SerializeField] private string url;
		[SerializeField] private string position;

		public string Name => name;
		public string Url => url;
		public string Position => position;
	}
}