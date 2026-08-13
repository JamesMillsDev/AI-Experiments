#version 330

// Input vertex attributes
in vec3 vertexPosition;
in vec2 vertexTexCoord;
in vec3 vertexNormal;
in vec4 vertexTangent;
in vec4 vertexColor;

uniform mat4 mvp;
uniform mat4 matModel;

// Output vertex attributes (to fragment shader)
out vec3 fragPosition;
out vec2 fragTexCoord;
out vec3 fragNormal; //used for when normal mapping is toggled off
out vec4 fragColor;
out mat3 TBN;

void main() {
    fragTexCoord = vertexTexCoord;
    fragColor = vertexColor;
    fragPosition = vec3(matModel * vec4(vertexPosition, 1.0));

    vec3 vertexBiNormal = cross(vertexNormal, vertexTangent.xyz) * vertexTangent.w;
    mat3 normalMatrix = mat3(transpose(inverse(matModel)));

    fragNormal = normalize(normalMatrix * vertexNormal);

    vec3 fragTangent = normalize(normalMatrix * vertexTangent.xyz);
    fragTangent = normalize(fragTangent - dot(fragTangent, fragNormal) * fragNormal);

    vec3 fragBinormal = normalize(normalMatrix * vertexBiNormal);
    fragBinormal = cross(fragNormal, fragTangent);

    TBN = transpose(mat3(fragTangent, fragBinormal, fragNormal));

    gl_Position = mvp * vec4(vertexPosition, 1.0);
}