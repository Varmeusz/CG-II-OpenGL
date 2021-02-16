#version 450 core
in vec2 vs_textureCoordinate;
in vec3 vs_normal;
in vec3 FragPos;
uniform sampler2D textureObject;
out vec4 color;
layout(location = 23) uniform  vec3 lightPos;
layout(location = 24) uniform vec3 viewPos;
void main(void)
{
	vec3 norm = normalize(vs_normal);
	if(gl_FrontFacing == false) norm *= -1;
	//norm = vec3(0);
	vec3 lightDir = normalize(lightPos - FragPos);
	float diff = max(dot(norm, lightDir), 0.0);
	vec3 diffuse = diff * vec3(1,1,1);
	float ambientStrength = 0.3;
	vec3 ambient = ambientStrength * vec3(1.0f, 1.0f, 1.0f);
	float specularStrength = 0.5;
	vec3 viewDir = normalize(viewPos - FragPos);
	vec3 reflectDir = reflect(-lightDir, norm);
	float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
	vec3 specular = specularStrength * spec * vec3(1.0f, 1.0f, 1.0f);
	//vec2 flipped_texcoord = vec2(vs_textureCoordinate.x, 1.0 - vs_textureCoordinate.y);
	//vec3 tex_color =  vec3( texture(textureObject, flipped_texcoord));
	vec3 tex_color =  vec3( texture(textureObject, vs_textureCoordinate));
	vec3 result = (ambient + diffuse + specular) * tex_color;
	//vec3 result = (ambient  + specular) * tex_color;
  // color = vec4(Color,1.0f);
		//color = texelFetch(textureObject, ivec2(vs_textureCoordinate.x, vs_textureCoordinate.y), 0);
	color =  vec4(result, 1.0f);

	
}