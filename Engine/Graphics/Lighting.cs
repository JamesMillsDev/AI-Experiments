using System.Numerics;
using Raylib_cs;

namespace Engine.Graphics
{
    public class Lighting
    {
        private const int MAX_LIGHTS = 16;

        public Color ambientColor = Color.White;
        public float ambientIntensity = .1f;

        private readonly List<LightComponent> lightComponents = [];

        internal void AddLight(LightComponent light)
        {
            if (lightComponents.Count + 1 >= MAX_LIGHTS)
            {
                return;
            }

            lightComponents.Add(light);
        }

        internal void RemoveLight(LightComponent light) => lightComponents.Remove(light);

        internal void SetShaderValues(Shader shader)
        {
            int sceneLightingAmbientColorIndex = Raylib.GetShaderLocation(shader, "sceneLighting.ambientColor");
            int sceneLightingAmbientIntensityIndex = Raylib.GetShaderLocation(shader, "sceneLighting.ambientIntensity");
            int lightCountIndex = Raylib.GetShaderLocation(shader, "lightCount");

            if (lightCountIndex != -1)
            {
                Raylib.SetShaderValue(
                    shader, lightCountIndex, lightComponents.Count, ShaderUniformDataType.Int
                );
            }

            for (int i = 0; i < lightComponents.Count; i++)
            {
                lightComponents[i].SetShaderValues(shader, i);
            }

            if (sceneLightingAmbientColorIndex != -1)
            {
                Raylib.SetShaderValue(
                    shader, sceneLightingAmbientColorIndex,
                    new Vector3(ambientColor.R / 255f, ambientColor.G / 255f, ambientColor.B / 255f),
                    ShaderUniformDataType.Vec3
                );
            }

            if (sceneLightingAmbientIntensityIndex != -1)
            {
                Raylib.SetShaderValue(
                    shader, sceneLightingAmbientIntensityIndex, ambientIntensity, ShaderUniformDataType.Float
                );
            }
        }
    }
}