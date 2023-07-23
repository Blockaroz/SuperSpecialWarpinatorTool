using Microsoft.Xna.Framework;
using SuperSpecialWarpinatorTool.Content.WarpMenuElements;
using SuperSpecialWarpinatorTool.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace SuperSpecialWarpinatorTool.Content.WarpinatorActions
{
    public class Move : WarpinatorAction
    {
        public Ref<NPC> selection = new Ref<NPC>();
        private Ref<bool> active = new Ref<bool>();

        public override void SetDefaults()
        {
            active.Value = false;
        }

        public override void Perform(Player player)
        {
        }
    }
}
