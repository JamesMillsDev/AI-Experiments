#version 330

in vec3 fragPosition;
in vec2 fragTexCoord;
in vec3 fragNormal; //used for when normal mapping is toggled off
in vec4 fragColor;
in mat3 TBN;
uniform vec3 viewPos;

out vec4 finalColor;

const float PI = 3.14159265359;

struct Material
{
    vec4 tint;
    float ao;
    float roughness;
    float metallic;

    sampler2D baseColor;
    sampler2D normalMap;
    sampler2D orm;

    int baseColorSet;
    int normalMapSet;
    int ormSet;
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

    float radius;
};
uniform Light lights[MAX_LIGHT_COUNT];
uniform int lightCount;

struct SceneLighting
{
    vec3 ambientColor;
    float ambientIntensity;
};
uniform SceneLighting sceneLighting;

vec4 trySampleMap(sampler2D tex, int condition, vec4 fallback)
{
    if (condition == 0)
    {
        return fallback;
    }

    return texture(tex, fragTexCoord);
}

vec3 mapNormals()
{
    if (material.normalMapSet == 0)
    {
        return fragNormal;
    }

    vec3 normal = texture(material.normalMap, fragTexCoord).rgb;
    normal = normalize(normal * 2.0 - 1.0);
    return normalize(normal * TBN);
}

vec3 getLightDirection(Light light)
{
    if (light.type == 0)
    {
        return normalize(light.direction);
    }

    return normalize(light.location - fragPosition);
}

float attenuation(Light light, float distance)
{
    if(light.type == 0)
    {
        // directional lights will always have an attenuation of 1 since
        // they are infinite.
        return 1.0;
    }

    float d2 = distance * distance;
    const float constant = 1.0;
    float linear = 2.0 / max(light.radius, 0.000001); // max used to prevent division by 0
    float quadratic = 1.0 / max(light.radius * light.radius, 0.000001); // max used to prevent division by 0
    return 1.0 / max(constant + linear * distance + quadratic * d2, 0.000001); // max used to prevent division by 0
}

vec3 fresnelSchlick(float cosTheta, vec3 F0)
{
    return F0 + (1.0 - F0) * pow(clamp(1.0 - cosTheta, 0.0, 1.0), 5.0);
}

float distributionGGX(float NdotH, float roughness)
{
    float a = roughness * roughness;
    float a2 = a * a;
    float NdotH2 = NdotH * NdotH;

    float num = a2;
    float denom = (NdotH2 * (a2 - 1.0) + 1.0);
    denom = PI * denom * denom;

    return num / denom;
}

float geometrySchlickGGX(float NdotV, float roughness)
{
    float r = (roughness + 1.0);
    float k = (r * r) / 8.0;

    float num = NdotV;
    float denom = NdotV * (1.0 - k) + k;

    return num / denom;
}
float geometrySmith(float NdotV, float NdotL, float roughness)
{
    float ggx2 = geometrySchlickGGX(NdotV, roughness);
    float ggx1 = geometrySchlickGGX(NdotL, roughness);

    return ggx1 * ggx2;
}

void main() {
    vec3 albedo = (material.tint * trySampleMap(material.baseColor, material.baseColorSet, vec4(1.0))).rgb;
    //vec3 ambient = sceneLighting.ambientIntensity * sceneLighting.ambientColor;

    vec3 orm = trySampleMap(material.orm, material.ormSet, vec4(material.ao, material.roughness, material.metallic, 1.0)).rgb;
    float ao = orm.r;
    float roughness = orm.g;
    float metallic = orm.b;

    vec3 N = mapNormals();
    vec3 V = normalize(viewPos - fragPosition);

    vec3 Lo = vec3(0.0);

    if (lightCount > 0)
    {
        vec3 F0 = vec3(0.04);
        F0 = mix(F0, albedo, metallic);
        float NdotV = max(dot(N, V), 0.0);

        for (int i = 0; i < lightCount; ++i)
        {
            Light light = lights[i];

            vec3 L = getLightDirection(light);
            vec3 H = normalize(V + L);

            float HdotV = max(dot(H, V), 0.0);
            float NdotH = max(dot(N, H), 0.0);
            float NdotL = max(dot(N, L), 0.0);

            float distance = length(L);

            float attenuation = attenuation(light, distance);
            vec3 radiance = light.color * light.intensity * attenuation;

            vec3 F = fresnelSchlick(HdotV, F0);
            float NDF = distributionGGX(NdotH, roughness);
            float G = geometrySmith(NdotV, NdotL, roughness);

            vec3 numerator = NDF * G * F;
            float denominator = 4.0 * NdotV * NdotL + 0.0001;
            vec3 specular = numerator / denominator;

            vec3 kS = F;
            vec3 kD = vec3(1.0) - kS;

            kD *= 1.0 - metallic;

            Lo += (kD * albedo / PI + specular) * radiance * NdotL;
        }

        vec3 ambient = vec3(0.03) * albedo * ao;
        vec3 color = ambient + Lo;

        // Tonemapping
        color = color / (color + vec3(1.0));
        color = pow(color, vec3(1.0 / 2.2));

        finalColor = vec4(color, 1.0);
        return;
    }

    finalColor = vec4(sceneLighting.ambientIntensity * sceneLighting.ambientColor, 1.0);
}