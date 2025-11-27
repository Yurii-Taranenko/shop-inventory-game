using System;

namespace Game.Core.Abstractions
{
    /// <summary>
    /// Provides an update tick. Implement in Unity via a MonoBehaviour.
    /// </summary>
    public interface IUnityTimeProvider
    {
        /// <summary>
        /// Fired every frame with deltaTime
        /// </summary>
        event Action<float> Tick;
    }
}
