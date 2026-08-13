#version 330

in vec3 fragPosition;
in vec2 fragTexCoord;
in vec3 fragNormal; //used for when normal mapping is toggled off
in vec4 fragColor;
in mat3 TBN;
uniform vec3 viewPos;

out vec4 finalColor;

struct Material
{
    vec4 tint;
};
uniform Material material;

const int MAX_LIGHT_COUNT = 16;
struct Light
{
    int type;
    float intensity;
    vec3 color;
    vec3 location;
    vec3 direction;
};
uniform Light lights[MAX_LIGHT_COUNT];
uniform int lightCount;

struct SceneLighting
{
    vec3 ambientColor;
    float ambientIntensity;
};
uniform SceneLighting sceneLighting;

float specularStrength = 0.5;

void main() {
    vec3 ambient = sceneLighting.ambientIntensity * sceneLighting.ambientColor;

    vec3 norm = normalize(fragNormal);
    vec3 lightDir = normalize(lights[0].location - fragPosition);
    vec3 viewDir = normalize(viewPos - fragPosition);
    vec3 reflectDir = reflect(-lightDir, norm);

    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = diff * (lights[0].color * lights[0].intensity);

    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
    vec3 specular = spec * (lights[0].color * lights[0].intensity) * specularStrength;

    vec3 result = (ambient + diffuse + specular) * material.tint.rgb;
    finalColor = vec4(result, 1.0);
}