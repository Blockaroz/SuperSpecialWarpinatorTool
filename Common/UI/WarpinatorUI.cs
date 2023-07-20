using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperSpecialWarpinatorTool.Content.Items;
using SuperSpecialWarpinatorTool.Core;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using static Terraria.GameContent.Animations.IL_Actions.NPCs;

namespace SuperSpecialWarpinatorTool.Common.UI
{
    public class WarpinatorUI : UIState
    {
        public bool OnMyInterface;

        public bool Open { get; set; }

        public bool MenuOpen { get; set; }

        public bool Lefty { get; set; }
        public bool ShowName { get; set; }
        public bool ShowNamePermanent { get; set; }

        private int direction => Lefty ? -1 : 1;

        private bool Usable => Open && fadeIn > 0.95f;
        private bool MenuUsable => MenuOpen && menuFadeIn > 0.95f;

        public void OpenUI()
        {           
            Open = !Open;
            if (Open)
            {
                position = Main.MouseScreen;
                if (PlayerInput.UsingGamepad && Main.SmartCursorWanted)
                    position = new Vector2(Main.screenWidth, Main.screenHeight) / 2f;

            }

            //SoundEngine.PlaySound(Open ? AssetDirectory.Sounds_UI.MenuOpen : AssetDirectory.Sounds_UI.MenuClose);
        }

        private Color TextColor => Color.Lerp(new Color(180, 0, 200, 200), Color.White, Utils.GetLerpValue(0.7f, 0.85f, menuFadeIn * fadeIn, true)) * Utils.GetLerpValue(0.5f, 0.9f, menuFadeIn * fadeIn, true);

        private Vector2 position;

        private float rotation;
        private float targetRotation;

        private float fadeIn;
        private float menuFadeIn;

        private bool dragging;
        private Vector2 dragOffset;

        private Vector2 MenuPosition => position + new Vector2(((60 + 40 * menuFadeIn) * fadeIn + 5 * Math.Max(Main.LocalPlayer.WarpPlayer().actions.Count - 5, 0)) * direction, 4);
        private Vector2 MenuButtonPosition => position + new Vector2((45 + 30 * fadeIn + 5 * Math.Max(Main.LocalPlayer.WarpPlayer().actions.Count - 5, 0)) * direction, 0);

        private bool HoveringOverDrag => Main.MouseScreen.Distance(position) < 16 && fadeIn > 0.96f || dragging;
        private bool HoveringOverMenuArrow => Main.MouseScreen.Distance(MenuButtonPosition) < 16 && fadeIn > 0.96f;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Player player = Main.LocalPlayer;
            WarpinatorAction action = player.WarpPlayer().CurrentAction;

            fadeIn = Math.Clamp(MathHelper.Lerp(fadeIn, Open ? 1f : 0, 0.2f), 0, 1);
            menuFadeIn = Math.Clamp(MathHelper.Lerp(menuFadeIn, MenuOpen ? 1f : 0, MenuOpen ? 0.3f : 0.2f), 0, 1);
            targetRotation = -MathHelper.TwoPi / player.WarpPlayer().actions.Count * player.WarpPlayer().currentActionIndex - ((1f - fadeIn) * MathHelper.Pi * (Open ? -1 : 1) * direction);
            rotation = Utils.AngleLerp(rotation, targetRotation + (Lefty ? 0 : MathHelper.Pi), 0.3f);

            if (fadeIn > 0.99f)
                fadeIn = 1f;
            if (fadeIn < 0.3f && !Open)
                fadeIn = 0f;

            bool otherInterface = (player.mouseInterface || player.lastMouseInterface) && !OnMyInterface;
            bool condition1 = player.dead || Main.mouseItem.type > ItemID.None;
            bool condition2 = !player.ValidWarpinator() || PlayerInput.LockGamepadTileUseButton || player.noThrow != 0 || Main.HoveringOverAnNPC || player.talkNPC != -1;
            if (otherInterface || condition1 || condition2)
            {
                Open = false;
                return;
            }

            OnMyInterface = false;

            if (player.WarpPlayer().mouseRight && !Main.SmartInteractShowingGenuine)
                OpenUI();

            if (!dragging && HoveringOverDrag && player.WarpPlayer().mouseLeftHold)
            {
                dragOffset = Main.MouseScreen - position;
                dragging = true;
            }

            if ((dragging && !player.WarpPlayer().mouseLeftHold) || Main.MouseScreen.X > Main.screenWidth || Main.MouseScreen.Y > Main.screenHeight)
                dragging = false;

            if (dragging)
            {
                position = Main.MouseScreen - dragOffset;
                dragOffset = Vector2.Lerp(dragOffset, Vector2.Zero, 0.04f);
            }

            position = new Vector2((int)position.X, (int)position.Y);
            bool hoveringOverAnything = HoveringOverDrag || HoveringOverMenuArrow;
            for (int i = 0; i < player.WarpPlayer().actions.Count; i++)
            {
                Vector2 actPosition = position - new Vector2(30 + 10 * fadeIn + 5 * Math.Max(player.WarpPlayer().actions.Count - 5, 0), 0).RotatedBy(rotation + MathHelper.TwoPi / player.WarpPlayer().actions.Count * i);
                bool innerHover = Main.MouseScreen.Distance(actPosition) < 20;
                player.WarpPlayer().CurrentAction.Selected = false;
                if (innerHover)
                {
                    //Main.instance.MouseTextHackZoom(player.WarpPlayer().actions[i].Name);
                    //Main.mouseText = true;

                    hoveringOverAnything = true;
                    if (player.WarpPlayer().mouseLeft)
                    {
                        player.WarpPlayer().currentActionIndex = i;
                        player.WarpPlayer().CurrentAction.Selected = true;
                        SoundEngine.PlaySound(AssetDirectory.Sounds_UI.MenuTickSelect);
                    }
                }
            }

