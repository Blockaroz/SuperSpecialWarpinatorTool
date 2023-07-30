using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperSpecialWarpinatorTool.Common.Systems;
using SuperSpecialWarpinatorTool.Content.MenuElements;
using SuperSpecialWarpinatorTool.Core;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.UI;

namespace SuperSpecialWarpinatorTool.Common.UI
{
    public class WarpUI : UIState
    {
        public static class UISettings
        {
            public static bool Lefty { get; set; }
            public static bool BackBox { get; set; }
            public static bool Name { get; set; }
            public static bool NamePerm { get; set; }
            public static bool ShowCursorWires { get; set; }
            public static OptionEnum CursorMode { get; set; }
            public static bool CursorMouseColor { get; set; }
        }

        public bool OnMyInterface;

        public bool Open { get; set; }

        public bool MenuOpen { get; set; }

        private int direction => UISettings.Lefty ? -1 : 1;

        private bool Usable => Open && fadeIn > 0.95f;
        private bool MenuUsable => MenuOpen && menuFadeIn > 0.95f;

        public bool Active => fadeIn > 0f;

        public void OpenUI(bool open)
        {           
            if (Open != open)
                SoundEngine.PlaySound(open ? AssetDirectory.Sounds_UI.MenuOpen : AssetDirectory.Sounds_UI.MenuClose);

            Open = open;
            if (Open)
            {
                position = Main.MouseScreen;
                if (PlayerInput.UsingGamepad && Main.SmartCursorWanted)
                    position = new Vector2(Main.screenWidth, Main.screenHeight) / 2f;

            }
        }

        private Color MenuFadeColor => Color.Lerp(new Color(180, 0, 200, 200), Color.White, Utils.GetLerpValue(0.7f, 0.85f, menuFadeIn * fadeIn, true)) * Utils.GetLerpValue(0.5f, 0.9f, menuFadeIn * fadeIn, true);

        private Vector2 position;

        private float rotation;
        private float targetRotation;

        private float fadeIn;
        private float menuFadeIn;

        public float GetFadeIn => fadeIn;
        public float GetMenuFadeIn => menuFadeIn;

        private bool dragging;
        private Vector2 dragOffset;

        private Vector2 MenuPosition => position + new Vector2(((60 + 44 * menuFadeIn) * fadeIn + 5 * Math.Max(Main.LocalPlayer.WarpPlayer().actions.Count - 5, 0)) * direction, 4);
        private Vector2 MenuButtonPosition => position + new Vector2((26 + 50 * fadeIn + 5 * Math.Max(Main.LocalPlayer.WarpPlayer().actions.Count - 5, 0)) * direction, 0);

        private bool HoveringOverDragButton => Main.MouseScreen.Distance(position) < 16 && Usable || dragging;
        private bool HoveringOverMenuArrow => Main.MouseScreen.Distance(MenuButtonPosition) < 16 && Usable && Main.LocalPlayer.WarpPlayer().CurrentAction.MenuElements.Count > 0;

        private bool oldHover;
        private int scrollCD;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Player player = Main.LocalPlayer;
            WarpinatorAction action = player.WarpPlayer().CurrentAction;

            fadeIn = Math.Clamp(MathHelper.Lerp(fadeIn, Open ? 1f : 0, 0.15f), 0, 1);
            menuFadeIn = Math.Clamp(MathHelper.Lerp(menuFadeIn, MenuOpen ? fadeIn : 0, 0.2f), 0, 1);
            targetRotation = -MathHelper.TwoPi / player.WarpPlayer().actions.Count * player.WarpPlayer().currentActionIndex - ((1f - fadeIn) * MathHelper.Pi * (Open ? -1 : 1) * direction);
            rotation = Utils.AngleLerp(rotation, targetRotation + (UISettings.Lefty ? 0 : MathHelper.Pi), 0.3f);

            if (fadeIn > 0.98f)
                fadeIn = 1f;
            if (fadeIn < 0.2f && !Open)
                fadeIn = 0f;           
            
            if (menuFadeIn > 0.98f)
                menuFadeIn = 1f;
            if (menuFadeIn < 0.2f && !MenuOpen)
                menuFadeIn = 0f;

            if (scrollCD > 0)
                scrollCD--;

            bool otherInterface = ((player.mouseInterface || player.lastMouseInterface) && !OnMyInterface) || Main.inFancyUI || Main.ingameOptionsWindow;
            bool condition1 = player.dead || Main.mouseItem.type > ItemID.None;
            bool condition2 = !player.HoldingWarpinator() || PlayerInput.LockGamepadTileUseButton || player.noThrow != 0 || Main.HoveringOverAnNPC || player.talkNPC != -1;
            if (otherInterface || condition1 || condition2)
            {
                Open = false;
                return;
            }

