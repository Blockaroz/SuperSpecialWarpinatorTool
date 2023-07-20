using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperSpecialWarpinatorTool.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.GameInput;

namespace SuperSpecialWarpinatorTool.Content.WarpMenuElements
{
    public class TypeBox : IWarpMenuElement
    {
        private Ref<string> text;

        private string indicator;

        private int indicatorTime;

        private bool typing;

        public TypeBox(Ref<string> text)
        {
            this.text = text;
        }

        public int Height => 24;

        public void Draw(SpriteBatch spriteBatch, Color color, Player player, Vector2 position, Vector2 mousePos, int direction)
        {
            if (text.Value == null)
                text.Value = "";

            Utils.DrawBorderString(spriteBatch, text.Value + indicator, position + new Vector2(3 * direction + (direction < 0 ? -FontAssets.MouseText.Value.MeasureString(text.Value).X : 0), 3), color, 0.8f);
        }

        public void Update(Player player, Vector2 position, Vector2 mousePos, int direction)
        {
            if (typing)
            {
                if (++indicatorTime > 30)
                {
                    indicatorTime = 0;
                    if (indicator == "|")
                        indicator = "";
                    else
                        indicator = "|";
                }

                UpdateTyping();
            }
            else
            {
                indicatorTime = 30;
                indicator = "";
            }
        }

        private void UpdateTyping()
        {
            if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                typing = false;
                return;
            }

            PlayerInput.WritingText = true;
            Main.instance.HandleIME();
            string newText = Main.GetInputText(text.Value);
        }
    }
}
