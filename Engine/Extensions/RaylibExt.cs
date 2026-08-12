using System.Numerics;
using Raylib_cs;

namespace Engine.Extensions
{
    public static class RaylibExt
    {
        public static void DrawRectangle(Vector3 location, Vector3 size, Color color)
        {
            Raylib.DrawRectangleV(
                new Vector2(location.X, location.Y), new Vector2(size.X, size.Y), color
            );
        }
    }
}