            OnMyInterface = false;

            if (player.WarpPlayer().mouseRight && !Main.SmartInteractShowingGenuine)
                OpenUI(!Open);

            if (!Usable)
                return;

            if (!dragging && HoveringOverDragButton && player.WarpPlayer().mouseLeftHold)
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

                VisualsSystem.HideCursor();
            }

            position = new Vector2((int)position.X, (int)position.Y);
            bool hoveringOverAnything = HoveringOverDragButton || HoveringOverMenuArrow;
            for (int i = 0; i < player.WarpPlayer().actions.Count; i++)
            {
                Vector2 actPosition = position - new Vector2((20 + 20 * fadeIn + 5 * Math.Max(player.WarpPlayer().actions.Count - 5, 0)), 0).RotatedBy((rotation + MathHelper.TwoPi / player.WarpPlayer().actions.Count * i) * direction);
                bool innerHover = Main.MouseScreen.Distance(actPosition) < 20;

                if (innerHover)
                {
                    if (!oldHover)
                        SoundEngine.PlaySound(AssetDirectory.Sounds_UI.MenuTick);
                    Main.instance.MouseTextHackZoom(player.WarpPlayer().actions[i].DisplayName.Value);
                    Main.mouseText = true;

                    hoveringOverAnything = true;
                    if (player.WarpPlayer().mouseLeft)
                    {
                        player.WarpPlayer().currentActionIndex = i;
                        SoundEngine.PlaySound(AssetDirectory.Sounds_UI.MenuPageSelect);
                    }

                    if (Player.GetMouseScrollDelta() != 0)
                    {
                        player.WarpPlayer().currentActionIndex += Player.GetMouseScrollDelta();
                        SoundEngine.PlaySound(AssetDirectory.Sounds_UI.MenuPageSelect);

                        if (player.WarpPlayer().currentActionIndex < 0)
                            player.WarpPlayer().currentActionIndex = player.WarpPlayer().actions.Count - 1;
                        if (player.WarpPlayer().currentActionIndex > player.WarpPlayer().actions.Count - 1)
                            player.WarpPlayer().currentActionIndex = 0;
                    }
                }
            }

            if (player.WarpPlayer().CurrentAction.MenuElements.Count > 0 && MenuUsable)
            {
                Vector2 menuPos = MenuPosition;

                for (int i = 0; i < action.MenuElements.Count; i++)
                {
                    float height = action.MenuElements[i].Height * fadeIn;
                    menuPos.Y -= height / 2f;
                }
                for (int i = 0; i < action.MenuElements.Count; i++)
                {
                    float height = action.MenuElements[i].Height * fadeIn;
                    action.MenuElements[i].Update(player, menuPos, Main.MouseScreen, direction);
                    menuPos.Y += height;
                }
            }

            if (HoveringOverMenuArrow && !oldHover)
                SoundEngine.PlaySound(AssetDirectory.Sounds_UI.MenuTick.WithVolumeScale(0.7f));

            if (HoveringOverDragButton && !oldHover)
                SoundEngine.PlaySound(AssetDirectory.Sounds_UI.MenuTick.WithVolumeScale(0.7f));

            oldHover = hoveringOverAnything;

            if (HoveringOverMenuArrow && player.WarpPlayer().mouseLeft)
            {
                MenuOpen = !MenuOpen;
                SoundEngine.PlaySound(MenuOpen ? AssetDirectory.Sounds_UI.MenuSmallOpen : AssetDirectory.Sounds_UI.MenuSmallClose);
            }

            if (hoveringOverAnything)
                scrollCD = 5;

            if (scrollCD > 0)
                player.WarpInterface();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            if (fadeIn < 0.05f)
                return;

            Player player = Main.LocalPlayer;
            WarpinatorAction action = player.WarpPlayer().CurrentAction;

