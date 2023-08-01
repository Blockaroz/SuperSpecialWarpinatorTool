using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperSpecialWarpinatorTool.Common.Systems;
using SuperSpecialWarpinatorTool.Common.UI;
using SuperSpecialWarpinatorTool.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;

namespace SuperSpecialWarpinatorTool.Content.MenuElements
{
    public class EntitySelector<T> : IMenuElement where T : Entity
    {
        private Ref<int> selection;
        private T[] values;
        private string selectionData;
        private bool initialized;

        private bool dragging;
        private bool oldDragging;

        private Rope cursorRope;
        private float distance;
        private Vector2 cursorPos;

        private bool oldHover;

        public EntitySelector(Ref<int> selection, ref T[] values)
        {
            this.selection = selection;
            this.values = values;
        }

        public int Width => 144;

        public int Height => 24;

        public void Draw(SpriteBatch spriteBatch, Color color, Player player, Vector2 position, Vector2 mousePos, int direction)
        {
            int boxWidth = Width - 22;
            Vector2 offsetPosition = position + new Vector2(direction < 0 ? -Width : 0, 0);
            WarpUtils.DrawPanel(spriteBatch, (int)offsetPosition.X, (int)offsetPosition.Y, Width, Height, color);
            WarpUtils.DrawPanel(spriteBatch, (int)offsetPosition.X + (direction < 0 ? 22 : 0), (int)offsetPosition.Y, boxWidth, Height, color);

            if (!WarpinatorUISystem.WarpinatorUI.MenuUsable)
                cursorRope = null;

            if (selection.Value > -1)
            {
                if (!initialized)
                {
                    selectionData = "";//values[selection.Value].GetSource_FromThis().Context.ToString();
                    initialized = true;
                }

                RasterizerState priorRasterizer = spriteBatch.GraphicsDevice.RasterizerState;
                Rectangle priorScissor = spriteBatch.GraphicsDevice.ScissorRectangle;

                spriteBatch.End();
                spriteBatch.GraphicsDevice.RasterizerState = WarpUtils.OverflowHiddenRasterizerState;
                spriteBatch.GraphicsDevice.ScissorRectangle = new Rectangle((int)offsetPosition.X + 6, (int)offsetPosition.Y + 6, boxWidth - 6, Height);
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, WarpUtils.OverflowHiddenRasterizerState, null, Main.UIScaleMatrix);

                string text = typeof(T).Name + " " + values[selection.Value].whoAmI + ":" + selectionData;
                Utils.DrawBorderString(spriteBatch, text, offsetPosition + new Vector2(8 + (direction < 0 ? 22 : 0), 6), color, 0.66f);

                spriteBatch.End();
                spriteBatch.GraphicsDevice.RasterizerState = priorRasterizer;
                spriteBatch.GraphicsDevice.ScissorRectangle = priorScissor;
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, WarpUtils.OverflowHiddenRasterizerState, null, Main.UIScaleMatrix);
            }
            else
                initialized = false;

            Vector2 buttonPos = position + new Vector2((Width - 14) * direction, Height / 2);

            Rectangle area = new Rectangle((int)position.X, (int)position.Y, Width, Height);
            bool hovered = area.Contains(mousePos.ToPoint());

            Texture2D buttonTexture = AssetDirectory.Textures_UI.SelectorButton;
            Rectangle buttonFrame = buttonTexture.Frame(2, 2, (dragging || selection.Value > -1) ? 1 : 0, 0);
            Rectangle buttonHoverFrame = buttonTexture.Frame(2, 2, (dragging || selection.Value > -1) ? 1 : 0, 1);

            spriteBatch.Draw(buttonTexture, buttonPos, buttonFrame, color, 0f, buttonFrame.Size() * 0.5f, 1f, 0, 0);
            if (hovered)
                spriteBatch.Draw(buttonTexture, buttonPos, buttonHoverFrame, Main.OurFavoriteColor.MultiplyRGBA(color), 0f, buttonFrame.Size() * 0.5f, 1f, 0, 0);

