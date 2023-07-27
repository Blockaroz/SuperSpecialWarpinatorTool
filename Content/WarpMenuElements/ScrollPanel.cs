using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperSpecialWarpinatorTool.Core;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;

namespace SuperSpecialWarpinatorTool.Content.WarpMenuElements
{
    public class ScrollPanel : IWarpMenuElement
    {
        private Ref<List<string>> elements;
        private Ref<int> selection;
        private int width;
        private int height;
        private bool dragging;
        private bool canSelect;
        private int oldHover;
        private int hovered;

        private float scrollAmount;
        private bool scrollOldHover;
        private bool scrollHovered;
        private bool needsScrollBar;
        private int scrollOffset;

        public ScrollPanel(Ref<List<string>> elements, Ref<int> selection, int width, int height, bool canSelect = true)
        {
            this.elements = elements;
            this.selection = selection;
            this.width = width;
            this.height = height;
            this.canSelect = canSelect;
            needsScrollBar = elements.Value.Count * 20 > height;
        }

        public int Height => height + 16;

        public void Draw(SpriteBatch spriteBatch, Color color, Player player, Vector2 position, Vector2 mousePos, int direction)
        {
            int scrollBarOffset = needsScrollBar ? 24 : 0;
            Utils.DrawSplicedPanel(spriteBatch, AssetDirectory.Textures_UI.WarpPanel[0], (int)(position.X - (direction < 0 ? width + scrollBarOffset : 0)), (int)position.Y, width + scrollBarOffset, height + 12, 10, 10, 10, 10, color);

            if (needsScrollBar)
            {
                int scrollX = direction < 0 ? -width - scrollBarOffset + 4 : width + scrollBarOffset - 16;
                Utils.DrawSplicedPanel(spriteBatch, AssetDirectory.Textures_UI.WarpPanel[1], (int)(position.X + scrollX), (int)position.Y + 4, 12, height + 4, 4, 4, 4, 4, color);

                int scrollHeight = (int)((float)(height / 20f) * elements.Value.Count);
                Color scrollColor = scrollHovered ? Color.White : Color.White * 0.85f;
                Utils.DrawSplicedPanel(spriteBatch, AssetDirectory.Textures_UI.ScrollButton, (int)(position.X + scrollX), (int)(position.Y + 4 + scrollAmount * height), 12, scrollHeight, 4, 4, 4, 4, scrollColor.MultiplyRGBA(color));
            }

            RasterizerState priorRasterizer = spriteBatch.GraphicsDevice.RasterizerState;
            Rectangle priorScissor = spriteBatch.GraphicsDevice.ScissorRectangle;

            spriteBatch.End();
            spriteBatch.GraphicsDevice.RasterizerState = WarpUtils.OverflowHiddenRasterizerState;
            spriteBatch.GraphicsDevice.ScissorRectangle = new Rectangle((int)position.X + 6, (int)position.Y + 6, width - 6, height);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, WarpUtils.OverflowHiddenRasterizerState, null, Main.UIScaleMatrix);

            Vector2 elementPosition = position + new Vector2(10 * direction, 8);
            for (int i = 0; i < elements.Value.Count; i++)
            {
                Vector2 off = elementPosition + new Vector2((int)(direction < 0 ? -FontAssets.MouseText.Value.MeasureString(elements.Value[i]).X * 0.66f : 0), 0);
                Color drawColor = i == hovered || i == selection.Value ? Main.OurFavoriteColor : Color.White;
                Utils.DrawBorderString(spriteBatch, elements.Value[i], off, drawColor.MultiplyRGBA(color), 0.66f);
                elementPosition.Y += 20;
            }

            spriteBatch.End();
            spriteBatch.GraphicsDevice.RasterizerState = priorRasterizer;
            spriteBatch.GraphicsDevice.ScissorRectangle = priorScissor;
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, priorRasterizer, null, Main.UIScaleMatrix);

            Utils.DrawBorderString(spriteBatch, scrollAmount.ToString(), position + new Vector2(180, 20), color, 1f);

        }

        public void Update(Player player, Vector2 position, Vector2 mousePos, int direction)
        {
            int scrollBarOffset = needsScrollBar ? 24 : 0;
            int scrollX = direction < 0 ? -width - scrollBarOffset + 4 : width + scrollBarOffset - 16;

            Rectangle area = new Rectangle((int)position.X - (direction < 0 ? width + scrollBarOffset : 0), (int)position.Y + 6, width + scrollBarOffset, height);
            hovered = selection.Value;
            
            Rectangle scrollArea = new Rectangle((int)position.X + scrollX, (int)position.Y + 6, 16, height - 6);
            int scrollHeight = (int)((float)(height / (elements.Value.Count * 20f));
            Rectangle scrollBarArea = new Rectangle((int)position.X + scrollX, (int)(position.Y + 4 + scrollAmount * height), 12, scrollHeight);

            Vector2 elementPosition = position + new Vector2(10 * direction, 8);

            if (needsScrollBar)
            {
                scrollHovered = scrollBarArea.Contains(mousePos.ToPoint());
                if (scrollHovered && player.WarpPlayer().mouseLeftHold && !dragging)
                {
                    dragging = true;
                    scrollOffset = (int)(Main.MouseScreen.Y - scrollBarArea.Y);
                }

                if (scrollBarArea.Contains(mousePos.ToPoint()) && player.WarpPlayer().mouseLeft)
                {
                    //scrollAmt = 
                }
            }

            if (area.Contains(mousePos.ToPoint()) && !dragging)
            {
                player.WarpInterface();

                for (int i = 0; i < elements.Value.Count; i++)
                {
                    Vector2 off = elementPosition + new Vector2(direction < 0 ? -width + 4 : 0, 0);
                    Rectangle elementArea = new Rectangle((int)off.X, (int)off.Y, width, 20);
                    if (elementArea.Contains(mousePos.ToPoint()))
                    {
                        hovered = i;
                        if (player.WarpPlayer().mouseLeft)
                            selection.Value = i;
                    }

                    elementPosition.Y += 20;
                }
            }

            if (dragging)
            {
                player.WarpInterface();

                float pos = Main.MouseScreen.Y - scrollBarArea.Y - scrollOffset;
                scrollAmount = MathHelper.Clamp(pos / height * (elements.Value.Count * 20), 0, elements.Value.Count * 20 - height);
                if (!player.WarpPlayer().mouseLeftHold)
                {
                    scrollHovered = false;
                    dragging = false;
                    scrollOffset = 0;
                }
            }

            scrollAmount = MathHelper.Clamp(scrollAmount, 0f, 1f);

            if (scrollHovered && !scrollOldHover)
                SoundEngine.PlaySound(AssetDirectory.Sounds_UI.MenuTick);

            if (hovered > -1 && oldHover != hovered)
                SoundEngine.PlaySound(AssetDirectory.Sounds_UI.MenuTick);

            oldHover = hovered;
            scrollOldHover = scrollHovered;
        }
    }
}
