using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public override void SetDefaults()
        {
            npc.Value = null;
            search.Value = "";
        }

        public override void Perform(Player player)
        {
            
        }

        public override void Update(Player player)
        {
            oldSearch = search.Value;
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
                new Page(Mod, "NPCs", icons[0], new List<IWarpMenuElement>()
                {
                    new Text(Mod, "Test", Color.Orchid, 0.5f),
                    new Selector<NPC>(npc),
                }),
                new Page(Mod, "Projectiles", icons[1], new List<IWarpMenuElement>()
                {                    
                    new Text(Mod, "Test", Color.Cyan, 0.5f),
                    new Selector<Projectile>(projectile),                   
                }),
            })
        };
    }
}
