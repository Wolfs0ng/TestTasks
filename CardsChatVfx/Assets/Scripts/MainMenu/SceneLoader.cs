using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CardsChatVfx.MainMenu
{
	public sealed class SceneLoader
	{
		public bool IsLoading { get; private set; }

		public event Action<AppScene> LoadStarted;

		public async Task LoadAsync(AppScene scene)
		{
			if (IsLoading)
			{
				return;
			}

			string sceneName = scene.ToString();

			if (!Application.CanStreamedLevelBeLoaded(sceneName))
			{
				Debug.LogError($"Scene '{sceneName}' cannot be loaded. " +
				               "Make sure it is included in the active Build Profile.");

				return;
			}

			IsLoading = true;
			LoadStarted?.Invoke(scene);

			try
			{
				AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

				if (operation == null)
				{
					Debug.LogError($"Unity failed to create a loading operation for scene '{sceneName}'.");
					return;
				}

				while (!operation.isDone)
				{
					await Task.Yield();
				}
			}
			finally
			{
				IsLoading = false;
			}
		}
	}
}