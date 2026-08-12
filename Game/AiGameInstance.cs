using System.Numerics;
using Engine.Gameplay;
using Engine.Gameplay.Actors;
using Engine.Gameplay.Components;
using Raylib_cs;

namespace Game
{
    public class AiGameInstance : GameInstance
    {
        private Actor? testActor;

        public override void Init()
        {
            window!.ClearColor = Color.Black;

            ActorBuilder builder = new();
            builder
                .WithComponent(new MeshComponent("Content/shaderBall.fbx"));

            testActor = World.SpawnActor(builder);
        }

        public override void Shutdown()
        {
            World.DestroyActor(testActor!);
        }
    }
}