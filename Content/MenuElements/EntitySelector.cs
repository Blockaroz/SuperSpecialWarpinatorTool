using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperSpecialWarpinatorTool.Common.Systems;
using SuperSpecialWarpinatorTool.Common.UI;
using SuperSpecialWarpinatorTool.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace SuperSpecialWarpinatorTool.Content.MenuElements
{
    public class EntitySelector<T> : IMenuElement where T : Entity
    {
        private Ref<T> selection;

        private List<T> entities;

        private bool dragging;

        private Rope cursorRope;

        public EntitySelector(Ref<T> selection, T[] values)
        {
            this.selection = selection;
        }

        public int Width => 32;

        public int Height => 32;

        public void Draw(SpriteBatch spriteBatch, Color color, Player player, Vector2 position, Vector2 mousePos, int direction)
        {
            Vector2 buttonPos = position + new Vector2(40 * direction, 0);

            if (WarpUI.UISettings.ShowCursorWires)
            {
                if (cursorRope != null)
                {

                }
            }
        }

        public void Update(Player player, Vector2 position, Vector2 mousePos, int direction)
        {
            Vector2 buttonPos = position + new Vector2(40 * direction, 0);

            if (dragging)
            {
                VisualsSystem.HideCursor();

                if (player.WarpPlayer().mouseLeft)
                {
                    if (entities.Any(n => n.active && n.Hitbox.Contains(mousePos.ToPoint())))
                    {
                        selection.Value = entities.First(n => n.active && n.Hitbox.Contains(mousePos.ToPoint()));
                        dragging = false;
                    }
                }

                if (player.WarpPlayer().mouseRight)
                    dragging = false;
            }
        }
    }
}
