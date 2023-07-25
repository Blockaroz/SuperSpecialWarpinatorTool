using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperSpecialWarpinatorTool.Common.UI;
using SuperSpecialWarpinatorTool.Content.WarpMenuElements;
using SuperSpecialWarpinatorTool.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SuperSpecialWarpinatorTool.Content.WarpinatorActions
{
    public class Light : WarpinatorAction
    {
        public Ref<bool> enabled = new Ref<bool>();

        public Ref<Color> lightColor = new Ref<Color>();

        public override bool HasPerformableAction => false;

        public override void SetDefaults()
        {
            enabled.Value = false;
            lightColor.Value = WarpUtils.WarpColor();
        }

        public override void Update(Player player)
        {
            if (enabled.Value)
                Lighting.AddLight(player.Center, lightColor.Value.ToVector3());
        }

        public override List<IWarpMenuElement> AddMenuElements() => new List<IWarpMenuElement>()            
        {       
            new Text(Mod, "Enabled"),
            new Toggle(enabled),
            new Text(Mod, "Color"),
            new ColorSlider(lightColor)
        };

        private TextureAsset glowTexture;

        public override void OnLoad(Mod mod)
        {
            glowTexture = new TextureAsset(Texture + "Glow");
        }

        public override void DrawIcon(SpriteBatch spriteBatch, Vector2 center, Color color, float scale)
        {
            base.DrawIcon(spriteBatch, center, color, scale);

            Color bulbColor = new Color(lightColor.Value.ToVector3() * 0.9f + Vector3.One * 0.1f);
            spriteBatch.Draw(glowTexture, center, iconTexture.Frame(), bulbColor.MultiplyRGBA(color), 0, iconTexture.Size() * 0.5f, scale, 0, 0);
        }
    }
}
