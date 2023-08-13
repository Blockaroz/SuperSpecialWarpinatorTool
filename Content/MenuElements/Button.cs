using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperSpecialWarpinatorTool.Core;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ModLoader;
using static System.Net.Mime.MediaTypeNames;

namespace SuperSpecialWarpinatorTool.Content.MenuElements
{
    public class Button : IMenuElement
    {
        private Action onClick;

        private Text buttonText;

        public Button(Action onClick, Text buttonText)
        {
            this.onClick = onClick;
            this.buttonText = buttonText;
        }

        public int Width => buttonText.Width + 16;

        public int Height => buttonText.Height + 8;

        private bool oldHover;

        public void Draw(SpriteBatch spriteBatch, Color color, Player player, Vector2 position, Vector2 mousePos, int direction)
        {
            Vector2 buttonPos = position + new Vector2(direction < 0 ? -Width : 0, 0);
            Rectangle area = new Rectangle((int)buttonPos.X, (int)buttonPos.Y, buttonText.Width + 16, buttonText.Height + 16);
            bool hovered = area.Contains(mousePos.ToPoint());

            Color hoverColor = hovered ? Main.OurFavoriteColor : Color.White;
            Color clickColor = hovered && player.WarpPlayer().mouseLeftHold ? Color.DarkGray : Color.White;
            WarpUtils.DrawPanel(spriteBatch, (int)buttonPos.X, (int)buttonPos.Y, Width, Height, color.MultiplyRGBA(clickColor), hovered);
            buttonText.Draw(spriteBatch, color.MultiplyRGBA(hoverColor).MultiplyRGBA(clickColor), player, position + new Vector2(8 * direction, 6), mousePos, direction);
        }

        public void Update(Player player, Vector2 position, Vector2 mousePos, int direction)
        {
            Vector2 buttonPos = position + new Vector2(direction < 0 ? -Width : 0, 0);
            Rectangle area = new Rectangle((int)buttonPos.X, (int)buttonPos.Y, buttonText.Width + 12, buttonText.Height + 12);
            bool hovered = area.Contains(mousePos.ToPoint());

            if (hovered)
            {
                if (!oldHover)
                    SoundEngine.PlaySound(AssetDirectory.Sounds.MenuTick);

                if (player.WarpPlayer().mouseLeft)
                {
                    onClick.Invoke();
                    SoundEngine.PlaySound(AssetDirectory.Sounds.MenuTickSelect);
                }
            }

            oldHover = hovered;
            buttonText.Update(player, position + new Vector2(6), mousePos, direction);
        }
    }
}
