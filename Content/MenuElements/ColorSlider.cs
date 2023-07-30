using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using SuperSpecialWarpinatorTool.Common.UI;
using SuperSpecialWarpinatorTool.Core;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using static SuperSpecialWarpinatorTool.Content.MenuElements.ColorSlider;

namespace SuperSpecialWarpinatorTool.Content.MenuElements
{
    public class ColorSlider : IMenuElement
    {
        private Ref<Color> colorValue;

        private Slider[] sliders;

        private bool dragging;
        private bool moving;

        public ColorSlider(Ref<Color> color)
        {
            this.colorValue = color;
            sliders = new Slider[3];
            for (int i = 0; i < sliders.Length; i++)
            {
                sliders[i].area.Width = 178;
                sliders[i].area.Height = 14;
            }

            sliders[0].value = Main.rgbToHsl(colorValue.Value).X;
            sliders[1].value = Main.rgbToHsl(colorValue.Value).Y;
            sliders[2].value = Main.rgbToHsl(colorValue.Value).Z;
        }

        public int Width => 178;

        public int Height => 55;

        public void Draw(SpriteBatch spriteBatch, Color color, Player player, Vector2 position, Vector2 mousePos, int direction)
        {
            for (int i = 0; i < sliders.Length; i++)
                sliders[i].area.Location = new Point((int)position.X - (direction < 0 ? 178 : 0), (int)position.Y + (int)(18 * i * WarpinatorUISystem.WarpinatorUI.GetMenuFadeIn));

            sliders[0].DrawSlider(spriteBatch, color, Color.White, true, true);
            sliders[1].DrawSlider(spriteBatch, color, colorValue.Value, true);
            sliders[2].DrawSlider(spriteBatch, color, Color.White, true);
        }

        public void Update(Player player, Vector2 position, Vector2 mousePos, int direction)
        {
            Vector3 color = Main.rgbToHsl(colorValue.Value);

            if (!moving)
            {
                sliders[0].value = color.X;
                sliders[1].value = color.Y;
                sliders[2].value = color.Z;
            }

            bool anyHovers = false;
            for (int i = 0; i < 3; i++)
            {
                sliders[i].UpdateSlider(player, mousePos, dragging);

                if (sliders[i].moving)
                {
                    moving = true;
                    dragging = true;
                }

                if (sliders[i].hovered)
                    anyHovers = true;
            }

            if (anyHovers)
                player.WarpInterface();

            if (dragging)
            {
                moving = true;
                player.WarpInterface();

                color.X = sliders[0].value;
                color.Y = sliders[1].value;
                color.Z = sliders[2].value;

                if (!player.WarpPlayer().mouseLeftHold)
                    dragging = false;
            }

            colorValue.Value = Main.hslToRgb(color);
        }
    }
}
