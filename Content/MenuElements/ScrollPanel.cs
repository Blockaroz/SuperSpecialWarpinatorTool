using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperSpecialWarpinatorTool.Common.UI;
using SuperSpecialWarpinatorTool.Core;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;

namespace SuperSpecialWarpinatorTool.Content.MenuElements
{
    public class ScrollPanel : IMenuElement
    {
        private Ref<List<string>> elements;
        private Ref<int> selection;
        private int width;
        private int height;
        private bool dragging;
        private bool canSelect;
        private int hovered;

        private ScrollBar scrollBar;
        private bool needsScrollBar;

        public ScrollPanel(Ref<List<string>> elements, Ref<int> selection, int width, int height, bool canSelect = true)
        {
            this.elements = elements;
            this.selection = selection;
            this.width = width;
            this.height = height;
            this.canSelect = canSelect;
            needsScrollBar = elements.Value.Count * 20 > height;
        }

        public int Width => width;

        public int Height => height + 16;

        public void Draw(SpriteBatch spriteBatch, Color color, Player player, Vector2 position, Vector2 mousePos, int direction)
        {
            int scrollBarOffset = needsScrollBar ? 18 : 0;
            WarpUtils.DrawPanel(spriteBatch, (int)(position.X - (direction < 0 ? width + scrollBarOffset : 0)), (int)position.Y, width + scrollBarOffset, height + 12, color);

            int scrollX = direction < 0 ? -width - scrollBarOffset + 4 : width + scrollBarOffset - 16;
            scrollBar.viewArea = height;
            scrollBar.maxViewArea = elements.Value.Count * 20f;
            scrollBar.position = new Vector2((int)position.X + scrollX, (int)position.Y + 4);
            scrollBar.height = height + 4;

            if (needsScrollBar)
                scrollBar.Draw(spriteBatch, color);

            RasterizerState priorRasterizer = spriteBatch.GraphicsDevice.RasterizerState;
            Rectangle priorScissor = spriteBatch.GraphicsDevice.ScissorRectangle;

            spriteBatch.End();
            spriteBatch.GraphicsDevice.RasterizerState = WarpUtils.OverflowHiddenRasterizerState;
            spriteBatch.GraphicsDevice.ScissorRectangle = new Rectangle((int)position.X + 6, (int)position.Y + 6, width - 6, height);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, WarpUtils.OverflowHiddenRasterizerState, null, Main.UIScaleMatrix);

            Vector2 elementPosition = position + new Vector2(8 * direction, 8 - scrollBar.value);
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
        }

        public void Update(Player player, Vector2 position, Vector2 mousePos, int direction)
        {
            int scrollBarOffset = needsScrollBar ? 18 : 0;
            Rectangle area = new Rectangle((int)position.X - (direction < 0 ? width + scrollBarOffset : 0), (int)position.Y + 6, width + scrollBarOffset, height);
            hovered = selection.Value;
            
            Vector2 elementPosition = position + new Vector2(10 * direction, 8 - scrollBar.value);
            
            dragging = false;

            if (area.Contains(mousePos.ToPoint()) && !dragging)
            {
                player.WarpInterface();

                if (needsScrollBar && Player.GetMouseScrollDelta() != 0)
                    scrollBar.value -= Player.GetMouseScrollDelta() * 20;

                if (!scrollBar.hoverArea)
                {
                    for (int i = 0; i < elements.Value.Count; i++)
                    {
                        Vector2 off = elementPosition + new Vector2(direction < 0 ? -width + 4 : 0, 0);
                        Rectangle elementArea = new Rectangle((int)off.X, (int)off.Y, width, 20);
                        if (elementArea.Contains(mousePos.ToPoint()) && canSelect)
                        {
                            hovered = i;
                            if (player.WarpPlayer().mouseLeft)
                            {
                                selection.Value = i;
                                SoundEngine.PlaySound(AssetDirectory.Sounds.MenuTickSelect);
                            }
                        }
                        elementPosition.Y += 20;
                    }
                }
            }

            selection.Value = Math.Clamp(selection.Value, 0, elements.Value.Count - 1);

            if (needsScrollBar)
            {
                scrollBar.Update(player, mousePos, dragging);
                if (scrollBar.hoverArea)
                    player.WarpInterface();
                if (scrollBar.moving)
                    dragging = true;
            }

            if (dragging)
                player.WarpInterface();
        }
    }
}
