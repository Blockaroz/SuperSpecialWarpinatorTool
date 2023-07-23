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
    public class EntitySelector : IWarpMenuElement
    {
        Ref<Entity> selection;

        private bool dragging;

        public EntitySelector(Ref<Entity> selection)
        {
            this.selection = selection;
        }

        public int Height => 32;

        public void Draw(SpriteBatch spriteBatch, Color color, Player player, Vector2 position, Vector2 mousePos, int direction)
        {
            if (WarpUI.UISettings.SelectionWires)
            {

            }
        }

        public void Update(Player player, Vector2 position, Vector2 mousePos, int direction)
        {
            if (dragging && player.WarpPlayer().mouseRight)
                dragging = false;
        }
    }
}
