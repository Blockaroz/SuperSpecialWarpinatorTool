using FullSerializer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using SuperSpecialWarpinatorTool.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.WorldBuilding;
using tModPorter;

namespace SuperSpecialWarpinatorTool;

[JsonObject(MemberSerialization.OptIn)]
internal class WarpinatorIO : ModSystem
{
    internal static readonly string SavePath = Path.Combine(Main.SavePath, "SuperSpecialWarpinator");
    internal static readonly string ImagePath = Path.Combine(Main.SavePath, "SuperSpecialWarpinator/Captures");

    internal static readonly JsonSerializerSettings serializerSettings = new()
    {
        Formatting = Formatting.Indented,
        TypeNameHandling = TypeNameHandling.Auto,
        DefaultValueHandling = DefaultValueHandling.Populate,
        ObjectCreationHandling = ObjectCreationHandling.Replace,
        NullValueHandling = NullValueHandling.Include,
        ContractResolver = new DefaultContractResolver(),
        SerializationBinder = new DefaultSerializationBinder()
    };

    internal static void Save(Player player)
    {
        Dictionary<string, object> saveData = new Dictionary<string, object>();
        foreach (WarpinatorAction action in player.WarpPlayer().actions)
        {
            FieldInfo[] fields = action.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (FieldInfo field in fields)
            {
                string key = action.Name + ':' + field.Name;
                saveData.Add(key, field.GetValue(action));
            }
        }

        Directory.CreateDirectory(SavePath);
        string path = Path.Combine(SavePath, "ActionConfig.json");
        string json = JsonConvert.SerializeObject(saveData, serializerSettings);
        File.WriteAllText(path, json);
    }

    internal static void Load(Player player)
    {
        string path = Path.Combine(SavePath, "ActionConfig.json");
        foreach (WarpinatorAction action in player.WarpPlayer().actions)
            action.SetDefaults();

        try
        {
            Dictionary<string, object> saveData = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(path), serializerSettings);
            
            foreach (WarpinatorAction action in player.WarpPlayer().actions)
            {
                FieldInfo[] fields = action.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
                foreach (FieldInfo field in fields.Where(n => n.MemberType == MemberTypes.Field))
                {
                    string key = action.Name + ':' + field.Name;
                    if (saveData.TryGetValue(key, out object value))
                    {
                        if (value != null)
                            field.SetValue(action, value);
                    }
                }
            }
        }
        catch
        {              
            Save(player);
        }
    }
}
