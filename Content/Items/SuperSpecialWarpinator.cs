using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SuperSpecialWarpinatorTool.Common.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperSpecialWarpinatorTool.Content.Items
{
    public class SuperSpecialWarpinator : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 28;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.channel = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.Purple;
            Item.autoReuse = true;
            Item.useTurn = false;
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
                player.WarpPlayer().CurrentAction.Perform(player);

            return true;
        }

        public override bool CanUseItem(Player player) => player.WarpPlayer().CurrentAction.HasPerformableAction;

        public override Color? GetAlpha(Color lightColor) => Color.Lerp(lightColor, Color.White, 0.5f);
    }
}
