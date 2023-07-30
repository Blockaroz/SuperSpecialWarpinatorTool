using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperSpecialWarpinatorTool.Common.Systems;
using SuperSpecialWarpinatorTool.Content.MenuElements;
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
        private Vector2 offset;

        public override void SetDefaults()
        {
            npc.Value = null;
            search.Value = "";
        }

        public override void Perform(Player player, Item item)
        {
            item.useTime = 2;
            item.useAnimation = 2;

            if (!movingSomething)
            {
                movedNPC = -1;
                movedProj = -1;

                if (Main.npc.Any(n => n.active && n.Hitbox.Contains(Main.MouseWorld.ToPoint())))
                {
                    movingSomething = true;
                    movedNPC = Main.npc.First(n => n.active && n.Hitbox.Contains(Main.MouseWorld.ToPoint())).whoAmI;
                    offset = Main.MouseWorld - Main.npc[movedNPC].Center;
                    return;
                }

                if (Main.projectile.Any(n => n.active && n.Hitbox.Contains(Main.MouseWorld.ToPoint())))
                {
                    movingSomething = true;
                    movedProj = Main.projectile.First(n => n.active && n.Hitbox.Contains(Main.MouseWorld.ToPoint())).whoAmI;
                    offset = Main.MouseWorld - Main.projectile[movedProj].Center;
                    return;
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

            offset = Vector2.Lerp(offset, Vector2.Zero, 0.1f);

            if (movingSomething)
            {
                if (movedNPC > -1)
                {
                    if (npc.Value == null)
                        npc.Value = Main.npc[movedNPC];

                    else
                        npc.Value.Center = Main.MouseWorld + offset;
                }
                if (movedProj > -1)
                {
                    if (projectile.Value == null)
                        projectile.Value = Main.projectile[movedProj];

                    else
                        projectile.Value.Center = Main.MouseWorld + offset;
                }
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

        public override List<IMenuElement> AddMenuElements() => new List<IMenuElement>()
        {
            new PageList(new List<Page>()
            {
                new Page(Mod, "Common.NPCs", icons[0], new List<IMenuElement>()
                {
                    new EntitySelector<NPC>(npc, Main.npc)
                }),
                new Page(Mod, "Common.Projectiles", icons[1], new List<IMenuElement>()
                {                    
                    new EntitySelector<Projectile>(projectile, Main.projectile)
                }),
            })
        };
    }
}