            if (HoveringOverMenuArrow && player.WarpPlayer().mouseLeft)
            {
                MenuOpen = !MenuOpen;
                SoundEngine.PlaySound(MenuOpen ? AssetDirectory.Sounds_UI.MiniMenuOpen : AssetDirectory.Sounds_UI.MiniMenuClose);
            }

            if (hoveringOverAnything)
                player.WarpInterface();

            if (player.WarpPlayer().CurrentAction.Options.Count > 0 && MenuUsable)
            {
                Vector2 menuPos = MenuPosition;

                for (int i = 0; i < action.Options.Count; i++)
                {
                    float height = action.Options[i].Height * fadeIn;
                    menuPos.Y -= height / 2f;
                }
                for (int i = 0; i < action.Options.Count; i++)
                {
                    float height = action.Options[i].Height * fadeIn;
                    action.Options[i].Update(player, menuPos, Main.MouseScreen, direction);
                    menuPos.Y += height;
                }
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            if (fadeIn < 0.05f)
                return;

            Player player = Main.LocalPlayer;
            WarpinatorAction action = player.WarpPlayer().CurrentAction;

            //Draw drag button in center

            float dragScale = Utils.GetLerpValue(0f, 0.7f, fadeIn, true) * (dragging ? 1.1f : 0.9f);
            spriteBatch.Draw(AssetDirectory.Textures_UI.DragButton[0], position, AssetDirectory.Textures_UI.DragButton[0].Texture.Frame(), Color.White, 0, AssetDirectory.Textures_UI.DragButton[0].Texture.Size() * 0.5f, dragScale, 0, 0);
            if (HoveringOverDrag || dragging)
                spriteBatch.Draw(AssetDirectory.Textures_UI.DragButton[1], position, AssetDirectory.Textures_UI.DragButton[1].Texture.Frame(), Color.Gold, 0, AssetDirectory.Textures_UI.DragButton[1].Texture.Size() * 0.5f, dragScale, 0, 0);

            float arrowScale = Utils.GetLerpValue(0.85f, 0.95f, fadeIn, true);
            SpriteEffects arrowEffect = direction > 0 ^ MenuOpen ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(AssetDirectory.Textures_UI.ButtonArrow[0], MenuButtonPosition, AssetDirectory.Textures_UI.ButtonArrow[0].Texture.Frame(), Color.White, 0, AssetDirectory.Textures_UI.ButtonArrow[0].Texture.Size() * 0.5f, arrowScale, arrowEffect, 0);
            if (HoveringOverMenuArrow)
                spriteBatch.Draw(AssetDirectory.Textures_UI.ButtonArrow[1], MenuButtonPosition, AssetDirectory.Textures_UI.ButtonArrow[1].Texture.Frame(), Color.Gold, 0, AssetDirectory.Textures_UI.ButtonArrow[1].Texture.Size() * 0.5f, arrowScale, arrowEffect, 0);

            Color iconColor = Color.Lerp(Color.Magenta, Color.White, Utils.GetLerpValue(0.2f, 0.9f, fadeIn, true));

            for (int i = 0; i < player.WarpPlayer().actions.Count; i++)
            {
                Vector2 actPosition = position - new Vector2(30 + 10 * fadeIn + 5 * Math.Max(player.WarpPlayer().actions.Count - 5, 0), 0).RotatedBy(rotation + MathHelper.TwoPi / player.WarpPlayer().actions.Count * i).ToPoint().ToVector2();
                float actScale = MathF.Sqrt(Utils.GetLerpValue(0.1f, 1f, fadeIn, true));

                bool hoverOrSelected = (Main.MouseScreen.Distance(actPosition) < 20 || i == player.WarpPlayer().currentActionIndex) && Usable;

                Texture2D texture = AssetDirectory.Textures_UI.FlowerButton[hoverOrSelected ? 1 : 0];
                spriteBatch.Draw(texture, actPosition, texture.Frame(), Color.White, 0, texture.Size() * 0.5f, actScale, 0, 0);
                player.WarpPlayer().actions[i].DrawIcon(spriteBatch, actPosition, iconColor, actScale);
            }

            if (player.WarpPlayer().CurrentAction.Options.Count > 0 && menuFadeIn > 0.05f)
            {
                Vector2 menuPos = MenuPosition;

                for (int i = 0; i < action.Options.Count; i++)
                {
                    float height = action.Options[i].Height * fadeIn;
                    menuPos.Y -= height / 2f;
                }
                for (int i = 0; i < action.Options.Count; i++)
                {
                    float height = action.Options[i].Height * fadeIn;
                    action.Options[i].Draw(spriteBatch, TextColor, player, menuPos, Main.MouseScreen, direction);
                    menuPos.Y += height;
                }
            }

            if (ShowName)
            {
                float fade = (ShowNamePermanent ? 1f : menuFadeIn) * fadeIn;
                string name = player.WarpPlayer().CurrentAction.DisplayName.Value;
                Vector2 namePosition = position + new Vector2(-(30 + 40 * fade + 5 * Math.Max(player.WarpPlayer().actions.Count - 5, 0) + (direction > 0 ? FontAssets.MouseText.Value.MeasureString(name).X : 0)) * direction, -10);
                Utils.DrawBorderString(spriteBatch, name, namePosition, Color.Lerp(new Color(180, 0, 200, 200), Color.White, Utils.GetLerpValue(0.7f, 0.85f, fade, true)) * Utils.GetLerpValue(0.5f, 0.9f, fade, true), 1f);
            }
        }
    }
}