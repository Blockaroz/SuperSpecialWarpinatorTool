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
        public Ref<bool> world = new Ref<bool>();

        public override void SetDefaults()
        {
            despawn.Value = false;
        }

        public override void Update(Player player)
        {
            if (Selected && !player.mouseInterface)
            {
                player.WarpPlayer().useSpecialCursorWireHands = true;
                if (player.controlUseItem)
                    player.ChangeDir(Main.MouseWorld.X > player.Center.X ? 1 : -1);
            }
        }

        public override void Perform(Player player, Item item)
        {
            if (world.Value)
            {
                foreach (NPC npc in Main.npc.Where(n => n.active))
                {
                    npc.life = 0;

                    if (despawn.Value)
                        npc.active = false;
                    else
                        npc.checkDead();
                }
            }
            else if (Main.npc.Any(n => n.active && n.Hitbox.Contains(Main.MouseWorld.ToPoint())))
            {
                NPC target = Main.npc.First(n => n.active && n.Hitbox.Contains(Main.MouseWorld.ToPoint()));

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
            new Text(Mod, "WarpinatorMenus.Butcher.DespawnNPC"),
            new Toggle(despawn),
            new Text(Mod, "WarpinatorMenus.Butcher.AffectAllNPCs"),
            new Toggle(world),
        };
    }
}
