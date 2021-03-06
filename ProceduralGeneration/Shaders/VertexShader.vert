#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec2 aNormal;
layout (location = 2) in vec2 aTexCoord;

out vec2 texCoord;

uniform mat4 transform;
uniform mat4 view;
uniform mat4 projection;

void main()
{
    texCoord = aTexCoord;
//    gl_Position = projection * view * transform * vec4(aPosition, 1.0);
//    gl_Position = transform * view * projection * vec4(aPosition, 1.0);
    gl_Position = vec4(aPosition, 1.0) * transform * view * projection;
}