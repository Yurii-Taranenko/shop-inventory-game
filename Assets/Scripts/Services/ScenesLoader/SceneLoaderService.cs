using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Services.Scene
{
    /// <summary>
    /// Pure C# async-based scene loader without MonoBehaviour.
    /// </summary>
    public class SceneLoaderService : ISceneLoader
    {
        public async void LoadScene(
            string sceneName,
            bool additive = false,
            Action onComplete = null,
            Action<float> onProgress = null)
        {
            if (string.IsNullOrWhiteSpace(sceneName))
            {
                Debug.LogError("[SceneLoaderService] sceneName is null or empty.");
                onComplete?.Invoke();
                return;
            }

            try
            {
                await LoadSceneInternal(sceneName, additive, onProgress);
                onComplete?.Invoke();
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SceneLoaderService] Load failed: {ex}");
                onComplete?.Invoke();
            }
        }

        public async void UnloadScene(string sceneName, Action onComplete = null)
        {
            try
            {
                await UnloadSceneInternal(sceneName);
            }
            finally
            {
                onComplete?.Invoke();
            }
        }

        private async Task LoadSceneInternal(
            string sceneName,
            bool additive,
            Action<float> onProgress)
        {
            var mode = additive ? LoadSceneMode.Additive : LoadSceneMode.Single;
            var op = SceneManager.LoadSceneAsync(sceneName, mode);

            if (op == null)
                throw new Exception($"Failed LoadSceneAsync: {sceneName}");

            op.allowSceneActivation = false;

            while (!op.isDone)
            {
                float raw = Mathf.Clamp01(op.progress / 0.9f);
                onProgress?.Invoke(raw);

                if (op.progress >= 0.9f)
                {
                    onProgress?.Invoke(1f);
                    op.allowSceneActivation = true;
                }

                await Task.Yield();
            }

            if (!additive)
            {
                var scene = SceneManager.GetSceneByName(sceneName);
                if (scene.IsValid())
                    SceneManager.SetActiveScene(scene);
            }
        }

        private async Task UnloadSceneInternal(string sceneName)
        {
            var scene = SceneManager.GetSceneByName(sceneName);
            if (!scene.IsValid())
                return;

            var op = SceneManager.UnloadSceneAsync(scene);
            if (op == null)
                return;

            while (!op.isDone)
                await Task.Yield();
        }
    }
}
