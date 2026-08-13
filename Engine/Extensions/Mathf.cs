using Raylib_cs;

namespace Engine.Extensions
{
    public static class Mathf
    {
        public static float Radians(float degrees)
        {
            return degrees * Raylib.DEG2RAD;
        }

        public static float Degrees(float radians)
        {
            return radians * Raylib.RAD2DEG;
        }
    }
}