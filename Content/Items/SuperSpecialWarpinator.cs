using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperSpecialWarpinatorTool.Content.Items
{
    public class SuperSpecialWarpinator : ModItem
    {
        public override string Texture => Mod.Name + "/Assets/Textures/Items/SuperSpecialWarpinator";

        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 2;
            Item.useTime = 2;
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
                player.WarpPlayer().CurrentAction.Perform(player, Item);

            return true;
        }

        public override bool CanUseItem(Player player) => player.WarpPlayer().CurrentAction.HasPerformableAction;

        public override Color? GetAlpha(Color lightColor) => Color.Lerp(lightColor, Color.White, 0.5f);

        public override void EquipFrameEffects(Player player, EquipType type)
        {
            player.handon = -1;
            player.handoff = -1;
        }
    }
}