            if (player.WarpPlayer().CurrentAction.MenuElements.Count > 0 && menuFadeIn > 0.05f)
            {
                bool paged = action.MenuElements.Any(n => n is IDoNotDrawBackBox);

                Vector2 menuPos = MenuPosition;
                
                float height = 0;
                float width = 0;
                for (int i = 0; i < action.MenuElements.Count; i++)
                {
                    height += action.MenuElements[i].Height * fadeIn;
                    width = Math.Max(width, action.MenuElements[i].Width);
                }

                menuPos.Y -= height / 2f;

                if (UISettings.BackBox && !paged)
                    WarpUtils.DrawPanel(spriteBatch, (int)menuPos.X - 10 - (int)(direction < 0 ? width : 0), (int)menuPos.Y - 10, (int)width + 20, (int)height + 20, MenuFadeColor);

                for (int i = 0; i < action.MenuElements.Count; i++)
                {
                    float drawHeight = action.MenuElements[i].Height * fadeIn;
                    action.MenuElements[i].Draw(spriteBatch, MenuFadeColor, player, menuPos, Main.MouseScreen, direction);
                    menuPos.Y += drawHeight;
                }
            }

            //Draw drag button in center

            float dragScale = Utils.GetLerpValue(0f, 0.7f, fadeIn, true) * (dragging ? 1.1f : 0.9f);
            spriteBatch.Draw(AssetDirectory.Textures_UI.DragButton[0], position, AssetDirectory.Textures_UI.DragButton[0].Texture.Frame(), Color.White, 0, AssetDirectory.Textures_UI.DragButton[0].Texture.Size() * 0.5f, dragScale, 0, 0);
            if (HoveringOverDragButton || dragging)
                spriteBatch.Draw(AssetDirectory.Textures_UI.DragButton[1], position, AssetDirectory.Textures_UI.DragButton[1].Texture.Frame(), Main.OurFavoriteColor, 0, AssetDirectory.Textures_UI.DragButton[1].Texture.Size() * 0.5f, dragScale, 0, 0);

            if (Main.LocalPlayer.WarpPlayer().CurrentAction.MenuElements.Count > 0)
            {
                float arrowScale = Utils.GetLerpValue(0.85f, 0.95f, fadeIn, true);
                SpriteEffects arrowEffect = direction > 0 ^ MenuOpen ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                spriteBatch.Draw(AssetDirectory.Textures_UI.ButtonArrow[0], MenuButtonPosition, AssetDirectory.Textures_UI.ButtonArrow[0].Texture.Frame(), Color.White, 0, AssetDirectory.Textures_UI.ButtonArrow[0].Texture.Size() * 0.5f, arrowScale, arrowEffect, 0);
                if (HoveringOverMenuArrow)
                    spriteBatch.Draw(AssetDirectory.Textures_UI.ButtonArrow[1], MenuButtonPosition, AssetDirectory.Textures_UI.ButtonArrow[1].Texture.Frame(), Main.OurFavoriteColor, 0, AssetDirectory.Textures_UI.ButtonArrow[1].Texture.Size() * 0.5f, arrowScale, arrowEffect, 0);
            }

            Color iconColor = Color.Lerp(Color.Magenta, Color.White, Utils.GetLerpValue(0.2f, 0.9f, fadeIn, true));

            for (int i = 0; i < player.WarpPlayer().actions.Count; i++)
            {
                Vector2 actPosition = position - new Vector2(20 + 20 * fadeIn + 5 * Math.Max(player.WarpPlayer().actions.Count - 5, 0), 0).RotatedBy((rotation + MathHelper.TwoPi / player.WarpPlayer().actions.Count * i) * direction).ToPoint().ToVector2();
                float actScale = MathF.Sqrt(Utils.GetLerpValue(0.1f, 1f, fadeIn, true));

                bool hoverOrSelected = (Main.MouseScreen.Distance(actPosition) < 20 || i == player.WarpPlayer().currentActionIndex) && Usable;

                Texture2D texture = AssetDirectory.Textures_UI.FlowerButton[hoverOrSelected ? 1 : 0];
                spriteBatch.Draw(texture, actPosition, texture.Frame(), Color.White, 0, texture.Size() * 0.5f, actScale, 0, 0);
                player.WarpPlayer().actions[i].DrawIcon(spriteBatch, actPosition, iconColor, actScale);
            }

            if (UISettings.Name)
            {
                float fade = (UISettings.NamePerm ? 1f : menuFadeIn) * fadeIn;
                string name = player.WarpPlayer().CurrentAction.DisplayName.Value;
                Vector2 namePosition = position + new Vector2(-(40 + 40 * fade + 5 * Math.Max(player.WarpPlayer().actions.Count - 5, 0) + (direction > 0 ? FontAssets.MouseText.Value.MeasureString(name).X : 0)) * direction, -10);
                Utils.DrawBorderString(spriteBatch, name, namePosition, Color.Lerp(new Color(180, 0, 200, 200), Color.White, Utils.GetLerpValue(0.7f, 0.85f, fade, true)) * Utils.GetLerpValue(0.5f, 0.9f, fade, true), 1f);
            }
        }
    }
}