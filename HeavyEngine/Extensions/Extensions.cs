using System;
using System.Collections.Generic;
using System.Text;

namespace HeavyEngine.Extensions {
    public static class Extensions {
        public static void Inject(this object obj) {
            DependencyObtainer.PrimaryInjector.Inject(obj);
        }
    }
}
