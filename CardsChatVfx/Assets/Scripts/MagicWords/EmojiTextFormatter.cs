using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CardsChatVfx.MagicWords
{
	public static class EmojiTextFormatter
	{
		private static readonly Regex EmojiTokenRegex = new(@"\{(?<key>[A-Za-z0-9_]+)\}", RegexOptions.Compiled);

		private static readonly IReadOnlyDictionary<string, string> EmojiMap =
			new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
			{
				["satisfied"] = char.ConvertFromUtf32(0x1F60C), // 😌
				["intrigued"] = char.ConvertFromUtf32(0x1F914), // 🤔
				["neutral"] = char.ConvertFromUtf32(0x1F610), // 😐
				["affirmative"] = char.ConvertFromUtf32(0x1F44D), // 👍
				["laughing"] = char.ConvertFromUtf32(0x1F602), // 😂
				["win"] = char.ConvertFromUtf32(0x1F3C6) // 🏆
			};

		public static string Format(string source)
		{
			if (string.IsNullOrEmpty(source))
			{
				return string.Empty;
			}

			return EmojiTokenRegex.Replace(source, ReplaceToken);
		}

		private static string ReplaceToken(Match match)
		{
			string key = match.Groups["key"].Value;
			return EmojiMap.TryGetValue(key, out string emoji) ? emoji : match.Value;
		}
	}
}