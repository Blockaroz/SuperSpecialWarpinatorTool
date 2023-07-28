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

            Vector2 handPos = Main.LocalPlayer.RotatedRelativePoint(Main.LocalPlayer.GetBackHandPosition(Player.CompositeArmStretchAmount.Full, Main.LocalPlayer.compositeBackArm.rotation)).ToScreenPosition();

            if (popitRope == null)
                popitRope = new Rope(handPos, Main.MouseScreen, 30, 10f, Vector2.Zero, 0.04f, 5);

            float newDistance = popitRope.StartPos.Distance(popitRope.EndPos) * 0.05f;
            distance = Math.Max(MathHelper.Lerp(distance, newDistance, 0.1f), newDistance);
            popitRope.StartPos = handPos;
            popitRope.EndPos = Main.MouseScreen;
            popitRope.segmentLength = distance;
            popitRope.gravity = Vector2.UnitY * 0.05f;
            popitRope.Update();

            specialCursorWireHands = false;
            specialCursor = false;
        }

        private static bool cursorOverride;
        private static bool drawCursor;

        private static bool hotbarUpdateOverride;
        private static bool updateHotbar;
        
        private static bool specialCursorWireHands;
        private static bool specialCursor;
        private static Rope popitRope;
        private static float distance;

        public static void HideCursor() => cursorOverride = true;

        public static void UseSpecialCursorWireHands() => specialCursorWireHands = true;
        public static void UseSpecialCursor()
        {
            specialCursor = true;
            specialCursorWireHands = true;
        }

        public static void StopHotbar() => hotbarUpdateOverride = true;

        private void DrawCursor(On_Main.orig_DrawInterface_36_Cursor orig)
        {
            Texture2D line = TextureAssets.BlackTile.Value;

            float thicknessFix = 1.3f / Main.UIScale * Main.GameZoomTarget;

            if (specialCursor || specialCursorWireHands)
            {
                Vector2 backHandPos = Main.LocalPlayer.RotatedRelativePoint(Main.LocalPlayer.GetBackHandPosition(Player.CompositeArmStretchAmount.Full, Main.LocalPlayer.compositeBackArm.rotation)).ToScreenPosition();
                Vector2 frontHandPos = Main.LocalPlayer.RotatedRelativePoint(Main.LocalPlayer.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, Main.LocalPlayer.compositeFrontArm.rotation)).ToScreenPosition();
                Vector2 interHandLength = new Vector2(thicknessFix, backHandPos.Distance(frontHandPos) / 1.33f);
                Main.spriteBatch.Draw(line, backHandPos, new Rectangle(0, 0, 2, 1), WarpUtils.WarpColor(), backHandPos.AngleTo(frontHandPos) - MathHelper.PiOver2, Vector2.UnitX, interHandLength, 0, 0);
            }
            if (drawCursor)
            {
                if (specialCursor)
                {
                    Texture2D texture = AssetDirectory.Textures_UI.SelectorCursor;
                    Rectangle colorFrame = texture.Frame(2, 1, 0);
                    Rectangle highlightFrame = texture.Frame(2, 1, 1);

                    Color cursorColor = WarpUI.UISettings.CursorMouseColor ? (Main.MouseBorderColor == Color.Transparent ? Main.mouseColor : Main.MouseBorderColor) : WarpUtils.WarpColor();

                    Main.spriteBatch.Draw(texture, Main.MouseScreen, colorFrame, cursorColor, 0f, colorFrame.Size() * 0.5f, 1f, 0, 0);

                    if (popitRope != null)
                    {
                        List<Vector2> points = new BezierCurve(popitRope.GetPoints()).GetPoints(8 + (int)(distance));
                        points.Add(Main.MouseScreen);

                        for (int i = 0; i < points.Count - 1; i++)
                        {
                            Vector2 length = new Vector2(MathHelper.Lerp(thicknessFix, 1.3f, Utils.GetLerpValue(0f, points.Count * 0.5f, i, true)), points[i].Distance(points[i + 1]) / 2f);
                            float rotation = points[i + 1].AngleTo(points[i]) + MathHelper.PiOver2;
                            Color lerpColor = Color.Lerp(WarpUtils.WarpColor(), cursorColor, Utils.GetLerpValue(0.1f, 0.8f, (float)i / (points.Count - 1), true));
                            Main.spriteBatch.Draw(line, points[i], new Rectangle(0, 0, 2, 2), lerpColor, rotation, Vector2.UnitX, length, 0, 0);
                        }
                    }

                    Color ifCustomEnabled = Main.MouseBorderColor == new Color(255, 255, 255, Main.MouseBorderColor.A) || cursorColor == new Color(255, 255, 255, cursorColor.A) ? WarpUtils.WarpColor() : Color.White;
                    Color cursorHighColor = WarpUI.UISettings.CursorMouseColor ? ifCustomEnabled : Color.White;

                    Main.spriteBatch.Draw(texture, Main.MouseScreen, highlightFrame, cursorHighColor, 0f, highlightFrame.Size() * 0.5f, 1f, 0, 0);
                }
                else
                    orig();
            }
        }
    }
}
