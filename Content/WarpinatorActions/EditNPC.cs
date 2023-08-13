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
    public class EditNPC : WarpinatorAction
    {
        private Ref<bool> shakeToButcher = new Ref<bool>();
        private Ref<bool> butcherDespawn = new Ref<bool>();

        private Ref<int> npc = new Ref<int>();

        private string oldSearch;
        private Ref<string> search = new Ref<string>();

        private int movedNPC;
        private bool movingSomething;
        private Vector2 offset;

        public override void SetDefaults()
        {
            shakeToButcher.Value = false;
            npc.Value = -1;
            search.Value = "";
        }

        public override void Perform(Player player, Item item)
        {
            if (!movingSomething)
            {
                movedNPC = -1;

                if (Main.npc.Any(n => n.active && n.Hitbox.Contains(Main.MouseWorld.ToPoint())))
                {
                    movingSomething = true;
                    movedNPC = Main.npc.First(n => n.active && n.Hitbox.Contains(Main.MouseWorld.ToPoint())).whoAmI;
                    offset = Main.npc[movedNPC].Center - Main.MouseWorld;
                    return;
                }
            }
        }

        public override void Update(Player player)
        {
            oldSearch = search.Value;

            if (npc.Value > -1)
            {
            }

            if (Selected && !player.mouseInterface)
            {
                if (player.controlUseItem)
                    player.ChangeDir(Main.MouseWorld.X > player.Center.X ? 1 : -1);
            }

            offset = Vector2.Lerp(offset, Vector2.Zero, 0.1f);

            if (movingSomething && movedNPC > -1)
            {
                Main.npc[movedNPC].Center = Main.MouseWorld - offset;

                if (shakeToButcher.Value && WarpUtils.MouseShaking)
                {
                    Main.npc[movedNPC].life = 0;

                    if (butcherDespawn.Value)
                        Main.npc[movedNPC].active = false;
                    else
                        Main.npc[movedNPC].checkDead();

                }
            }

            if (!player.ItemAnimationActive && movingSomething)
            {
                movedNPC = -1;
                movingSomething = false;
            }
        }

        private void RefreshFieldSearch()
        {

        }

        private void KillCurrentNPC()
        {
            if (npc.Value > -1)
            {
                Main.npc[npc.Value].life = 0;

                if (butcherDespawn.Value)
                    Main.npc[npc.Value].active = false;
                else
                    Main.npc[npc.Value].checkDead();
            }
        }

        public override List<IMenuElement> AddMenuElements() => new List<IMenuElement>()
        {
            new PageList(new List<Page>()
            {
                new Page(Mod, "Common.ToolSettings", AssetDirectory.Textures.Pages.Settings, new List<IMenuElement>()
                {
                }),                
                new Page(Mod, "WarpinatorMenus.Pages.ButcherSettings", AssetDirectory.Textures.Pages.Butcher, new List<IMenuElement>()
                {
                    new Text(Mod, "WarpinatorMenus.Butcher.ShakeToButcher"),
                    new Toggle(shakeToButcher),
                    new Text(Mod, "WarpinatorMenus.Butcher.DespawnNPC"),
                    new Toggle(butcherDespawn),
                }),
                new Page(Mod, "Common.Edit", AssetDirectory.Textures.Pages.Cursor, new List<IMenuElement>()
                {
                    new NPCSelector(npc, ref Main.npc),
                    new Button(KillCurrentNPC, new Text(Mod, "Common.Butcher"))
                }),
            })
        };
    }
}
