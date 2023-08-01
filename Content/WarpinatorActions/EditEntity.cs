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
        public override string Texture => AssetDirectory.TexturePath + "WarpinatorActions/EditEntity";

        private Ref<bool> shakeToButcher = new Ref<bool>();
        private Ref<int> npc = new Ref<int>();
        private Ref<int> projectile = new Ref<int>();

        private string oldSearch;
        private Ref<string> search = new Ref<string>();

        private int movedNPC;
        private int movedProj;
        private bool movingSomething;
        private Vector2 offset;

        public override void SetDefaults()
        {
            shakeToButcher.Value = false;
            npc.Value = -1;
            projectile.Value = -1;
            search.Value = "";
        }

        public override void Perform(Player player, Item item)
        {
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

            if (npc.Value > -1)
            {
                //Main.NewText(player.GetSource_CatchEntity(Main.npc[npc.Value]).ToString());
            }

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
                    Main.npc[movedNPC].Center = Main.MouseWorld + offset;

                if (movedProj > -1)
                    Main.projectile[movedProj].Center = Main.MouseWorld + offset;

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

        public override List<IMenuElement> AddMenuElements() => new List<IMenuElement>()
        {
            new PageList(new List<Page>()
            {
                new Page(Mod, "Common.NPCs", AssetDirectory.Textures_UI.Pages.NPCs, new List<IMenuElement>()
                {
                    new EntitySelector<NPC>(npc, ref Main.npc)
                }),
                new Page(Mod, "Common.Projectiles", AssetDirectory.Textures_UI.Pages.Projectiles, new List<IMenuElement>()
                {                    
                    new EntitySelector<Projectile>(projectile, ref Main.projectile)
                }),
            })
        };
    }
}
