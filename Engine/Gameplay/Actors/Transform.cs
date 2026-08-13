using System.Collections;
using System.Numerics;

namespace Engine.Gameplay.Actors
{
    public class Transform : IEnumerable<Transform>
    {
        public Transform? Parent
        {
            get => parent;
            set
            {
                delayedChildListUpdates.Add(
                    new Tuple<Transform?, Action<Transform?>>(value, newParent =>
                    {
                        parent?.children.Remove(this);
                        parent = newParent;
                        newParent?.children.Add(this);
                    })
                );
            }
        }

        public Vector3 Forward => Vector3.Transform(Vector3.UnitZ, rotation);

        internal readonly List<Tuple<Transform?, Action<Transform?>>> delayedChildListUpdates = [];

        private readonly List<Transform> children = [];
        private Transform? parent;

        public Vector3 location = Vector3.Zero;
        public Quaternion rotation = Quaternion.Identity;
        public Vector3 scale = Vector3.One;

        internal Transform()
        {
        }

        public IEnumerator<Transform> GetEnumerator() => children.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}