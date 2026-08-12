using Engine.Core;

namespace Game
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Application.Run<AiGameInstance>(1080, 720, "AI Experiments");
        }
    }
}