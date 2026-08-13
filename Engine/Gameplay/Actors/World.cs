using Engine.Gameplay.Components;
using Engine.Graphics;

namespace Engine.Gameplay.Actors
{
    public class World
    {
        public Lighting Lighting { get; } = new();

        private readonly List<Actor> actors = [];

        public Actor SpawnActor(ref ActorBuilder builder)
        {
            Actor actor = builder.Build(this);
            foreach (Component component in actor.components)
            {
                component.BeginPlay();
            }

            actors.Add(actor);
            builder.Reset();

            return actor;
        }

        public void DestroyActor(Actor actor)
        {
            foreach (Component component in actor.components)
            {
                component.EndPlay();
            }

            actors.Remove(actor);
        }

        internal void TickActors(float dt)
        {
            foreach (Actor actor in actors)
            {
                actor.Tick(dt);
            }
        }

        internal void RenderActors()
        {
            foreach (Actor actor in actors)
            {
                actor.Render();
            }
        }
    }
}