using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace SuperSpecialWarpinatorTool.Common.Systems
{
    public class WarpinatorSystem : ModSystem
    {
        public override void Load()
        {
            On_Main.DrawInterface_36_Cursor += DrawCursor;
            On_Main.UpdateUIStates += UpdateUI;
        }

        private void UpdateUI(On_Main.orig_UpdateUIStates orig, GameTime gameTime)
        {
            if (cursorOverride)
            {
                drawCursor = false;
                cursorOverride = false;
            }
            else
                drawCursor = true;

            orig(gameTime);
        }

        private static bool cursorOverride;
        private static bool drawCursor;

        public static void HideCursor() => cursorOverride = true;

        private void DrawCursor(On_Main.orig_DrawInterface_36_Cursor orig)
        {
            if (drawCursor)
                orig();
        }
    }
}
