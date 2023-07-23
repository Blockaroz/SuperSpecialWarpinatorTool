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
        public Ref<bool> name = new Ref<bool>();
        public Ref<bool> namePermanent = new Ref<bool>();
        public Ref<bool> selectionWires = new Ref<bool>();
        public Ref<bool> entityHitboxes = new Ref<bool>();

        public override void SetDefaults()
        {
            lefty.Value = false;
            name.Value = true;
            namePermanent.Value = false;
            selectionWires.Value = true;
            entityHitboxes.Value = true;
        }

        public override void Update(Player player)
        {
            if (!name.Value)
                name.Value |= namePermanent.Value;

            WarpUI.UISettings.Lefty = lefty.Value;
            WarpUI.UISettings.NamePerm = namePermanent.Value;
            WarpUI.UISettings.Name = name.Value || namePermanent.Value;
            WarpUI.UISettings.SelectionWires = selectionWires.Value;
            WarpUI.UISettings.EntityHitboxes = entityHitboxes.Value;
        }

        public override List<IWarpMenuElement> AddMenuElements() => new List<IWarpMenuElement>
        {
            new Text(Mod, "MenuOnLeft"),
            new Toggle(lefty),
            new Text(Mod, "ShowName"),
            new Toggle(name),
            new Text(Mod, "Permanent", Color.DarkGray, 0.66f),
            new Toggle(namePermanent),
        };
    }
}
