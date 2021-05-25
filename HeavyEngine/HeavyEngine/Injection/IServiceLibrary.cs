using System;

namespace HeavyEngine.Injection {
    public interface IServiceLibrary {
        void AddSingleton<TAbstract, TImplementation>(string tag = null) where TImplementation : class, new();
        void AddTransient<TAbstract, TImplementation>(string tag = null) where TImplementation : class, new();
        bool BindTag(string tag, string target);
        TAbstract Get<TAbstract>(string tag = null);
        object Get(Type type, string tag = null);
    }
}