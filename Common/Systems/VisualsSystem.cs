using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperSpecialWarpinatorTool.Common.UI;
using SuperSpecialWarpinatorTool.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace SuperSpecialWarpinatorTool.Common.Systems
{
    public class VisualsSystem : ModSystem
    {
        public override void Load()
        {
            On_Main.DrawInterface_36_Cursor += DrawCursor;
            On_Main.UpdateUIStates += UpdateUI;
            On_Main.DrawInterface_1_2_DrawEntityMarkersInWorld += DrawIndicators;
            On_Player.HandleHotbar += StopHotbarHandling;
        }

        private void DrawIndicators(On_Main.orig_DrawInterface_1_2_DrawEntityMarkersInWorld orig)
        {
            orig();

            Rectangle hitBox = WarpUtils.GetHoveredHitbox(Main.MouseWorld);

            if (WarpUI.UISettings.Hitboxes == OptionEnum.All)
            {
                foreach (Projectile projectile in Main.projectile.Where(n => n.active))
                    WarpUtils.DrawRectangleIndicator(Main.spriteBatch, projectile.Hitbox, WarpUI.UISettings.CursorMouseColor);

                foreach (NPC nPC in Main.npc.Where(n => n.active))
                    WarpUtils.DrawRectangleIndicator(Main.spriteBatch, nPC.Hitbox, WarpUI.UISettings.CursorMouseColor);
            }
            else if (WarpUI.UISettings.Hitboxes == OptionEnum.Hovered && hitBox.Size().Length() > 2)
                WarpUtils.DrawRectangleIndicator(Main.spriteBatch, hitBox, WarpUI.UISettings.CursorMouseColor);
        }

        private void StopHotbarHandling(On_Player.orig_HandleHotbar orig, Player self)
        {
            if (updateHotbar)
                orig(self);
        }

        private void UpdateUI(On_Main.orig_UpdateUIStates orig, GameTime gameTime)
        {
            if (Main.gameMenu)
            {
                orig(gameTime);
                return;
            }    

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

            Vector2 handPos = Main.LocalPlayer.RotatedRelativePointOld(Main.LocalPlayer.GetBackHandPosition(Player.CompositeArmStretchAmount.Full, Main.LocalPlayer.compositeBackArm.rotation)).ToScreenPosition();

            if (specialCursorRope == null)
                specialCursorRope = new Rope(handPos, Main.MouseScreen, 30, 10f, Vector2.Zero, 0.04f, 5);

            float newDistance = specialCursorRope.StartPos.Distance(specialCursorRope.EndPos) * 0.05f;
            distance = Math.Max(MathHelper.Lerp(distance, newDistance, 0.1f), newDistance);
            specialCursorRope.StartPos = handPos;
            specialCursorRope.EndPos = Main.MouseScreen;
            specialCursorRope.segmentLength = distance;
            specialCursorRope.gravity = Vector2.UnitY * 0.05f;

            if (Main.hasFocus)
            {
                specialCursor = false;
                specialCursorRope.Update();
            }
        }

        private static bool cursorOverride;
        private static bool drawCursor;

        private static bool hotbarUpdateOverride;
        private static bool updateHotbar;
        
        private static bool specialCursor;
        private static Rope specialCursorRope;
        private static float distance;

        public static void HideCursor() => cursorOverride = true;

        public static void UseSpecialCursor() => specialCursor = true;

        public static void StopHotbar() => hotbarUpdateOverride = true;

        private void DrawCursor(On_Main.orig_DrawInterface_36_Cursor orig)
        {
            float thicknessFix = 1.3f / Main.UIScale * Main.GameZoomTarget;

            if (drawCursor)
            {
                if (specialCursor && !Main.mapFullscreen)
                {
                    Texture2D texture = AssetDirectory.Textures.SpecialCursor;
                    Rectangle colorFrame = texture.Frame(2, 1, 0);
                    Rectangle highlightFrame = texture.Frame(2, 1, 1);

                    Color cursorColor = WarpUI.UISettings.CursorMouseColor ? (Main.MouseBorderColor == Color.Transparent ? Main.mouseColor : Main.MouseBorderColor) : WarpUtils.WarpColor();

                    Main.spriteBatch.Draw(texture, Main.MouseScreen, colorFrame, cursorColor, 0f, colorFrame.Size() * 0.5f, 1f, 0, 0);

                    if (specialCursorRope != null && WarpUI.UISettings.CursorWires)
                    {
                        Texture2D line = TextureAssets.BlackTile.Value;

                        List<Vector2> points = new BezierCurve(specialCursorRope.GetPoints()).GetPoints(8 + (int)distance);
                        points.Add(Main.MouseScreen);

                        for (int i = 0; i < points.Count - 1; i++)
                        {
                            Vector2 length = new Vector2(MathHelper.Lerp(thicknessFix, 1.3f, Utils.GetLerpValue(0f, points.Count * 0.5f, i, true)), points[i].Distance(points[i + 1]) / 2f);
                            float rotation = points[i + 1].AngleTo(points[i]) + MathHelper.PiOver2;
                            Color lerpColor = Color.Lerp(WarpUtils.WarpColor(), cursorColor, Utils.GetLerpValue(0.1f, 0.8f, (float)i / (points.Count - 1), true));
                            Main.spriteBatch.Draw(line, points[i], new Rectangle(0, 0, 2, 2), lerpColor, rotation, Vector2.UnitX, length, 0, 0);
                        }
                    }

                    Color ifCustomEnabled = Main.mouseColor;//Main.MouseBorderColor == new Color(255, 255, 255, Main.MouseBorderColor.A) || cursorColor == new Color(255, 255, 255, cursorColor.A) ? WarpUtils.WarpColor() : Color.White;
                    Color cursorHighColor = WarpUI.UISettings.CursorMouseColor ? ifCustomEnabled : Color.White;

                    Main.spriteBatch.Draw(texture, Main.MouseScreen, highlightFrame, cursorHighColor, 0f, highlightFrame.Size() * 0.5f, 1f, 0, 0);
                }
                else
                    orig();
            }
        }
    }
}
