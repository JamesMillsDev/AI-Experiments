using Raylib_cs;

namespace Engine.Core
{
    public class Window(int width, int height, string title)
    {
        public float Width
        {
            get => width;
            set
            {
                width = (int)value;
                Raylib.SetWindowSize(width, height);
            }
        }

        public float Height
        {
            get => height;
            set
            {
                height = (int)value;
                Raylib.SetWindowSize(width, height);
            }
        }

        public string Title
        {
            get => title;
            set
            {
                title = value;
                Raylib.SetWindowTitle(value);
            }
        }

        public Color ClearColor { get; set; } = Color.White;

        private string title = title;
        private int height = height;
        private int width = width;

        internal bool Open()
        {
            Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
            Raylib.InitWindow(width, height, title);
            return Raylib.IsWindowReady();
        }

        internal void Close()
        {
            Raylib.CloseWindow();
        }

        internal void NewFrame()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(ClearColor);
        }

        internal void EndFrame()
        {
            Raylib.EndDrawing();
        }
    }
}