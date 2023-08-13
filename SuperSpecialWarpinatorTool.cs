using ReLogic.Content.Sources;
using SuperSpecialWarpinatorTool.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;

namespace SuperSpecialWarpinatorTool
{
	public class SuperSpecialWarpinatorTool : Mod
    {
        internal static Dictionary<string, WarpinatorAction> actions = new Dictionary<string, WarpinatorAction>();

        public static WarpinatorAction ActionType<T>() where T : WarpinatorAction => actions[typeof(T).Name].NewInstance();
    }

    public class AssetDirectoryLoader : ILoadable
    {
        public void Load(Mod mod) => AssetDirectory.Load(mod);

        public void Unload() => AssetDirectory.Unload();
    }
}