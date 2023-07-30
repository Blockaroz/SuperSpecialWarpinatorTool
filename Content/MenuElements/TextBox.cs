using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Localization.IME;
using ReLogic.OS;
using SuperSpecialWarpinatorTool.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.GameInput;

namespace SuperSpecialWarpinatorTool.Content.MenuElements
{
    public class TextBox : IMenuElement
    {
        private Ref<string> text;

        private string indicator;

        private int indicatorTime;

        private bool typing;

        private InputType inputType;

        private int width;

        public TextBox(Ref<string> text, InputType inputType = InputType.Text)
        {
            this.text = text;
            text.Value = "";
            this.inputType = inputType;
        }

        public int Width => width;

        public int Height => 24;

        public void Draw(SpriteBatch spriteBatch, Color color, Player player, Vector2 position, Vector2 mousePos, int direction)
        {
            if (text.Value == null)
                text.Value = "";

            Utils.DrawBorderString(spriteBatch, text.Value + indicator, position + new Vector2(3 * direction + (direction < 0 ? -FontAssets.MouseText.Value.MeasureString(text.Value).X : 0), 3), color, 0.8f);
        }

        public void Update(Player player, Vector2 position, Vector2 mousePos, int direction)
        {
            width = (int)Math.Max(50, FontAssets.MouseText.Value.MeasureString(text.Value).X * 0.66f);
            Rectangle area = new Rectangle((int)position.X - (direction < 0 ? width : 0), (int)position.Y, Math.Max(50, width), Height);
            if (area.Contains(mousePos.ToPoint()) && player.WarpPlayer().mouseLeft)
                typing = true;

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

        private bool _oldHasCompositionString;

        public enum InputType
        {
            Text,
            Number,
            Integer
        }

        private void UpdateTyping()
        {
            if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                typing = false;
                Main.blockInput = false;
                return;
            }

            PlayerInput.WritingText = true;
            Main.instance.HandleIME();

            string newText = Main.GetInputText(text.Value);

            if (_oldHasCompositionString && Main.inputText.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Back))
                newText = text.Value;

            bool integer = inputType == InputType.Integer && Regex.IsMatch(newText, "[0-9]*$");
            bool number = inputType == InputType.Number && Regex.IsMatch(newText, "(?<=^| )[0-9]+(.[0-9]+)?(?=$| )|(?<=^| ).[0-9]+(?=$| )");
            bool normalText = inputType == InputType.Text;

            if ((integer || number || normalText) && text.Value != newText)
                text.Value = newText;

            _oldHasCompositionString = Platform.Get<IImeService>().CompositionString is { Length: > 0 };
        }
    }
}
