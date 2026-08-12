using System.Collections;
using System.Numerics;
using Engine.Extensions;

namespace Engine.Gameplay.Actors
{
    public class Transform : IEnumerable<Transform>
    {
        public Vector3 Location
        {
            get => Global.Translation;
            set => local.Translation = value;
        }

        public Quaternion Rotation
        {
            get => Quaternion.CreateFromRotationMatrix(Global);
            set
            {
                Vector3 translation = local.Translation;
                Vector3 scale = local.Scale();

                local = Matrix4x4.CreateTranslation(translation) *
                        Matrix4x4.CreateFromQuaternion(value) *
                        Matrix4x4.CreateScale(scale);
            }
        }

        public Vector3 EulerAngles
        {
            get => Global.EulerAngles();
            set
            {
                Vector3 translation = local.Translation;
                Vector3 scale = local.Scale();

                local = Matrix4x4.CreateTranslation(translation) *
                            Matrix4x4.CreateFromYawPitchRoll(value.X, value.Y, value.Z) *
                            Matrix4x4.CreateScale(scale);
            }
        }

        public Vector3 Scale
        {
            get => Global.Scale();
            set
            {
                Vector3 translation = local.Translation;
                Quaternion rotation = Quaternion.CreateFromRotationMatrix(local);

                local = Matrix4x4.CreateTranslation(translation) *
                            Matrix4x4.CreateFromQuaternion(rotation) *
                            Matrix4x4.CreateScale(value);
            }
        }

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

        private Matrix4x4 Global => parent != null ? parent.local * local : local;

        internal List<Tuple<Transform?, Action<Transform?>>> delayedChildListUpdates = [];

        private Matrix4x4 local = Matrix4x4.Identity;
        private readonly List<Transform> children = [];
        private Transform? parent;

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