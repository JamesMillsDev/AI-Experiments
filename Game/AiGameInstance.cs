using System.Numerics;
using Engine.Extensions;
using Engine.Gameplay;
using Engine.Gameplay.Actors;
using Engine.Gameplay.Components;
using Engine.Graphics;
using Raylib_cs;
using Material = Engine.Graphics.Material;

namespace Game
{
    public class AiGameInstance : GameInstance
    {
#pragma warning disable CS8618
        private Actor shaderBallActor;
        private Actor lightActor;
#pragma warning restore CS8618
        private float angle;

        public override void Init()
        {
            window!.ClearColor = Color.Black;

            Material material = new("lit");
            material.SetTexture("baseColor", Raylib.LoadTexture("Content/Textures/T_RebarConcrete_BC.png"));
            material.SetTexture("normalMap", Raylib.LoadTexture("Content/Textures/T_RebarConcrete_N.png"));
            material.SetTexture("orm", Raylib.LoadTexture("Content/Textures/T_RebarConcrete_ORM.png"));

            ActorBuilder builder = new();
            builder
                .WithComponent(new MeshComponent("Content/Models/shaderBall.fbx", material: material))
                .WithScale(new Vector3(0.5f));
            shaderBallActor = World.SpawnActor(ref builder);

            builder
                .WithComponent(new LightComponent(type: LightType.Point))
                .WithComponent(new MeshComponent(mesh: Raylib.GenMeshSphere(7.5f, 32, 16)));
            lightActor = World.SpawnActor(ref builder);
        }

        public override void Render()
        {
            Raylib.DrawGrid(50, 1);
        }

        public override void Tick(float dt)
        {
            angle += dt * 90f;

            // Make the light orbit the centre of the world
            lightActor.Transform.location =
                Vector3.Transform(
                    Vector3.UnitZ * 5, Quaternion.CreateFromAxisAngle(Vector3.UnitY, Mathf.Radians(angle))
                    ) + new Vector3(0.0f, 1.0f, 0.0f);
        }

        public override void Shutdown()
        {
            World.DestroyActor(lightActor);
            World.DestroyActor(shaderBallActor);
        }
    }
}