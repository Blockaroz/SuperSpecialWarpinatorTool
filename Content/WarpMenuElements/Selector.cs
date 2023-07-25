using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperSpecialWarpinatorTool.Common.UI;
using SuperSpecialWarpinatorTool.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace SuperSpecialWarpinatorTool.Content.WarpMenuElements
{
    public class Selector<T> : IWarpMenuElement
    {
        Ref<T> selection;

        private bool dragging;

        public Selector(Ref<T> selection)
        {
            this.selection = selection;
        }

        public int Height => 32;

        public void Draw(SpriteBatch spriteBatch, Color color, Player player, Vector2 position, Vector2 mousePos, int direction)
        {
            Vector2 buttonPos = position + new Vector2(40 * direction, 0);

            if (WarpUI.UISettings.SelectionWires)
            {

            }
        }

        public void Update(Player player, Vector2 position, Vector2 mousePos, int direction)
        {
            Vector2 buttonPos = position + new Vector2(40 * direction, 0);
            if (dragging && player.WarpPlayer().mouseRight)
                dragging = false;
        }
    }
}
