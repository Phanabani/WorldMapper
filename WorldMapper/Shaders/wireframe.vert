#version 330 core

uniform mat4 projectionMatrix;
uniform mat4 viewMatrix;
uniform mat4 modelMatrix;

in vec3 position;
in vec3 barycentric;
in float even;

out vec3 vBarycentric;
out vec3 vPosition;

void main() {
	gl_Position = projectionMatrix * viewMatrix * modelMatrix * vec4(position.xyz, 1.0);
	vBarycentric = barycentric;
	vPosition = position.xyz;
}
