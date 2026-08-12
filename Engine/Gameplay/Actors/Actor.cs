using Engine.Gameplay.Components;

namespace Engine.Gameplay.Actors
{
    public class Actor
    {
        public Transform Transform { get; } = new();

        internal readonly List<Component> components = [];

        internal Actor()
        {
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