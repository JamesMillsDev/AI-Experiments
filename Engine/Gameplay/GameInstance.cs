using Engine.Core;
using Engine.Gameplay.Actors;

namespace Engine.Gameplay
{
    public abstract class GameInstance
    {
        protected Window? window = null;
        public World World { get; } = new();

        public abstract void Init();

        public abstract void Shutdown();

        public virtual void Tick(float dt)
        {
        }

        public virtual void Render()
        {
        }

        internal void SetWindow(Window win)
        {
            this.window = win;
        }
    }
}