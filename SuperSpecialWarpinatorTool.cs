using SuperSpecialWarpinatorTool.Core;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace SuperSpecialWarpinatorTool
{
	public class SuperSpecialWarpinatorTool : Mod
    {
        internal static Dictionary<string, WarpinatorAction> actions = new Dictionary<string, WarpinatorAction>();

        public static WarpinatorAction ActionType<T>() where T : WarpinatorAction => actions[typeof(T).Name].NewInstance();
    }
}