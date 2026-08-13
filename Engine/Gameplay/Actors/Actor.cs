using Engine.Gameplay.Components;

namespace Engine.Gameplay.Actors
{
    public class Actor
    {
        public Transform Transform { get; } = new();
        public World World { get; }

        internal readonly List<Component> components = [];

        public T? FindComponent<T>() where T : Component
        {
            foreach (Component component in components)
            {
                if (component is T t)
                {
                    return t;
                }
            }

            return null;
        }

        internal Actor(World world)
        {
            World = world;
        }

        internal void Tick(float dt)
        {
            foreach (Tuple<Transform?, Action<Transform?>> update in Transform.delayedChildListUpdates)
            {
                update.Item2(update.Item1);
            }

            foreach (Component component in components)
            {
                component.Tick(dt);
            }
        }

        internal void Render()
        {
            foreach (Component component in components)
            {
                component.Render();
            }
        }
    }
}