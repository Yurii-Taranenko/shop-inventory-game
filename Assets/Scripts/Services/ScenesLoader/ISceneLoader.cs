using System;

namespace Game.Services.Scene
{
    /// <summary>
    /// Abstract scene loader used by bootstrap and other systems.
    /// </summary>
    public interface ISceneLoader
    {
        /// <summary>
        /// Start loading a scene by name.
        /// - sceneName: the target scene to load
        /// - additive: whether to load additively (default false)
        /// - onComplete: called when loading is finished (scene activated)
        /// - onProgress: optional progress callback [0..1]
        void LoadScene(string sceneName, bool additive = false, Action onComplete = null, Action<float> onProgress = null);

        /// <summary>
        /// Same as LoadScene but returns immediately and allows multiple loads.
        /// </summary>
        void UnloadScene(string sceneName, Action onComplete = null);
    }
}
