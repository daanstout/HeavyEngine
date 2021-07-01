﻿#version 330 core
out vec4 FragColor;

in vec2 texCoord;

uniform sampler2D texture0;

void main()
{
    //FragColor = mix(texture(texture0, texCoord), texture(texture1, texCoord), 0.2);
    FragColor = texture(texture0, texCoord);
}