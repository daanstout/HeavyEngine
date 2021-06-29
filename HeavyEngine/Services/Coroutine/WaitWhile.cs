using System;
using System.Collections;

namespace HeavyEngine {
    /// <summary>
    /// Returns an <see cref="IEnumerator"/> that waits while the specified function evaluates to true
    /// </summary>
    public sealed class WaitWhile : IEnumerator {
        private readonly Func<bool> func;

        public WaitWhile(Func<bool> func) => this.func = func;

        public object Current => null;

        public bool MoveNext() => func();
        public void Reset() {}
    }
}
