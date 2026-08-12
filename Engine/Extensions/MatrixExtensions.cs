using System.Numerics;

namespace Engine.Extensions
{
    public static class MatrixExtensions
    {
        public static Vector3 Scale(this Matrix4x4 matrix)
        {
            return new Vector3(
                new Vector3(matrix.M11, matrix.M12, matrix.M13).Length(),
                new Vector3(matrix.M21, matrix.M22, matrix.M23).Length(),
                new Vector3(matrix.M31, matrix.M32, matrix.M33).Length()
            );
        }

        public static Vector3 EulerAngles(this Matrix4x4 matrix)
        {
            return Quaternion.CreateFromRotationMatrix(matrix).ToEuler();
        }
    }
}