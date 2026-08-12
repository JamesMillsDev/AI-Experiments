using Engine.Gameplay.Actors;

namespace Engine.Gameplay.Components
{
    public class Component
    {
        public Actor Owner { get; internal set; } = null!;

        public virtual void BeginPlay()
        {
        }

        public virtual void Tick(float dt)
        {
        }

        public virtual void Render()
        {
        }

        public virtual void EndPlay()
        {
        }
    }
}