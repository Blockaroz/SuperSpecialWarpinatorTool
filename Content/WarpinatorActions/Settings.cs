using Microsoft.Xna.Framework;
using SuperSpecialWarpinatorTool.Common.UI;
using SuperSpecialWarpinatorTool.Content.WarpMenuElements;
using SuperSpecialWarpinatorTool.Core;
using System.Collections.Generic;
using Terraria;

namespace SuperSpecialWarpinatorTool.Content.WarpinatorActions
{
    public class Settings : WarpinatorAction
    {
        public override bool HasPerformableAction => false;

        public Ref<bool> lefty = new Ref<bool>();
        public Ref<bool> showName = new Ref<bool>();
        public Ref<bool> showNamePerm = new Ref<bool>();

        public override void SetDefaults()
        {
            lefty.Value = false;
            showName.Value = true;
            showNamePerm.Value = false;
        }

        public override void Update(Player player)
        {
            if (showName.Value)
                showNamePerm.Value &= showName.Value;
            else
                showName.Value |= showNamePerm.Value;

            WarpinatorUISystem.WarpinatorUI.Lefty = lefty.Value;
            WarpinatorUISystem.WarpinatorUI.ShowNamePermanent = showNamePerm.Value;
            WarpinatorUISystem.WarpinatorUI.ShowName = showName.Value || showNamePerm.Value;
        }

        public override List<IWarpMenuElement> AddMenuElements() => new List<IWarpMenuElement>
        {
            new Text(Mod, "MenuOnLeft"),
            new Toggle(lefty),
            new Text(Mod, "ShowName"),
            new Toggle(showName, true),
            new Text(Mod, "Permanent", Color.DimGray * 0.7f, 0.7f),
            new Toggle(showNamePerm, true),
        };
    }
}
