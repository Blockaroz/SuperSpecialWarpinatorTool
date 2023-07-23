using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperSpecialWarpinatorTool.Core;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using static SuperSpecialWarpinatorTool.Content.WarpMenuElements.ColorSlider;

namespace SuperSpecialWarpinatorTool.Content.WarpMenuElements
{
    public class ColorSlider : IWarpMenuElement
    {
        private Ref<Color> realColor;

        private float[] hsl;
        private Slider[] sliders;

        private bool dragging;

        public ColorSlider(Ref<Color> color)
        {
            this.realColor = color;
            sliders = new Slider[3];

            Vector3 toFloat = Main.rgbToHsl(color.Value);
            hsl = new float[3] { toFloat.X, toFloat.Y, toFloat.Z };
        }

        public int Height => 44;

        public void Draw(SpriteBatch spriteBatch, Color color, Player player, Vector2 position, Vector2 mousePos, int direction)
        {
            for (int i = 0; i < 3; i++)
                sliders[i].area = new Rectangle((int)position.X - (direction < 0 ? -120 : 0), (int)position.Y + 18 * i, 178, 14);

            sliders[0].DrawSlider(spriteBatch, color, Color.White, true);
            sliders[1].DrawSlider(spriteBatch, color, realColor.Value, false);
            sliders[2].DrawSlider(spriteBatch, color, Color.White, false);
        }

        private int lastDragged;
        private bool oldHover;

        public void Update(Player player, Vector2 position, Vector2 mousePos, int direction)
        {
            bool anyHover = false;
            for (int i = 0; i < 3; i++)
            {
                sliders[i].hovered = false;

                if (sliders[i].area.Contains(mousePos.ToPoint()) && !dragging)
                {
                    sliders[i].hovered = true;
                    if (!oldHover)
                        SoundEngine.PlaySound(AssetDirectory.Sounds_UI.MenuTick);

                    anyHover = true;

                    if (player.WarpPlayer().mouseLeftHold)
                    {
                        dragging = true;
                        lastDragged = i;
                    }
                }

                sliders[i].value = hsl[i];
            }

            oldHover = anyHover;

            if (dragging && lastDragged > -1)
            {
                player.WarpInterface();
                hsl[lastDragged] = Utils.GetLerpValue(4, sliders[lastDragged].area.Width - 4, mousePos.X - sliders[lastDragged].area.X, true);
                if (!player.WarpPlayer().mouseLeftHold)
                {
                    dragging = false;
                    lastDragged = -1;
                }
            }

            realColor.Value = Main.hslToRgb(hsl[0], hsl[1], hsl[2]);
        }

        public struct Slider
        {
            public float value;
            public Rectangle area;
            public bool hovered;

            public void DrawSlider(SpriteBatch spriteBatch, Color color, Color barColor, bool rainbow)
            {
                Texture2D texture = AssetDirectory.Textures_UI.Slider;
                Texture2D buttonTexture = AssetDirectory.Textures_UI.SliderButton;
                Rectangle baseFrame = texture.Frame(1, 4, 0, 0);
                Rectangle hoverFrame = texture.Frame(1, 4, 0, 0);
                Rectangle colorFrame = texture.Frame(1, 4, 0, rainbow ? 3 : 2);

                spriteBatch.Draw(texture, area.Location.ToVector2(), baseFrame, color, 0, Vector2.Zero, 1f, 0, 0);
                if (hovered)
                    spriteBatch.Draw(texture, area.Location.ToVector2(), hoverFrame, color.MultiplyRGBA(Main.OurFavoriteColor), 0, Vector2.Zero, 1f, 0, 0);

                spriteBatch.Draw(texture, area.Location.ToVector2(), colorFrame, barColor.MultiplyRGBA(color), 0, Vector2.Zero, 1f, 0, 0);
                spriteBatch.Draw(buttonTexture, area.Location.ToVector2() + new Vector2(texture.Width * value, baseFrame.Height / 2.5f), buttonTexture.Frame(), color, 0, buttonTexture.Size() * 0.5f, 1f, 0, 0);
            }
        }
    }
}
