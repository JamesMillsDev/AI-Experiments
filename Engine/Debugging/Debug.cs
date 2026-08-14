using Engine.Debugging.Editors;

namespace Engine.Debugging
{
    public static class Debug
    {
        internal static bool Enabled { get; set; }

        private static readonly List<EditorBase> editors = [];

        public static void RegisterEditor(EditorBase editor) => editors.Add(editor);
        public static void UnregisterEditor(EditorBase editor) => editors.Remove(editor);

        internal static void RenderEditors()
        {
            if (!Enabled)
            {
                return;
            }

            foreach (EditorBase editor in editors)
            {
                editor.Render();
            }
        }
    }
}