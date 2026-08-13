using System.Numerics;
using Engine.Gameplay.Components;

namespace Engine.Gameplay.Actors
{
    public class ActorBuilder
    {
        private readonly List<Component> components = [];
        private Vector3? initialLocation;
        private Quaternion? initialRotation;
        private Vector3? initialScale;
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
            initialLocation = null;
            initialRotation = null;
            initialScale = null;
        }

        internal Actor Build(World world)
        {
            Actor actor = new(world);
            foreach (Component component in components)
            {
                component.Owner = actor;
            }

            actor.components.AddRange(components);

            if (initialLocation.HasValue)
            {
                actor.Transform.location = initialLocation.Value;
            }

            if (initialRotation.HasValue)
            {
                actor.Transform.rotation = initialRotation.Value;
            }

            if (initialScale.HasValue)
            {
                actor.Transform.scale = initialScale.Value;
            }

            actor.Transform.Parent = initialParent;
            return actor;
        }
    }
}