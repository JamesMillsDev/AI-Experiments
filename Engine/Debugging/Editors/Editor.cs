namespace Engine.Debugging.Editors
{
    public abstract class Editor<T>(T context) : EditorBase
    {
        protected T context = context;
    }
}