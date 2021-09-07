using System;
using System.Collections.Generic;
using System.Text;

using HeavyEngine;

using OpenTK.Mathematics;

using Xunit;

namespace HeavyUnitTests {
    public class TransformTest {
        [Fact]
        public void Transform_GlobalPosition_UpdatesWhenParentMoves() {
            // Arrange
            var child = new Transform();
            var parent = new Transform();

            child.SetParent(parent);

            // Act
            parent.Position = Vector3.One;

            // Assert
            Assert.Equal(Vector3.One, child.GlobalPosition);
        }
        
        [Fact]
        public void Transform_GlobalPosition_UpdatesWhenParentRotates() {
            // Arrange
            var child = new Transform();
            var parent = new Transform();
            child.Position = Vector3.One;
            child.SetParent(parent);

            // Act
            parent.Rotation = Quaternion.FromEulerAngles(Mathf.PI / 2.0f, 0.0f, 0.0f);

            // Assert
            Assert.Equal(new Vector3(1.0f, -1.0f, 1.0f), child.GlobalPosition);
        }

        [Fact]
        public void Transform_Position_UpdatesWhenGlobalPositionIsChanged() {
            // Arrange
            var child = new Transform();
            var parent = new Transform();
            child.SetParent(parent);

            // Act
            parent.Position = Vector3.One;
            child.GlobalPosition = Vector3.Zero;

            // Assert
            Assert.Equal(-Vector3.One, child.Position);
        }
    }
}
