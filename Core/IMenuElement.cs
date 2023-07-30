using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SuperSpecialWarpinatorTool.Core
{
    public interface IMenuElement 
    {
        public abstract void Update(Player player, Vector2 position, Vector2 mousePos, int direction);

        public abstract void Draw(SpriteBatch spriteBatch, Color color, Player player, Vector2 position, Vector2 mousePos, int direction);

        public int Width { get; }

        public int Height { get; }
    }
}
