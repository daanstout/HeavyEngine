using System;
using System.Collections.Generic;
using System.Text;

namespace HeavyEngine.Injection {
    public interface IServiceContainer<out T> where T : class, new(){
        T Get();
    }
}
