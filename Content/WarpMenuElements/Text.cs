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

namespace SuperSpecialWarpinatorTool.Content.WarpMenuElements
{
    public class Text : IWarpMenuElement, ILocalizedModType
    {
        public string LocalizationCategory => "WarpinatorMenuElements";

        public Mod Mod { get; private set; }

        public string Name => GetType().Name;

        public string FullName => GetType().FullName;

        public int Height => (int)(24 * scale);

        private LocalizedText text;

        private Color color;

        private float scale;

        public Text(Mod mod, string key, Color? color = null, float scale = 1f)
        {
            text = Language.GetOrRegister(mod.GetLocalizationKey(LocalizationCategory + '.' + key));
            this.color = color.HasValue ? color.Value : Color.White;
            this.scale = scale;
        }

        public void Draw(SpriteBatch spriteBatch, Color color, Player player, Vector2 position, Vector2 mousePos, int direction)
        {
            Vector2 pos = position + new Vector2(direction < 0 ? -FontAssets.MouseText.Value.MeasureString(text.Value).X * scale : 0, 0); 

            Utils.DrawBorderString(spriteBatch, text.Value, pos, color, scale);
        }

        public void Update(Player player, Vector2 position, Vector2 mousePos, int direction) { }
    }
}
