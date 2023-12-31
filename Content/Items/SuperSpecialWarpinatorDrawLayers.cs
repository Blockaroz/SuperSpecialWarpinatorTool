﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperSpecialWarpinatorTool.Common.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace SuperSpecialWarpinatorTool.Content.Items
{
    public class SuperSpecialWarpinatorHandsOn : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.HandOnAcc);

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.HoldingWarpinator() || (drawInfo.drawPlayer.WarpPlayer().alwaysShowVisuals && drawInfo.drawPlayer.HasWarpinator());

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            if (drawInfo.drawPlayer.WarpPlayer().useSpecialCursorWireHands && drawInfo.drawPlayer.HoldingWarpinator())
            {
                Texture2D line = TextureAssets.BlackTile.Value;
                float thicknessFix = 1.3f;// / Main.UIScale * Main.GameZoomTarget;
                Vector2 backHandPos = drawInfo.drawPlayer.GetBackHandPosition(Player.CompositeArmStretchAmount.Full, Main.LocalPlayer.compositeBackArm.rotation) - Main.screenPosition;
                Vector2 frontHandPos = drawInfo.drawPlayer.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, Main.LocalPlayer.compositeFrontArm.rotation) - Main.screenPosition;
                Vector2 interHandLength = new Vector2(thicknessFix, frontHandPos.Distance(backHandPos)  + 1);
                drawInfo.DrawDataCache.Add(new DrawData(line, frontHandPos, new Rectangle(0, 0, 2, 1), WarpUtils.WarpColor(), frontHandPos.AngleTo(backHandPos) - MathHelper.PiOver2, Vector2.UnitX, interHandLength, 0, 0));
            }

            Texture2D texture = AssetDirectory.Textures.WarpinatorHandsOn;

            Vector2 position = drawInfo.BodyPosition() + drawInfo.frontShoulderOffset + VanityUtils.GetCompositeOffset_FrontArm(ref drawInfo);
            position.ApplyVerticalOffset(drawInfo);
            Vector2 origin = drawInfo.bodyVect + drawInfo.frontShoulderOffset + VanityUtils.GetCompositeOffset_FrontArm(ref drawInfo);
            if (drawInfo.compFrontArmFrame.X / drawInfo.compFrontArmFrame.Width >= 7)
                position += new Vector2((!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally)) ? 1 : (-1), (!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipVertically)) ? 1 : (-1));

            DrawData drawData = new DrawData(texture, position, drawInfo.compFrontArmFrame, Color.White * (1f - drawInfo.shadow), drawInfo.compositeFrontArmRotation, origin, 1f, drawInfo.playerEffect, 0);
            drawInfo.DrawDataCache.Add(drawData);
        }
    }

    public class SuperSpecialWarpinatorHandsOff : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.OffhandAcc);

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.HoldingWarpinator() || (drawInfo.drawPlayer.WarpPlayer().alwaysShowVisuals && drawInfo.drawPlayer.HasWarpinator());

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Texture2D texture = AssetDirectory.Textures.WarpinatorHandsOff;

            Vector2 position = drawInfo.BodyPosition() + drawInfo.backShoulderOffset + VanityUtils.GetCompositeOffset_BackArm(ref drawInfo);
            position.ApplyVerticalOffset(drawInfo);
            Vector2 origin = drawInfo.bodyVect + drawInfo.backShoulderOffset + VanityUtils.GetCompositeOffset_BackArm(ref drawInfo);
            DrawData drawData = new DrawData(texture, position, drawInfo.compBackArmFrame, Color.White * (1f - drawInfo.shadow), drawInfo.compositeBackArmRotation, origin, 1f, drawInfo.playerEffect, 0);
            drawInfo.DrawDataCache.Add(drawData);
        }
    }    
    
    public class SuperSpecialWarpinatorTank : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Backpacks);

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.HoldingWarpinator() || (drawInfo.drawPlayer.WarpPlayer().alwaysShowVisuals && drawInfo.drawPlayer.HasWarpinator());// && VanityUtils.NoBackpackOn(ref drawInfo);

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Texture2D backpackTexture = AssetDirectory.Textures.WarpinatorTank;

            Vector2 vec5 = drawInfo.BodyPosition() + new Vector2(-12 * drawInfo.drawPlayer.direction, -2 * drawInfo.drawPlayer.gravDir);
            vec5 = vec5.Floor();
            vec5.ApplyVerticalOffset(drawInfo);

            DrawData item = new DrawData(backpackTexture, vec5, backpackTexture.Frame(), Color.White * (1f - drawInfo.shadow), drawInfo.drawPlayer.bodyRotation, backpackTexture.Size() * 0.5f, 1f, drawInfo.playerEffect);
            drawInfo.DrawDataCache.Add(item);
        }
    }
}
