using System;

using HeavyEngine;

using OpenTK.Mathematics;

namespace TestConsole {
    class Program {
        static void Main() {
            //VecMath();
            TransformPositionMath();

            Console.ReadKey();
        }

        static void VecMath() {
            var pos1 = new Vector3(1.0f, 0.0f, 0.0f);
            var quat = Quaternion.FromEulerAngles(0.0f, MathF.PI / 2, 0.0f);
            var mat1 = Matrix4.CreateFromQuaternion(quat) * Matrix4.CreateTranslation(pos1);
            var pos2 = new Vector3(2.0f, 0.0f, 0.0f);
            var res = new Vector4(pos2, 1.0f) * mat1;
            var res2 = mat1 * new Vector4(pos2, 1.0f);
            Console.WriteLine(res.Xyz);
            Console.WriteLine(res2.Xyz);
        }

        static void TransformPositionMath() {
            var trans1 = new Transform();
            var trans2 = new Transform();
            trans2.SetParent(trans1);
            trans1.Position = new Vector3(1, 1, 1);
            Console.WriteLine(trans2.GlobalPosition);

            trans2.Position = new Vector3(2, 0, 0);
            Console.WriteLine(trans2.GlobalPosition);

            trans1.Rotation = Quaternion.FromEulerAngles(0.0f, Mathf.PI / 2, 0.0f);
            Console.WriteLine(trans2.GlobalPosition);

            trans1.Rotation = Quaternion.Identity;
            trans2.GlobalPosition = new Vector3(0, 0, 0);
            Console.WriteLine(trans2.Position);
            Console.WriteLine(trans2.GlobalPosition);

            var trans3 = new Transform();
            trans3.SetParent(trans2);
            Console.WriteLine(trans3.GlobalPosition);
        }
    }
}
