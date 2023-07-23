using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace SuperSpecialWarpinatorTool.Common.UI
{
    public class WarpinatorUISystem : ModSystem
    {
        internal static UserInterface WarpinatorInterface;
        internal static WarpUI WarpinatorUI;

        public override void OnModLoad()
        {
            WarpinatorUI = new WarpUI();
            WarpinatorUI.Activate();
            WarpinatorInterface = new UserInterface();
            WarpinatorInterface.SetState(WarpinatorUI);
        }

        public override void Unload()
        {
            WarpinatorInterface = null;
            WarpinatorUI = null;
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (Main.LocalPlayer.WarpPlayer().actions != null)
                WarpinatorUI.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Wire Selection"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "SuperSpecialWarpinator: UI",
                    delegate {
                        if (WarpinatorInterface.CurrentState != null && Main.LocalPlayer.WarpPlayer().actions != null)
                            WarpinatorInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}
