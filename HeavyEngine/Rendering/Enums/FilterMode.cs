using System;
using System.Collections.Generic;
using System.Text;

using OpenTK.Graphics.OpenGL4;

namespace HeavyEngine.Rendering {
    public enum FilterMode {
        Linear = TextureMagFilter.Linear,
        Nearest = TextureMagFilter.Nearest
    }
}
