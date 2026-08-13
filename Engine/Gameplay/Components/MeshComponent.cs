using System.Numerics;
using Engine.Extensions;
using Engine.Graphics;
using Raylib_cs;
using Color = Raylib_cs.Color;
using Material = Engine.Graphics.Material;

namespace Engine.Gameplay.Components
{
    public class MeshComponent(string meshName, string? shaderName = null, float scaleModifier = .01f) : Component
    {
        private readonly List<Model> models = [];
        private readonly List<Material> materials = [];

        public override unsafe void BeginPlay()
        {
            List<Mesh> meshes = MeshLoader.CreateFromAssimp(meshName);
            foreach (Mesh mesh in meshes)
            {
                models.Add(Raylib.LoadModelFromMesh(mesh));
            }

            if (shaderName == null)
            {
                return;
            }

            for (int i = 0; i < models.Count; i++)
            {
                materials.Add(new Material(shaderName));

                models[i].Materials[0].Shader = materials[i].shader;
            }
        }

        public override void EndPlay()
        {
            foreach (Model model in models)
            {
                Raylib.UnloadModel(model);
            }
        }

        public override void Render()
        {
            Owner.Transform.rotation.ToAxisAngle(out Vector3 axis, out float angle);

            for (int i = 0; i < models.Count; i++)
            {
                if (materials.Count != 0)
                {
                    materials[i].Bind(Owner.World.Lighting);
                }

                Raylib.DrawModelEx(
                    models[i], Owner.Transform.location, axis, Mathf.Degrees(angle),
                    Owner.Transform.scale * scaleModifier, Color.White
                );
            }
        }
    }
}