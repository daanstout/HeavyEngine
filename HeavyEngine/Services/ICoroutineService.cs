using System.Collections;

namespace HeavyEngine {
    public interface ICoroutineService {
        void StartCoroutine(IEnumerator coroutine);
    }
}