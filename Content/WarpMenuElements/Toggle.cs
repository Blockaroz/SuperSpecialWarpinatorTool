using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperSpecialWarpinatorTool.Core;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;

namespace SuperSpecialWarpinatorTool.Content.WarpMenuElements
{
    public class Toggle : IWarpMenuElement
    {
        public Ref<bool> condition;

        private bool oldHover;

        private float fancyTime;

        public Toggle(Ref<bool> condition)
        {
            this.condition = condition;
        }

        public int Height => 32;

        public void Draw(SpriteBatch spriteBatch, Color color, Player player, Vector2 position, Vector2 mousePos, int direction)
        {
            Texture2D texture = AssetDirectory.Textures_UI.ToggleButton;
            Rectangle frame = texture.Frame(2, 2, condition.Value ? 1 : 0, 0);
            Rectangle outline = texture.Frame(2, 2, condition.Value ? 1 : 0, 1);

            Vector2 offset = new Vector2(12 * direction, Height / 2);
            spriteBatch.Draw(texture, position + offset, frame, color, 0, frame.Size() * 0.5f, 1f, 0, 0);
            
            string text = Lang.menu[condition.Value ? 126 : 117].Value;

            Vector2 pos = position + new Vector2(25 * direction + (direction < 0 ? -FontAssets.MouseText.Value.MeasureString(text).X : 0), 9);
            if (fancyTime > 0)
                Utils.DrawBorderString(spriteBatch, text, pos + new Vector2(10 * (1 - fancyTime), 0), WarpUtils.WarpColor(0).MultiplyRGBA(color) * fancyTime, 0.7f);

            Utils.DrawBorderString(spriteBatch, text, pos, color * 0.5f, 0.7f);

            bool hovering = mousePos.Distance(position + offset) < 16;
            if (hovering)
                spriteBatch.Draw(texture, position + offset, outline, Main.OurFavoriteColor.MultiplyRGBA(color), 0, outline.Size() * 0.5f, 1f, 0, 0);
        }

        public void Update(Player player, Vector2 position, Vector2 mousePos, int direction)
        {
            Vector2 offset = new Vector2(12 * direction, Height / 2);

            bool hovering = mousePos.Distance(position + offset) < 16;

            if (hovering)
            {
                player.WarpInterface();

                if (!oldHover)
                    SoundEngine.PlaySound(AssetDirectory.Sounds_UI.MenuTick);
            }
            if (hovering && player.WarpPlayer().mouseLeft)
            {
                condition.Value = !condition.Value;
                SoundEngine.PlaySound(condition.Value ? AssetDirectory.Sounds_UI.MenuTickSelect : AssetDirectory.Sounds_UI.MenuTickSelectOff);
                fancyTime = 1f;
            }

            if (fancyTime > 0)
                fancyTime -= 0.1f;

            oldHover = hovering;
        }
    }
}
