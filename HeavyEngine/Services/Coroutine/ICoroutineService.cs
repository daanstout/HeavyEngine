using System.Collections;

namespace HeavyEngine {
    /// <summary>
    /// The <see cref="ICoroutineService"/> allows you to start coroutines that run over multiple frames
    /// </summary>
    public interface ICoroutineService {
        /// <summary>
        /// Starts a new coroutine
        /// </summary>
        /// <param name="coroutine">The coroutine to start</param>
        void StartCoroutine(IEnumerator coroutine);
    }
}