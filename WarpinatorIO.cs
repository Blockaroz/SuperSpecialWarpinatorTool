using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SuperSpecialWarpinatorTool.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace SuperSpecialWarpinatorTool;

internal class WarpinatorIO : ModSystem
{
    internal static readonly string SavePath = Path.Combine(Main.SavePath, "SuperSpecialWarpinator");
    internal static readonly string ImagePath = Path.Combine(Main.SavePath, "SuperSpecialWarpinator/Captures");

    internal static readonly JsonSerializerSettings serializerSettings = new()
    {
        Formatting = Formatting.Indented,
        TypeNameHandling = TypeNameHandling.Objects,
        DefaultValueHandling = DefaultValueHandling.Populate,
        ObjectCreationHandling = ObjectCreationHandling.Replace,
        NullValueHandling = NullValueHandling.Include,
        ContractResolver = new DefaultContractResolver(),
        SerializationBinder = new DefaultSerializationBinder()
    };

    internal static void Save(Player player)
    {
        Dictionary<string, object> saveData = new Dictionary<string, object>();

        foreach (WarpinatorAction action in player.WarpPlayer().actions.Where(n => n is not IDoNotAutosave))
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

            foreach (WarpinatorAction action in player.WarpPlayer().actions.Where(n => n is not IDoNotAutosave))
            {
                FieldInfo[] fields = action.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
                foreach (FieldInfo field in fields.Where(n => n.MemberType == MemberTypes.Field))
                {
                    string key = action.Name + ':' + field.Name;
                    if (saveData.TryGetValue(key, out object value))
                    {
                        if (value != null)
                        {
                            var fieldData = field.GetValue(action).GetType().GetField("Value");
                            var incomingData = value.GetType().GetField("Value");
                            if (fieldData != null && incomingData != null)
                                fieldData.SetValue(field.GetValue(action), incomingData.GetValue(value));
                        }
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
