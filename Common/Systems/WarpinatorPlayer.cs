using Microsoft.Xna.Framework.Input;
using SuperSpecialWarpinatorTool.Content.WarpinatorActions;
using SuperSpecialWarpinatorTool.Core;
using System;
using System.Collections.Generic;
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
                foreach (var action in actions)
                    action.Update(Player);
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
