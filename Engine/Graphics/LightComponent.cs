using System.Numerics;
using Engine.Gameplay.Components;
using Raylib_cs;

namespace Engine.Graphics
{
    public enum LightType
    {
        Directional,
        Point,
        Spot
    }

    public class LightComponent : Component
    {
        public LightType Type { get; set; } = LightType.Directional;
        public float Intensity { get; set; } = 1f;
        public Color Color { get; set; } = Color.White;

        public override void BeginPlay()
        {
            Owner.World.Lighting.AddLight(this);
        }

        public override void EndPlay()
        {
            Owner.World.Lighting.RemoveLight(this);
        }

        internal void SetShaderValues(Shader shader, int lightIndex)
        {
            int typeIndex = Raylib.GetShaderLocation(shader, $"lights[{lightIndex}].type");
            int intensityIndex = Raylib.GetShaderLocation(shader, $"lights[{lightIndex}].intensity");
            int colorIndex = Raylib.GetShaderLocation(shader, $"lights[{lightIndex}].color");
            int directionIndex = Raylib.GetShaderLocation(shader, $"lights[{lightIndex}].direction");
            int locationIndex = Raylib.GetShaderLocation(shader, $"lights[{lightIndex}].location");

            if (typeIndex != -1)
            {
                Raylib.SetShaderValue(shader, typeIndex, (int)Type, ShaderUniformDataType.Int);
            }

            if (intensityIndex != -1)
            {
                Raylib.SetShaderValue(shader, intensityIndex, Intensity, ShaderUniformDataType.Float);
            }

            if (colorIndex != -1)
            {
                Raylib.SetShaderValue(
                    shader, colorIndex, new Vector3(Color.R / 255f, Color.G / 255f, Color.B / 255f),
                    ShaderUniformDataType.Vec3
                );
            }

            if (directionIndex != -1)
            {
                Raylib.SetShaderValue(shader, directionIndex, Owner.Transform.Forward, ShaderUniformDataType.Vec3);
            }

            if (locationIndex != -1)
            {
                Raylib.SetShaderValue(shader, locationIndex, Owner.Transform.location, ShaderUniformDataType.Vec3);
            }
        }
    }
}