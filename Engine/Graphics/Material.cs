using System.Numerics;
using Engine.Core;
using Raylib_cs;
using RayShader = Raylib_cs.Shader;

namespace Engine.Graphics
{
    public class Material
    {
        public Color tint = Color.White;
        public float ambientOcclusion = 0f;
        public float metallic = .5f;
        public float roughness = .5f;

        private readonly Dictionary<string, Texture2D> textures = [];
        internal readonly RayShader shader;

        // Shader locations
        private readonly int tintLoc;
        private readonly int ambientOcclusionLoc;
        private readonly int metallicLoc;
        private readonly int roughnessLoc;
        private readonly int viewPosLoc;

        private readonly Dictionary<string, int> textureLocations = [];

        public Material(string shaderPath)
        {
            shader = Raylib.LoadShader($"Content/Shaders/{shaderPath}.vert", $"Content/Shaders/{shaderPath}.frag");

            tintLoc = Raylib.GetShaderLocation(shader, "material.tint");
            ambientOcclusionLoc = Raylib.GetShaderLocation(shader, "material.ao");
            metallicLoc = Raylib.GetShaderLocation(shader, "material.metallic");
            roughnessLoc = Raylib.GetShaderLocation(shader, "material.roughness");
            viewPosLoc = Raylib.GetShaderLocation(shader, "viewPos");
        }

        public void SetTexture(string textureName, Texture2D texture)
        {
            textures[textureName] = texture;
            textureLocations[textureName] = Raylib.GetShaderLocation(shader, "material." + textureName);
        }

        public void RemoveTexture(string textureName)
        {
            if (!textures.Remove(textureName))
            {
                return;
            }

            textureLocations.Remove(textureName);
        }

        public void Bind(Lighting lighting)
        {
            foreach (KeyValuePair<string, Texture2D> texture in textures)
            {
                Raylib.SetShaderValueTexture(shader, textureLocations[texture.Key], texture.Value);
            }

            TrySetShaderV(
                viewPosLoc, Application.Instance!.Camera3D.Position, ShaderUniformDataType.Vec3
            );
            TrySetShaderV(
                tintLoc, new Vector4(tint.R / 255f, tint.G / 255f, tint.B / 255f, tint.A / 255f),
                ShaderUniformDataType.Vec4
            );
            TrySetShaderV(ambientOcclusionLoc, ambientOcclusion, ShaderUniformDataType.Float);
            TrySetShaderV(metallicLoc, metallic, ShaderUniformDataType.Float);
            TrySetShaderV(roughnessLoc, roughness, ShaderUniformDataType.Float);

            lighting.SetShaderValues(shader);
        }

        private void TrySetShaderV<T>(int loc, T value, ShaderUniformDataType dataType) where T : unmanaged
        {
            if (loc == -1)
            {
                return;
            }

            Raylib.SetShaderValue(shader, loc, value, dataType);
        }
    }
}