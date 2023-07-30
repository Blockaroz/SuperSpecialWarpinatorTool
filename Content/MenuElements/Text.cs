using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using SuperSpecialWarpinatorTool.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SuperSpecialWarpinatorTool.Content.MenuElements
{
    public class Text : IMenuElement, ILocalizedModType
    {
        public string LocalizationCategory => "WarpinatorMenus";

        public Mod Mod { get; private set; }

        public string Name => GetType().Name;

        public string FullName => GetType().FullName;

        private readonly LocalizedText text;

        private readonly Color drawColor;

        private float scale;

        public Text(Mod mod, string key, Color? drawColor = null, float scale = 1f)
        {
            text = Language.GetOrRegister(mod.GetLocalizationKey(key));
            this.drawColor = drawColor.HasValue ? drawColor.Value : Color.White;
            this.scale = scale;
        }

        public int Width => (int)(FontAssets.MouseText.Value.MeasureString(text.Value).X * scale + 2);

        public int Height => (int)(25 * (0.33f + scale * 0.66f));

        public void Draw(SpriteBatch spriteBatch, Color color, Player player, Vector2 position, Vector2 mousePos, int direction)
        {
            Vector2 pos = position + new Vector2(direction < 0 ? -FontAssets.MouseText.Value.MeasureString(text.Value).X * scale : 0, 0);

            //spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle((int)pos.X, (int)pos.Y, (int)FontAssets.MouseText.Value.MeasureString(text.Value).X, Height), Color.Black);
            Utils.DrawBorderString(spriteBatch, text.Value, pos, drawColor.MultiplyRGBA(color), scale);
        }

        public void Update(Player player, Vector2 position, Vector2 mousePos, int direction) { }
    }
}
