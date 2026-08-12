using System.Numerics;

namespace Engine.Extensions
{
    public static class QuaternionExtensions
    {
        public static Vector3 ToEuler(this Quaternion q)
        {
            // 1. Calculate values for the conversion matrix elements
            float sqw = q.W * q.W;
            float sqx = q.X * q.X;
            float sqy = q.Y * q.Y;
            float sqz = q.Z * q.Z;

            // Normalization factor if the quaternion is not a unit quaternion
            float unit = sqx + sqy + sqz + sqw;

            // Singularity tracking (Gimbal Lock)
            float test = q.X * q.Y + q.Z * q.W;

            float yaw, pitch, roll;

            // 2. Handle Singularity at North Pole (Gimbal Lock)
            if (test > 0.499f * unit)
            {
                yaw = 2f * MathF.Atan2(q.X, q.W);
                pitch = MathF.PI / 2f;
                roll = 0f;
                return new Vector3(pitch, yaw, roll);
            }

            // 3. Handle Singularity at South Pole (Gimbal Lock)
            if (test < -0.499f * unit)
            {
                yaw = -2f * MathF.Atan2(q.X, q.W);
                pitch = -MathF.PI / 2f;
                roll = 0f;
                return new Vector3(pitch, yaw, roll);
            }

            // 4. Standard Conversion (Tait-Bryan ZYX / YXZ sequence reversal)
            yaw = MathF.Atan2(2f * q.Y * q.W - 2f * q.X * q.Z, sqx - sqy - sqz + sqw);
            pitch = MathF.Asin(2f * test / unit);
            roll = MathF.Atan2(2f * q.X * q.W - 2f * q.Y * q.Z, -sqx + sqy - sqz + sqw);

            // Vector3.X = Pitch, Vector3.Y = Yaw, Vector3.Z = Roll
            return new Vector3(pitch, yaw, roll);
        }

        public static void ToAxisAngle(this Quaternion q, out Vector3 axis, out float angle)
        {
            // Ensure the quaternion is normalized to avoid math errors
            Quaternion qNorm = Quaternion.Normalize(q);

            // Clip W component to stay within valid acos bounds due to floating-point drift
            float w = Math.Clamp(qNorm.W, -1.0f, 1.0f);

            // Calculate the angle (in radians)
            angle = 2.0f * MathF.Acos(w);

            // Calculate the sine magnitude of the half-angle
            float sinHalfAngle = MathF.Sqrt(1.0f - w * w);

            // Avoid division by zero if the angle is close to 0 (identity quaternion)
            if (sinHalfAngle > 0.0001f)
            {
                axis = new Vector3(qNorm.X, qNorm.Y, qNorm.Z) / sinHalfAngle;
            }
            else
            {
                // If angle is 0, the axis can be any normalized vector (defaulting to UnitX)
                axis = Vector3.UnitX;
            }
        }
    }
}