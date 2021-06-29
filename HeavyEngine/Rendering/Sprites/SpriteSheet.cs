using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using OpenTK.Mathematics;

namespace HeavyEngine.Rendering.Sprites {
    public class SpriteSheet {
        //private readonly Bitmap sheet;
        //private Vector2i size;

        private SpriteSheet() { }

        public static SpriteSheet Create(string filepath) {
            var sheet = new SpriteSheet();

            return sheet;
        }
    }
}
