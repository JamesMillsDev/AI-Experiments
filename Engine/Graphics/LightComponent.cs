using System.Numerics;
using Engine.Debugging;
using Engine.Debugging.Editors;
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

    public class LightComponent(LightType type = LightType.Directional, float intensity = 1f, float radius = 1f)
        : Component
    {
        public LightType Type { get; set; } = type;
        public float Intensity { get; set; } = intensity;
        public Color Color { get; set; } = Color.White;
        public float Radius { get; set; } = radius;

        private LightComponentEditor? editor;

        public override void BeginPlay()
        {
            editor = new LightComponentEditor(this);
            Debug.RegisterEditor(editor);

            Owner.World.Lighting.AddLight(this);
        }

        public override void EndPlay()
        {
            Debug.UnregisterEditor(editor!);
            Owner.World.Lighting.RemoveLight(this);
        }

        internal void SetShaderValues(Shader shader, int lightIndex)
        {
            int typeIndex = Raylib.GetShaderLocation(shader, $"lights[{lightIndex}].type");
            int intensityIndex = Raylib.GetShaderLocation(shader, $"lights[{lightIndex}].intensity");
            int colorIndex = Raylib.GetShaderLocation(shader, $"lights[{lightIndex}].color");
            int directionIndex = Raylib.GetShaderLocation(shader, $"lights[{lightIndex}].direction");
            int locationIndex = Raylib.GetShaderLocation(shader, $"lights[{lightIndex}].location");
            int radiusIndex = Raylib.GetShaderLocation(shader, $"lights[{lightIndex}].radius");

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

            if (radiusIndex != -1)
            {
                Raylib.SetShaderValue(shader, radiusIndex, Radius, ShaderUniformDataType.Float);
            }
        }
    }
}