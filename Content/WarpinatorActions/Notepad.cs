using SuperSpecialWarpinatorTool.Content.MenuElements;
using SuperSpecialWarpinatorTool.Core;
using System.Collections.Generic;
using Terraria;

namespace SuperSpecialWarpinatorTool.Content.WarpinatorActions
{
    public class Notepad : WarpinatorAction, IDoNotAutosave
    {
        public override void SetDefaults()
        {
        }

        public override List<IMenuElement> AddMenuElements() => new List<IMenuElement>()
        {
            new StickyNoteBoard()
        };
    }
}
