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
    public class Butcher : WarpinatorAction
    {
        public Ref<bool> despawn = new Ref<bool>();

        public override void SetDefaults()
        {
            despawn.Value = false;
        }

        public override void Perform(Player player)
        {
            if (Main.npc.Any(n => n.Hitbox.Contains(Main.MouseWorld.ToPoint())))
            {
                NPC target = Main.npc.First(n => n.Hitbox.Contains(Main.MouseWorld.ToPoint()));

                target.life = 0;

                if (despawn.Value)
                    target.active = false;
                else
                    target.checkDead();

                return;
            }
        }

        public override List<IWarpMenuElement> AddMenuElements() => new List<IWarpMenuElement>()
        {
            new Text(Mod, "DespawnNPC"),
            new Toggle(despawn),
        };
    }
}
