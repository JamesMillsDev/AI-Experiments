using System.Numerics;
using Engine.Extensions;
using Engine.Gameplay;
using Engine.Gameplay.Actors;
using Engine.Gameplay.Components;
using Engine.Graphics;
using Raylib_cs;

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

            ActorBuilder builder = new();
            builder
                .WithComponent(new MeshComponent("Content/Models/shaderBall.fbx", "lit"))
                .WithScale(new Vector3(0.5f));
            shaderBallActor = World.SpawnActor(ref builder);

            builder
                .WithComponent(new LightComponent())
                .WithComponent(new MeshComponent("Content/Models/SM_Sphere.fbx"))
                .WithLocation(new Vector3(1.2f, 1.0f, 2.0f))
                .WithScale(new Vector3(0.1f));
            lightActor = World.SpawnActor(ref builder);
        }

        public override void Render()
        {
            Raylib.DrawGrid(50, 1);
        }

        public override void Tick(float dt)
        {
            angle += dt * 90f;
            shaderBallActor.Transform.rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, Mathf.Radians(angle));
        }

        public override void Shutdown()
        {
            World.DestroyActor(lightActor);
            World.DestroyActor(shaderBallActor);
        }
    }
}