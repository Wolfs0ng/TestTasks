using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace CardsChatVfx.MagicWords
{
	public sealed class AvatarImageLoader : IDisposable
	{
		private readonly Dictionary<string, Sprite> loadedSprites = new();
		private readonly Dictionary<string, List<Action<Sprite>>> pendingRequests = new();

		private bool isDisposed;

		public IEnumerator Load(string url, Action<Sprite> onCompleted)
		{
			if (isDisposed)
			{
				onCompleted?.Invoke(null);
				yield break;
			}

			if (!IsValidHttpUrl(url))
			{
				Debug.LogWarning($"Avatar URL is missing or invalid: '{url}'.");

				onCompleted?.Invoke(null);
				yield break;
			}

			if (loadedSprites.TryGetValue(url, out Sprite cachedSprite))
			{
				onCompleted?.Invoke(cachedSprite);
				yield break;
			}

			if (pendingRequests.TryGetValue(url, out List<Action<Sprite>> existingCallbacks))
			{
				existingCallbacks.Add(onCompleted);
				yield break;
			}

			pendingRequests.Add(url, new List<Action<Sprite>> { onCompleted });
			using UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

			request.timeout = 10;

			yield return request.SendWebRequest();

			if (isDisposed)
			{
				yield break;
			}

			Sprite loadedSprite = null;

			if (request.result == UnityWebRequest.Result.Success)
			{
				loadedSprite = CreateSprite(request, url);
			}
			else
			{
				Debug.LogWarning($"Failed to load avatar from '{url}'. Reason: {request.error}");
			}

			CompleteRequest(url, loadedSprite);
		}

		public void Dispose()
		{
			if (isDisposed)
			{
				return;
			}

			isDisposed = true;

			foreach (Sprite sprite in loadedSprites.Values)
			{
				if (sprite == null)
				{
					continue;
				}

				Texture2D texture = sprite.texture;

				UnityEngine.Object.Destroy(sprite);

				if (texture != null)
				{
					UnityEngine.Object.Destroy(texture);
				}
			}

			loadedSprites.Clear();
			pendingRequests.Clear();
		}

		private Sprite CreateSprite(UnityWebRequest request, string url)
		{
			Texture2D texture;

			try
			{
				texture = DownloadHandlerTexture.GetContent(request);
			}
			catch (Exception exception)
			{
				Debug.LogWarning($"The response from '{url}' could not be read as an image.\n" + exception.Message);
				return null;
			}

			if (texture == null || texture.width <= 1 || texture.height <= 1)
			{
				Debug.LogWarning($"The URL '{url}' did not return a valid avatar image.");

				if (texture != null)
				{
					UnityEngine.Object.Destroy(texture);
				}

				return null;
			}

			texture.name = $"AvatarTexture_{texture.GetEntityId()}";

			Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height),
				new Vector2(0.5f, 0.5f), 100f);

			sprite.name = $"AvatarSprite_{texture.GetEntityId()}";
			loadedSprites[url] = sprite;

			return sprite;
		}

		private void CompleteRequest(string url, Sprite sprite)
		{
			if (!pendingRequests.Remove(url, out List<Action<Sprite>> callbacks))
			{
				return;
			}

			for (int i = 0; i < callbacks.Count; i++)
			{
				callbacks[i]?.Invoke(sprite);
			}
		}

		private static bool IsValidHttpUrl(string url)
		{
			if (string.IsNullOrWhiteSpace(url))
			{
				return false;
			}

			if (!Uri.TryCreate(url.Trim(), UriKind.Absolute, out Uri uri))
			{
				return false;
			}

			return uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps;
		}
	}
}