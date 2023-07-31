using Microsoft.Xna.Framework;
using SuperSpecialWarpinatorTool.Common.UI;
using SuperSpecialWarpinatorTool.Content.MenuElements;
using SuperSpecialWarpinatorTool.Core;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.Localization;
using static SuperSpecialWarpinatorTool.Common.UI.WarpUI;

namespace SuperSpecialWarpinatorTool.Content.WarpinatorActions
{
    public class Settings : WarpinatorAction
    {
        public override string Texture => AssetDirectory.TexturePath + "WarpinatorActions/Settings";

        public override bool HasPerformableAction => false;

        public Ref<bool> lefty = new Ref<bool>();
        public Ref<bool> backBox = new Ref<bool>();
        public Ref<bool> name = new Ref<bool>();
        public Ref<bool> namePermanent = new Ref<bool>();
        public Ref<bool> tileGrid = new Ref<bool>();

        private Ref<List<string>> cursorOptions = new Ref<List<string>>();
        public Ref<int> cursorMode = new Ref<int>();
        public Ref<bool> cursorColor = new Ref<bool>();
        public Ref<bool> cursorWires = new Ref<bool>();

        public override void SetDefaults()
        {
            lefty.Value = false;
            backBox.Value = false;
            name.Value = true;
            namePermanent.Value = false;
            tileGrid.Value = false;

            cursorOptions.Value = new List<string>
            {
                Language.GetOrRegister(Mod.GetLocalizationKey("Common.Default")).Value,
                Language.GetOrRegister(Mod.GetLocalizationKey("Common.Never")).Value,
                Language.GetOrRegister(Mod.GetLocalizationKey("Common.Always")).Value
            };
            cursorMode.Value = (int)OptionEnum.Default;
            cursorColor.Value = true;
            cursorWires.Value = true;
        }

        public override void Update(Player player)
        {
            if (!name.Value)
                name.Value |= namePermanent.Value;

            UISettings.Lefty = lefty.Value;
            UISettings.BackBox = backBox.Value;
            UISettings.NamePerm = namePermanent.Value;
            UISettings.Name = name.Value || namePermanent.Value;
            UISettings.TileGrid = tileGrid.Value;
            UISettings.CursorWires = cursorWires.Value;
            UISettings.CursorMode = cursorMode.Value switch
            {
                0 => OptionEnum.Default,
                1 => OptionEnum.Never,
                2 => OptionEnum.Always,
                _ => OptionEnum.Default,
            };
            UISettings.CursorMouseColor = cursorColor.Value;
        }

        public override List<IMenuElement> AddMenuElements() => new List<IMenuElement>
        {
            new PageList(new List<Page>()
            {
                new Page(Mod, "WarpinatorMenus.Pages.MenuSettings", AssetDirectory.Textures_UI.Pages.Settings, new List<IMenuElement>()
                {
                    new Text(Mod, "WarpinatorMenus.Settings.MenuOnLeft"),
                    new Toggle(lefty),
                    new Text(Mod, "WarpinatorMenus.Settings.MenuBackBoxes"),
                    new Toggle(backBox),
                    new Text(Mod, "WarpinatorMenus.Settings.ShowName"),
                    new Toggle(name),
                    new Text(Mod, "Common.Always", Color.DarkGray, 0.66f),
                    new Toggle(namePermanent),                    
                    new Text(Mod, "WarpinatorMenus.Settings.TileGrid"),
                    new Toggle(tileGrid),
                }),                
                new Page(Mod, "WarpinatorMenus.Pages.CursorSettings", AssetDirectory.Textures_UI.Pages.Cursor, new List<IMenuElement>()
                {
                    new Text(Mod, "WarpinatorMenus.Settings.DisplaySpecialCursor", Color.DarkGray, 0.66f),
                    new ScrollPanel(cursorOptions, cursorMode, 60, 60),
                    new Text(Mod, "WarpinatorMenus.Settings.UseCursorColor", Color.DarkGray, 0.66f),
                    new Toggle(cursorColor),                    
                    new Text(Mod, "WarpinatorMenus.Settings.ShowCursorWires", Color.DarkGray, 0.66f),
                    new Toggle(cursorWires),
                }),
            })
        };
    }
}
