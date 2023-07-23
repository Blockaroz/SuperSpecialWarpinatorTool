﻿using Microsoft.Xna.Framework;
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
        public static Color WarpColor(byte a = 255) => new Color(255, 25, 255, a);

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

        public static void DrawRectangleIndicator(SpriteBatch spritebatch, Rectangle rectangle)
        {
            Texture2D texture = AssetDirectory.Textures_UI.HitboxIndicator;

            Vector2 right = new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height / 2 + (int)(MathF.Sin(Main.GlobalTimeWrappedHourly * 30) * 6));
            spritebatch.Draw(texture, right, texture.Frame(), Color.White, 0, texture.Size() * 0.5f, 1f, 0, 0);            
            
            Vector2 left = new Vector2(rectangle.X, rectangle.Y + rectangle.Height / 2 + (int)(MathF.Sin(Main.GlobalTimeWrappedHourly * 30) * 6));
            spritebatch.Draw(texture, left, texture.Frame(), Color.White, 0, texture.Size() * 0.5f, 1f, SpriteEffects.FlipHorizontally, 0);            
            
            Vector2 top = new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height / 2 + (int)(MathF.Sin(Main.GlobalTimeWrappedHourly * 30) * 6));
            spritebatch.Draw(texture, top, texture.Frame(), Color.White, -MathHelper.PiOver2, texture.Size() * 0.5f, 1f, 0, 0);            
            
            Vector2 bottom = new Vector2(rectangle.X, rectangle.Y + rectangle.Height / 2 + (int)(MathF.Sin(Main.GlobalTimeWrappedHourly * 30) * 6));
            spritebatch.Draw(texture, bottom, texture.Frame(), Color.White, -MathHelper.PiOver2, texture.Size() * 0.5f, 1f, SpriteEffects.FlipHorizontally, 0);
        }
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
