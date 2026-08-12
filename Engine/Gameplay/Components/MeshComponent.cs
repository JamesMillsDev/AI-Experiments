using System.Numerics;
using Engine.Extensions;
using Engine.Graphics;
using Raylib_cs;
using Color = Raylib_cs.Color;
using Transform = Engine.Gameplay.Actors.Transform;

namespace Engine.Gameplay.Components
{
    public class MeshComponent(string meshName) : Component
    {
        private readonly List<Model> models = [];

        public override void BeginPlay()
        {
            List<Mesh> meshes = MeshLoader.CreateFromAssimp(meshName);
            foreach (Mesh mesh in meshes)
            {
                models.Add(Raylib.LoadModelFromMesh(mesh));
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
            Transform ownerTransform = Owner.Transform;
            ownerTransform.Rotation.ToAxisAngle(out Vector3 axis, out float angle);

            foreach (Model model in models)
            {
                Raylib.DrawModelEx(model, ownerTransform.Location, axis, angle, ownerTransform.Scale, Color.White);
            }
        }
    }
}