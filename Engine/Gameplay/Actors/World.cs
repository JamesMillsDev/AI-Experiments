using Engine.Gameplay.Components;

namespace Engine.Gameplay.Actors
{
    public class World
    {
        private readonly List<Actor> actors = [];

        public Actor SpawnActor(ActorBuilder builder)
        {
            Actor actor = builder.Build();
            foreach (Component component in actor.components)
            {
                component.BeginPlay();
            }

            actors.Add(actor);
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