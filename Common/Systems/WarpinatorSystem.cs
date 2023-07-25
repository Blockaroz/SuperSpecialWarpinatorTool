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
            On_Player.HandleHotbar += StopHotbarHandling;
        }

        private void StopHotbarHandling(On_Player.orig_HandleHotbar orig, Player self)
        {
            if (updateHotbar)
                orig(self);
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

            if (hotbarUpdateOverride)
            {
                updateHotbar = false;
                hotbarUpdateOverride = false;
            }
            else
                updateHotbar = true;

            orig(gameTime);
        }

        private static bool cursorOverride;
        private static bool drawCursor;

        private static bool hotbarUpdateOverride;
        private static bool updateHotbar;

        public static void HideCursor() => cursorOverride = true;

        public static void StopHotbar() => hotbarUpdateOverride = true;

        private void DrawCursor(On_Main.orig_DrawInterface_36_Cursor orig)
        {
            if (drawCursor)
                orig();
        }
    }
}
