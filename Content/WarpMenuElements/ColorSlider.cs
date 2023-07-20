using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperSpecialWarpinatorTool.Core;
using Terraria;
using Terraria.GameContent;

namespace SuperSpecialWarpinatorTool.Content.WarpMenuElements
{
    public class ColorSlider : IWarpMenuElement
    {
        private Ref<Color> color;

        private float[] hsv;

        private TypeBox boxHue;
        private TypeBox boxSaturation;
        private TypeBox boxValue;

        public ColorSlider(Ref<Color> color)
        {
            hsv = new float[3];
            this.color = color;
        }

        public int Height => 120;

        public void Draw(SpriteBatch spriteBatch, Color color, Player player, Vector2 position, Vector2 mousePos, int direction)
        {
        }

        public void Update(Player player, Vector2 position, Vector2 mousePos, int direction)
        {
            //color.Value = Main.hslToRgb(hsv[0], hsv[1], hsv[2]);
            color.Value = Main.DiscoColor;
        }
    }
}
