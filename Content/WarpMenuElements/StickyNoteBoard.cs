using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperSpecialWarpinatorTool.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace SuperSpecialWarpinatorTool.Content.WarpMenuElements
{
    public class StickyNoteBoard : IWarpMenuElement
    {
        public int Height => 20;

        public void Draw(SpriteBatch spriteBatch, Color color, Player player, Vector2 position, Vector2 mousePos, int direction)
        {
        }

        public void Update(Player player, Vector2 position, Vector2 mousePos, int direction)
        {
        }
    }
}
