using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperSpecialWarpinatorTool.Common.Systems;
using SuperSpecialWarpinatorTool.Common.UI;
using SuperSpecialWarpinatorTool.Content.Items;
using SuperSpecialWarpinatorTool.Core;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace SuperSpecialWarpinatorTool
{
    public static class WarpUtils
    {
        public static Color WarpColor(float x, byte a = 255) => Color.Lerp(new Color(110, 48, 255, a), new Color(255, 25, 174, a), MathHelper.Clamp(x, 0, 1));

        public static WarpinatorPlayer WarpPlayer(this Player player) => player.GetModPlayer<WarpinatorPlayer>();

        public static bool ValidWarpinator(this Player player) => player.HeldItem.type == WarpinatorID;
        public static bool HasWarpinator(this Player player) => player.HasItem(WarpinatorID);

        public static void WarpInterface(this Player player, bool condition = true)
        {
            player.mouseInterface = condition;
            WarpinatorUISystem.WarpinatorUI.OnMyInterface = condition;
        } 

        public static int WarpinatorID => ModContent.ItemType<SuperSpecialWarpinator>();

        public static Vector2 MouseVelocity;
        public static Vector2 MouseScreenOld;

        public static bool shakingMouse;

        public static int CommonOptionHeight = 32;
    }

    public class MouseShakeSystem : ModSystem
    {
        private int shakeCounter;
        private static Vector2 mouseScreenOlder;

        public override void PostUpdateEverything()
        {
            WarpUtils.MouseVelocity = (Main.MouseScreen - WarpUtils.MouseScreenOld);

            Vector2 accel = (WarpUtils.MouseScreenOld - Main.MouseScreen) - (WarpUtils.MouseScreenOld - mouseScreenOlder);

            if (accel.Length() > 70)
                shakeCounter++;
            else
                shakeCounter -= 3;

            shakeCounter = Math.Clamp(shakeCounter, 0, 8);

            mouseScreenOlder = WarpUtils.MouseScreenOld;
            WarpUtils.MouseScreenOld = Main.MouseScreen;
        }
    }
}
