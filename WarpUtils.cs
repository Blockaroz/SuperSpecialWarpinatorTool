using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperSpecialWarpinatorTool.Common.Systems;
using SuperSpecialWarpinatorTool.Common.UI;
using SuperSpecialWarpinatorTool.Content.Items;
using SuperSpecialWarpinatorTool.Core;
using System;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.PlayerDrawLayer;

namespace SuperSpecialWarpinatorTool
{
    public static class WarpUtils
    {
        public static Color WarpColor(byte a = 255) => new Color(255, 25, 255, a);

        public static WarpinatorPlayer WarpPlayer(this Player player) => player.GetModPlayer<WarpinatorPlayer>();

        public static bool HoldingWarpinator(this Player player) => player.HeldItem.type == WarpinatorID;
        public static bool HasWarpinator(this Player player) => player.HasItem(WarpinatorID);
        public static bool HasAction<T>(this Player Player) where T : WarpinatorAction => Player.WarpPlayer().actions.Any(n => n is T);

        public static void WarpInterface(this Player player, bool condition = true)
        {
            player.mouseInterface = condition;
            WarpinatorUISystem.WarpinatorUI.OnMyInterface = condition;
            VisualsSystem.StopHotbar();
        } 

        public static void DrawPanel(SpriteBatch spriteBatch, int x, int y, int width, int height, Color color) => Utils.DrawSplicedPanel(spriteBatch, AssetDirectory.Textures_UI.WarpPanel[0], x, y, width, height, 10, 10, 10, 10, color);
        public static void DrawThinPanel(SpriteBatch spriteBatch, int x, int y, int width, int height, Color color) => Utils.DrawSplicedPanel(spriteBatch, AssetDirectory.Textures_UI.WarpPanel[1], x, y, width, height, 8, 8, 8, 8, color);


        public static int WarpinatorID => ModContent.ItemType<SuperSpecialWarpinator>();

        public static Vector2 MouseVelocity;
        public static Vector2 MouseScreenOld;
        public static bool MouseShaking;

        public static readonly RasterizerState OverflowHiddenRasterizerState = new RasterizerState
        {
            CullMode = CullMode.None,
            ScissorTestEnable = true
        };

        public static void DrawRectangleIndicator(SpriteBatch spritebatch, Rectangle rectangle, bool cursorColor)
        {
            Texture2D texture = AssetDirectory.Textures_UI.HitboxIndicator[cursorColor ? 1 : 0];
            Color drawColor = cursorColor ? Main.MouseBorderColor : Color.White;

            Vector2 right = new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height / 2 + (int)(MathF.Sin(Main.GlobalTimeWrappedHourly * 30) * 6));
            spritebatch.Draw(texture, right - Main.screenPosition, texture.Frame(), drawColor, 0, texture.Size() * 0.5f, 1f, 0, 0);            
            
            Vector2 left = new Vector2(rectangle.X, rectangle.Y + rectangle.Height / 2 + (int)(MathF.Sin(Main.GlobalTimeWrappedHourly * 30) * 6));
            spritebatch.Draw(texture, left - Main.screenPosition, texture.Frame(), drawColor, 0, texture.Size() * 0.5f, 1f, SpriteEffects.FlipHorizontally, 0);            
            
            Vector2 top = new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height / 2 + (int)(MathF.Sin(Main.GlobalTimeWrappedHourly * 30) * 6));
            spritebatch.Draw(texture, top - Main.screenPosition, texture.Frame(), drawColor, -MathHelper.PiOver2, texture.Size() * 0.5f, 1f, 0, 0);            
            
            Vector2 bottom = new Vector2(rectangle.X, rectangle.Y + rectangle.Height / 2 + (int)(MathF.Sin(Main.GlobalTimeWrappedHourly * 30) * 6));
            spritebatch.Draw(texture, bottom - Main.screenPosition, texture.Frame(), drawColor, -MathHelper.PiOver2, texture.Size() * 0.5f, 1f, SpriteEffects.FlipHorizontally, 0);
        }
    }

    public class MouseShakeSystem : ModSystem
    {
        private float shakeCounter;
        private static Vector2 mouseOlder;

        public override void PostUpdateEverything()
        {
            Vector2 olderVelocity = WarpUtils.MouseScreenOld - mouseOlder;
            WarpUtils.MouseVelocity = Main.MouseScreen - WarpUtils.MouseScreenOld;

            float amt = Vector2.Dot(olderVelocity, WarpUtils.MouseVelocity);
            if (amt < 0)
                shakeCounter += Math.Abs(amt) * 0.1f;

            WarpUtils.MouseShaking = shakeCounter > 60;

            shakeCounter--;
            shakeCounter = MathHelper.Clamp(shakeCounter, 0f, 61f);

            mouseOlder = WarpUtils.MouseScreenOld;
            WarpUtils.MouseScreenOld = Main.MouseScreen;
        }
    }
}
