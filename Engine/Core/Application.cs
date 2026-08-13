using System.Numerics;
using Engine.Gameplay;
using Raylib_cs;

namespace Engine.Core
{
    public class Application
    {
        public static Application? Instance { get; private set; }

        public static void Run<T>(int width, int height, string title) where T : GameInstance, new()
        {
            if (Instance != null)
            {
                throw new InvalidOperationException("Application already initialized");
            }

            Instance = new Application(width, height, title, new T());
            Instance.Run();
        }

        public Camera3D Camera3D => camera;

        private Camera3D camera = new(
            new Vector3(0f, 1f, -10f), new Vector3(0f, 1f, -9f),
            new Vector3(0f, 1f, 0f), 45f, CameraProjection.Perspective
        );

        private readonly Window window;
        private readonly GameInstance gameInstance;

        private Application(int width, int height, string title, GameInstance gameInstance)
        {
            window = new Window(width, height, title);
            this.gameInstance = gameInstance;
            this.gameInstance.SetWindow(window);
        }

        private void Run()
        {
            if (!window.Open())
            {
                throw new InvalidOperationException("Window failed to open!");
            }

            gameInstance.Init();

            while (!Raylib.WindowShouldClose())
            {
                if (Raylib.IsMouseButtonPressed(MouseButton.Right))
                {
                    Raylib.HideCursor();
                    Raylib.DisableCursor();
                }
                else if (Raylib.IsMouseButtonReleased(MouseButton.Right))
                {
                    Raylib.EnableCursor();
                    Raylib.ShowCursor();
                }

                if (Raylib.IsMouseButtonDown(MouseButton.Right))
                {
                    Raylib.UpdateCamera(ref camera, CameraMode.Free);
                }

                gameInstance.Tick(Raylib.GetFrameTime());
                gameInstance.World.TickActors(Raylib.GetFrameTime());

                window.NewFrame(camera);

                gameInstance.Render();
                gameInstance.World.RenderActors();

                window.EndFrame();
            }

            gameInstance.Shutdown();
            window.Close();
        }
    }
}