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
    public class ScrollPanel : IWarpMenuElement
    {
        private Ref<List<string>> elements;
        private Ref<string> selection;
        private int width;
        private int height;
        private bool dragging;

        public ScrollPanel(Ref<List<string>> elements, Ref<string> selection, int width, int height)
        {
            this.elements = elements;
            this.selection = selection;
            this.height = height;
        }

        public int Height => height;

        public void Draw(SpriteBatch spriteBatch, Color color, Player player, Vector2 position, Vector2 mousePos, int direction)
        {
            //spriteBatch.GraphicsDevice.ScissorRectangle = new Rectangle((int)position.X, (int)position.Y, width, height);
        }

        public void Update(Player player, Vector2 position, Vector2 mousePos, int direction)
        {
            Rectangle area = new Rectangle((int)position.X, (int)position.Y, width + 16, height);
            if (area.Contains(mousePos.ToPoint()))
            {
                player.WarpInterface();
            }
        }
    }
}
