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
            var parent = new Transform();
            var child = new Transform();
            child.SetParent(parent);
            child.Position = new Vector3(1, 1, 1);

            parent.Rotation = Quaternion.FromEulerAngles(Mathf.PI / 2.0f, 0.0f, 0.0f);
            Console.WriteLine(child.GlobalPosition);


            child.Position = new Vector3(2, 0, 0);
            Console.WriteLine(child.GlobalPosition);

            parent.Rotation = Quaternion.FromEulerAngles(0.0f, Mathf.PI / 2, 0.0f);
            Console.WriteLine(child.GlobalPosition);

            parent.Rotation = Quaternion.Identity;
            child.GlobalPosition = new Vector3(0, 0, 0);
            Console.WriteLine(child.Position);
            Console.WriteLine(child.GlobalPosition);

            var trans3 = new Transform();
            trans3.SetParent(child);
            Console.WriteLine(trans3.GlobalPosition);
        }

        static void InputTest() {
            var inputService = new HeavyEngine.Input.InputService();
            var context = new object();

            var key = inputService.CreateTriggerKey(context);
            context = null;
            

        }
    }
}
