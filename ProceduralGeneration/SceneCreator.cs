using System;
using System.Collections.Generic;
using System.Text;

using HeavyEngine;

using ProceduralGeneration.Scenes;

namespace ProceduralGeneration {
    public static class SceneCreator {
        public static Scene GetMainScene() => SceneMain.GetScene();
    }
}
