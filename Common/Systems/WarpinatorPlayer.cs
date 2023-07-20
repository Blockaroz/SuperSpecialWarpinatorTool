using SuperSpecialWarpinatorTool.Common.UI;
using SuperSpecialWarpinatorTool.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace SuperSpecialWarpinatorTool.Common.Systems
{
    public class WarpinatorPlayer : ModPlayer
    {
        public List<WarpinatorAction> actions;
        public int currentActionIndex;

        public WarpinatorAction CurrentAction => actions[currentActionIndex];

        public override void OnEnterWorld()
        {
            actions = new List<WarpinatorAction>
            {
                SuperSpecialWarpinatorTool.actions["Settings"],
                SuperSpecialWarpinatorTool.actions["Light"],
                SuperSpecialWarpinatorTool.actions["Butcher"],
            };

            WarpinatorSave.Load();
        }

        public override void Unload()
        {
            WarpinatorSave.Save();
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