            if ((dragging || selection.Value > -1))
            {
                if (cursorRope != null)
                {
                    Texture2D texture = AssetDirectory.Textures_UI.SelectorCursor;
                    Rectangle colorFrame = texture.Frame(2, 1, 0);
                    Rectangle highlightFrame = texture.Frame(2, 1, 1);

                    Color cursorColor = WarpUI.UISettings.CursorMouseColor ? (Main.MouseBorderColor == Color.Transparent ? Main.mouseColor : Main.MouseBorderColor) : WarpUtils.WarpColor();

                    Main.spriteBatch.Draw(texture, cursorPos, colorFrame, cursorColor, 0f, colorFrame.Size() * 0.5f, 1f, 0, 0);

                    if (cursorRope != null && WarpUI.UISettings.CursorWires)
                    {
                        Texture2D line = TextureAssets.BlackTile.Value;

                        List<Vector2> points = new BezierCurve(cursorRope.GetPoints()).GetPoints(8 + (int)(distance));
                        points.Add(cursorPos);

                        for (int i = 0; i < points.Count - 1; i++)
                        {
                            Vector2 length = new Vector2(1.3f, points[i].Distance(points[i + 1]) / 2f);
                            float rotation = points[i + 1].AngleTo(points[i]) + MathHelper.PiOver2;
                            Color lerpColor = Color.Lerp(WarpUtils.WarpColor(), cursorColor, Utils.GetLerpValue(0.1f, 0.8f, (float)i / (points.Count - 1), true));
                            Main.spriteBatch.Draw(line, points[i], new Rectangle(0, 0, 2, 2), lerpColor, rotation, Vector2.UnitX, length, 0, 0);
                        }
                    }

                    Color cursorHighColor = WarpUI.UISettings.CursorMouseColor ? Main.mouseColor : Color.White;

                    Main.spriteBatch.Draw(texture, cursorPos, highlightFrame, cursorHighColor, 0f, highlightFrame.Size() * 0.5f, 1f, 0, 0);
                }
            }
        }

        public void Update(Player player, Vector2 position, Vector2 mousePos, int direction)
        {
            Vector2 offsetPosition = position + new Vector2(direction < 0 ? -Width : 0, 0);

            int boxWidth = Width - 22;
            Vector2 buttonPos = position + new Vector2((Width - 14) * direction, Height / 2);

            Rectangle area = new Rectangle((int)offsetPosition.X, (int)offsetPosition.Y, Width, Height);
            bool hovered = area.Contains(mousePos.ToPoint());
            if (hovered && !dragging)
            {
                if (player.WarpPlayer().mouseLeft)
                    dragging = true;

                if (!oldHover)
                    SoundEngine.PlaySound(AssetDirectory.Sounds_UI.MenuTick);
            }

            if (dragging && dragging == oldDragging)
            {
                player.WarpInterface();
                player.WarpPlayer().useSpecialCursorWireHands = true;
                WarpinatorUISystem.WarpinatorUI.DisableOpenAndClose = true;
                VisualsSystem.HideCursor();

                if (player.WarpPlayer().mouseLeft)
                {
                    if (values.Any(n => n.active && n.Hitbox.Contains(Main.MouseWorld.ToPoint())))
                    {
                        selection.Value = values.First(n => n.active && n.Hitbox.Contains(Main.MouseWorld.ToPoint())).whoAmI;
                        dragging = false;

                        SoundEngine.PlaySound(AssetDirectory.Sounds_UI.HookEntity);
                    }
                    else
                    {
                        selection.Value = -1;
                        dragging = false;
                        SoundEngine.PlaySound(AssetDirectory.Sounds_UI.UnhookEntity);
                    }
                }

                if (player.WarpPlayer().mouseRight)
                {
                    selection.Value = -1;
                    dragging = false;
                    SoundEngine.PlaySound(AssetDirectory.Sounds_UI.UnhookEntity);
                }
            }

            if (selection.Value > -1 && !dragging)
            {
                cursorPos = values[selection.Value].Center - Main.screenPosition;
                if (!values[selection.Value].active)
                    selection.Value = -1;
            }
            else
                cursorPos = mousePos;

            if (cursorRope == null)
                cursorRope = new Rope(buttonPos, mousePos, 30, 1f, Vector2.UnitY * 0.5f, 0.05f);

            float newDistance = cursorRope.StartPos.Distance(cursorRope.EndPos) * 0.03f;
            distance = Math.Max(MathHelper.Lerp(distance, newDistance, 0.1f), newDistance);

            cursorRope.StartPos = buttonPos;
            cursorRope.EndPos = cursorPos;
            cursorRope.segmentLength = distance;
            cursorRope.Update();

            oldHover = hovered;
            oldDragging = dragging;
        }
    }
}
