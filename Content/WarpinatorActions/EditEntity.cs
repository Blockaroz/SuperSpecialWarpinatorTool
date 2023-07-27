using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperSpecialWarpinatorTool.Common.Systems;
using SuperSpecialWarpinatorTool.Content.WarpMenuElements;
using SuperSpecialWarpinatorTool.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace SuperSpecialWarpinatorTool.Content.WarpinatorActions
{
    public class EditEntity : WarpinatorAction
    {
        private Ref<NPC> npc = new Ref<NPC>();
        private Ref<Projectile> projectile = new Ref<Projectile>();

        private string oldSearch;
        private Ref<string> search = new Ref<string>();

        private int movedNPC;
        private int movedProj;
        private bool movingSomething;

        public override void SetDefaults()
        {
            npc.Value = null;
            search.Value = "";
        }

        public override void Perform(Player player, Item item)
        {
            player.SetDummyItemTime(5);

            if (!movingSomething)
            {
                if (Main.npc.Any(n => n.active && n.Hitbox.Contains(Main.MouseWorld.ToPoint())))
                {
                    movingSomething = true;
                    movedNPC = Main.npc.First(n => n.active && n.Hitbox.Contains(Main.MouseWorld.ToPoint())).whoAmI;
                }
            }
        }

        public override void Update(Player player)
        {
            oldSearch = search.Value;

            if (Selected && !player.mouseInterface)
            {
                player.WarpPlayer().useSpecialCursor = true;

                if (player.controlUseItem)
                    player.ChangeDir(Main.MouseWorld.X > player.Center.X ? 1 : -1);
            }

            if (!player.ItemAnimationActive && movingSomething)
            {
                movedNPC = -1;
                movedProj = -1;
                movingSomething = false;
            }
        }

        private void RefreshFieldSearch()
        {

        }

        public static Texture2D[] icons;

        public override void OnLoad(Mod mod)
        {
            icons = new Texture2D[2];
        }

        public override List<IWarpMenuElement> AddMenuElements() => new List<IWarpMenuElement>()
        {
            new PageList(new List<Page>()
            {
                new Page(Mod, "Common.NPCs", icons[0], new List<IWarpMenuElement>()
                {
                    new Selector<NPC>(npc),
                }),
                new Page(Mod, "Common.Projectiles", icons[1], new List<IWarpMenuElement>()
                {                    
                    new Selector<Projectile>(projectile),                   
                }),
            })
        };
    }
}
