#version 450 core
//in vec2 vs_textureCoordinate;
in vec3 vs_normal;
in vec3 FragPos;
//uniform sampler2D textureObject;
out vec4 color;
layout(location = 23) uniform  vec3 lightPos;
layout(location = 24) uniform vec3 viewPos;
layout(location = 25) uniform float objColorMin;
layout(location = 26) uniform float objColorMax;
void main(void)
{
	vec3 norm = normalize(vs_normal);
	if(gl_FrontFacing == false) norm *= -1;
	//norm = vec3(0);
	vec3 lightDir = normalize(lightPos - FragPos);
	float diff = max(dot(norm, lightDir), 0.0);
	vec3 diffuse = diff * vec3(1,1,1);
	float ambientStrength = 0.1;
	vec3 ambient = ambientStrength * vec3(1.0f, 1.0f, 1.0f);
	float specularStrength = 0.5;
	vec3 viewDir = normalize(viewPos - FragPos);
	vec3 reflectDir = reflect(-lightDir, norm);
	float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
	vec3 specular = specularStrength * spec * vec3(1.0f, 1.0f, 1.0f);

	float intercolor = smoothstep(objColorMin, objColorMax, FragPos.y);	
	
	//float intercolor = (FragPos.y - objColorMin) / (objColorMax-objColorMin);
	vec3 objColor;
	
	objColor = vec3(0+intercolor, 1-intercolor, 0);
	vec3 result = (ambient + diffuse + specular) * objColor;
	color =  vec4(result, 1.0f);

	
}