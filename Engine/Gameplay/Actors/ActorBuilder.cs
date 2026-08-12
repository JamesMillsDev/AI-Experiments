using System.Numerics;
using Engine.Gameplay.Components;

namespace Engine.Gameplay.Actors
{
    public class ActorBuilder
    {
        private readonly List<Component> components = [];
        private Vector3 initialLocation;
        private Quaternion initialRotation;
        private Vector3 initialScale;
        private Transform? initialParent;

        public ActorBuilder WithComponent(Component component)
        {
            components.Add(component);
            return this;
        }

        public ActorBuilder WithLocation(Vector3 location)
        {
            initialLocation = location;
            return this;
        }

        public ActorBuilder WithRotation(Quaternion rotation)
        {
            initialRotation = rotation;
            return this;
        }

        public ActorBuilder WithScale(Vector3 scale)
        {
            initialScale = scale;
            return this;
        }

        public ActorBuilder WithParent(Transform parent)
        {
            initialParent = parent;
            return this;
        }

        public void Reset()
        {
            components.Clear();
        }

        internal Actor Build()
        {
            Actor actor = new Actor();
            foreach (Component component in components)
            {
                component.Owner = actor;
            }

            actor.components.AddRange(components);
            actor.Transform.Location = initialLocation;
            actor.Transform.Rotation = initialRotation;
            actor.Transform.Scale = initialScale;
            actor.Transform.Parent = initialParent;
            return actor;
        }
    }
}