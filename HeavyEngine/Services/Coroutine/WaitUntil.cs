using System;
using System.Collections;

namespace HeavyEngine {
    /// <summary>
    /// Returns an <see cref="IEnumerator"/> that waits until the specified function evaluates to true
    /// </summary>
    public class WaitUntil : IEnumerator {
        private readonly Func<bool> func;

        public WaitUntil(Func<bool> func) => this.func = func;


        public object Current => null;

        public bool MoveNext() => !func();
        public void Reset() { }
    }
}
