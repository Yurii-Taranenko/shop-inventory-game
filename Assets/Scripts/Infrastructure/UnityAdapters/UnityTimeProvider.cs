using System;
using UnityEngine;
using Game.Core.Abstractions;

namespace Game.Infrastructure.UnityAdapters
{
    /// <summary>
    /// Unity implementation of ITimeProvider.
    /// </summary>
    public class UnityTimeProvider : MonoBehaviour, IUnityTimeProvider
    {
        public event Action<float> Tick;

        private void Update()
        {
            Tick?.Invoke(Time.deltaTime);
        }
    }
}
