using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SuperSpecialWarpinatorTool.Common.UI;
using SuperSpecialWarpinatorTool.Content.WarpinatorActions;
using SuperSpecialWarpinatorTool.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameInput;
using Terraria.IO;
using Terraria.ModLoader;

namespace SuperSpecialWarpinatorTool.Common.Systems
{
    public class WarpinatorPlayer : ModPlayer
    {
        public List<WarpinatorAction> actions;
        public int currentActionIndex;

        public WarpinatorAction CurrentAction => actions[currentActionIndex];

        public bool useSpecialCursor;
        public bool useSpecialCursorWireHands;

        public override void Load()
        {
            On_Player.SavePlayer += SaveSettings;
        }

        public override void OnEnterWorld()
        {
            actions = new List<WarpinatorAction>
            {
                SuperSpecialWarpinatorTool.ActionType<Settings>(),
                SuperSpecialWarpinatorTool.ActionType<Light>(),
                SuperSpecialWarpinatorTool.ActionType<Butcher>(),
                SuperSpecialWarpinatorTool.ActionType<EditEntity>(),
                SuperSpecialWarpinatorTool.ActionType<Notepad>(),
            };

            WarpinatorIO.Load(Player);
        }

        private void SaveSettings(On_Player.orig_SavePlayer orig, PlayerFileData playerFile, bool skipMapSave)
        {
            orig(playerFile, skipMapSave);
            WarpinatorIO.Save(playerFile.Player);
        }

        public override void PostUpdate()
        {
            if (Player.HasWarpinator())
            {
                for (int i = 0; i < actions.Count; i++)
                {
                    actions[i].Selected = i == currentActionIndex;
                    actions[i].Update(Player);
                }
            }

            if (Player.HoldingWarpinator())
            {
                if (useSpecialCursorWireHands || useSpecialCursor)
                {
                    Player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, ((Main.MouseScreen.Y / Main.screenHeight) - 0.3f - MathHelper.PiOver2) * Player.direction);
                    Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, ((Main.MouseScreen.X / Main.screenWidth) + 0.1f - MathHelper.PiOver2) * Player.direction);
                }
                if ((useSpecialCursor && WarpUI.UISettings.CursorMode == OptionEnum.Default) || WarpUI.UISettings.CursorMode == OptionEnum.Always)
                {
                    useSpecialCursorWireHands = true;
                    VisualsSystem.UseSpecialCursor();
                }

                useSpecialCursor = WarpUI.UISettings.CursorMode == OptionEnum.Always;
                useSpecialCursorWireHands = useSpecialCursor;
            }
        }

        public bool mouseLeft;
        public bool mouseLeftHold;
        public bool mouseRight;
        public bool mouseRightHold;

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            mouseLeft = Main.mouseLeft && Main.mouseLeftRelease;
            mouseLeftHold = Main.mouseLeft;
            mouseRight = Main.mouseRight && Main.mouseRightRelease;
            mouseRightHold = Main.mouseRight;
        }
    }
}
