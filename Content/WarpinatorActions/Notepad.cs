using SuperSpecialWarpinatorTool.Core;
using System.Collections.Generic;
using Terraria;

namespace SuperSpecialWarpinatorTool.Content.WarpinatorActions
{
    public class Notepad : WarpinatorAction, IDoNotAutosave
    {
        public Ref<string> notes = new Ref<string>();

        public override void SetDefaults()
        {
            notes.Value = "";
        }

        public override List<IWarpMenuElement> AddMenuElements() => new List<IWarpMenuElement>()
        {

        };
    }
}
