#version 330 core

out vec4 FragColor;

in vec3 FragPos;
in vec3 Normal;
in vec2 TexCoords;

struct Light {
	vec3 position;
	vec3 color;
	float intensity;
};

uniform sampler2D texture_diffuse1;
uniform sampler2D texture_specular1;
uniform float shininess;
uniform float shininessModifier;
uniform float matteV;
uniform float shinnyV;
uniform float glossyV;
uniform bool matte;
uniform bool shinny;
uniform bool glossy;
uniform vec3 viewPos;
uniform Light light;

void main()
{
	// ambient
	float ambientStrength = 0.5f;
	vec3 ambient = light.intensity * 0.2 * texture(texture_diffuse1, TexCoords).rgb;

	// diffuse 
	vec3 norm = normalize(Normal);
	vec3 lightDir = normalize(light.position - FragPos);
	float diff = max(dot(norm, lightDir), 0.0);
	vec3 diffuse = light.intensity * light.color * diff * texture(texture_diffuse1, TexCoords).rgb;

	// specular
	vec3 viewDir = normalize(viewPos - FragPos);
	vec3 reflectDir = reflect(-lightDir, norm);
	float spec = pow(max(dot(viewDir, reflectDir), 0.0), shininess);
	vec3 specular;
	vec3 halfwayDir;
	
	
	if (matte) {
	specular = light.color * spec * matteV * texture(texture_specular1, TexCoords).rgb;
	}
	else if(shinny){
	specular = light.color * spec * shinnyV * texture(texture_specular1, TexCoords).rgb;
	}
	else if(glossy){
	specular = light.color * spec * glossyV * texture(texture_specular1, TexCoords).rgb;
	}
	else{
	specular = light.intensity * light.color * spec * texture(texture_specular1, TexCoords).rgb;
	}
	
	//specular = light.intensity * light.color * spec * texture(texture_specular1, TexCoords).rgb;
    //glossy metrio diffuse kai megalo specular
	vec3 result = ambient + diffuse + specular;
	FragColor = vec4(result, 1.0);
}