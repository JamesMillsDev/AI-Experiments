

using Engine.Graphics;
using raygui_cs;
using Raylib_cs;

namespace Engine.Debugging.Editors
{
    public class LightComponentEditor(LightComponent context) : Editor<LightComponent>(context)
    {
        public override void Render()
        {
            context.Color = Raygui.GuiColorPicker(new Rectangle(0, 0, 150, 150), "Color", context.Color);
        }
    }
}