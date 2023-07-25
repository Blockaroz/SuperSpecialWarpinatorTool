using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SuperSpecialWarpinatorTool.Common.UI;
using SuperSpecialWarpinatorTool.Core;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SuperSpecialWarpinatorTool.Content.WarpMenuElements
{
    public class PageList : IWarpMenuElement
    {
        private List<Page> pages;

        private int activePage;

        private readonly int height;

        private bool oldHover;

        public PageList(List<Page> pages)
        {
            this.pages = pages;
            foreach (Page page in this.pages)
                height += page.Height;
        }

        public int Height => (int)(height * (0.5f + WarpinatorUISystem.WarpinatorUI.GetMenuFadeIn * 0.5f / WarpinatorUISystem.WarpinatorUI.GetFadeIn));

        public void Draw(SpriteBatch spriteBatch, Color color, Player player, Vector2 position, Vector2 mousePos, int direction)
        {
            pages[activePage].Draw(spriteBatch, color, player, position + new Vector2(35 * direction, 0), mousePos, direction);

            Vector2 buttonPos = position + new Vector2(6 * direction, 11);
            foreach (Page page in pages)
            {
                bool hoveringOrActive = mousePos.Distance(buttonPos) < 13 || page.active;

                Texture2D texture = AssetDirectory.Textures_UI.PageButton[hoveringOrActive ? 1 : 0];
                spriteBatch.Draw(texture, buttonPos, texture.Frame(), color, 0, texture.Size() * 0.5f, 1f, 0, 0);

                if (page.icon != null)
                    spriteBatch.Draw(page.icon, buttonPos, page.icon.Frame(), color, 0, page.icon.Size() * 0.5f, 1f, 0, 0);

                buttonPos.Y += page.Height * WarpinatorUISystem.WarpinatorUI.GetMenuFadeIn * WarpinatorUISystem.WarpinatorUI.GetFadeIn;
            }
        }

        public void Update(Player player, Vector2 position, Vector2 mousePos, int direction)
        {
            pages[activePage].Update(player, position + new Vector2(35 * direction, 0), mousePos, direction);

            bool hovering = false;
            Vector2 buttonPos = position + new Vector2(6 * direction, 11);
            for (int i = 0; i < pages.Count; i++)
            {
                bool innerHover = mousePos.Distance(buttonPos) < 13;
                if (innerHover)
                {
                    player.WarpInterface();
                    hovering = true;

                    Main.instance.MouseTextHackZoom(pages[i].pageName.Value);
                    Main.mouseText = true;

                    if (player.WarpPlayer().mouseLeft)
                    {
                        SoundEngine.PlaySound(AssetDirectory.Sounds_UI.MenuTickSelect);
                        activePage = i;
                    }
                }

                pages[i].active = activePage == i;

                buttonPos.Y += pages[i].Height * WarpinatorUISystem.WarpinatorUI.GetMenuFadeIn * WarpinatorUISystem.WarpinatorUI.GetFadeIn;
            }

            if (hovering && !oldHover)
                SoundEngine.PlaySound(AssetDirectory.Sounds_UI.MenuTick);

            oldHover = hovering;
        }
    }

    public class Page : ILocalizedModType
    {
        public string LocalizationCategory => "WarpinatorMenuElements";

        public Mod Mod { get; private set; }

        public string Name => GetType().Name;

        public string FullName => GetType().FullName;

        public List<IWarpMenuElement> elements;

        public readonly Texture2D icon;

        public readonly LocalizedText pageName;

        internal bool active;

        public Page(Mod mod, string key, Texture2D icon, List<IWarpMenuElement> elements)
        {
            pageName = Language.GetOrRegister(mod.GetLocalizationKey(LocalizationCategory + '.' + key));
            this.icon = icon;
            this.elements = elements;
        }

        public int Height => 30;

        public void Draw(SpriteBatch spriteBatch, Color color, Player player, Vector2 position, Vector2 mousePos, int direction)
        {
            if (active)
            {
                Vector2 menuPos = position;
                foreach (IWarpMenuElement element in elements)
                    menuPos.Y -= (int)(element.Height / 2f);
                foreach (IWarpMenuElement element in elements)
                {
                    element.Draw(spriteBatch, color, player, menuPos, mousePos, direction);
                    menuPos.Y += element.Height;
                }
            }
        }

        public void Update(Player player, Vector2 position, Vector2 mousePos, int direction)
        {
            if (active)
            {
                Vector2 menuPos = position;
                foreach (IWarpMenuElement element in elements)
                    menuPos.Y -= (int)(element.Height / 2f);
                foreach (IWarpMenuElement element in elements)
                {
                    element.Update(player, menuPos, mousePos, direction);
                    menuPos.Y += element.Height;
                }
            }
        }
    }
}
