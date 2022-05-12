#version 330

out vec4 outputColor;

in vec3 ourColor;
in vec2 texCoord;

uniform sampler2D ourTexture;

void main()
{
    vec4 texColor = texture(ourTexture, texCoord);
    outputColor = vec4(ourColor, 1.0f) * texColor;
}
