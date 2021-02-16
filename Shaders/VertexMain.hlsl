#version 450 core
layout(location = 0) in vec3 position;
layout(location = 1) in vec3 aNormal;
out vec3 vs_normal;
out vec3 FragPos; 

layout(location = 20) uniform  mat4 projection;
layout(location = 21) uniform  mat4 model;
layout(location = 22) uniform  mat4 view;

void main(void)
{
	gl_Position = projection * view * model * vec4(position, 1.0f);
	vs_normal = mat3(transpose(inverse(model))) * aNormal;
	//this is fine ^ but cost inefficient
	// vs_normal = aNormal;
	FragPos = vec3(model * vec4(position, 1.0f));
}