using FullSerializer;
using Newtonsoft.Json;
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
using tModPorter;

namespace SuperSpecialWarpinatorTool;

//based off of modconfigs
internal static class WarpinatorSave
{
    internal static readonly string SavePath = Path.Combine(Main.SavePath, "SuperSpecialWarpinatorSettings");

    private static Dictionary<string, object> saveData;

    internal static readonly JsonSerializerSettings serializerSettings = new()
    {
        Formatting = Formatting.Indented,
        DefaultValueHandling = DefaultValueHandling.Populate,
        ObjectCreationHandling = ObjectCreationHandling.Replace,
        NullValueHandling = NullValueHandling.Include,
        ContractResolver = new DefaultContractResolver()
    };

    internal static void Save()
    {
        GetData();

        Directory.CreateDirectory(SavePath);
        string path = Path.Combine(SavePath, "Actions.json");
        string json = JsonConvert.SerializeObject(saveData, serializerSettings);
        File.WriteAllText(path, json);
    }

    internal static void Load()
    {
        string path = Path.Combine(SavePath, "Actions.json");
        try
        {
            bool jsonFileExists = File.Exists(path);
            string json = jsonFileExists ? File.ReadAllText(path) : "{}";

            JsonConvert.PopulateObject(json, saveData, serializerSettings);
        }
        catch
        {
            foreach (WarpinatorAction action in Main.LocalPlayer.WarpPlayer().actions)
                action.SetDefaults();
                
            Save();
        }

        SetData();
    }

    private static void GetData()
    {
        saveData = new Dictionary<string, object>();
        foreach (WarpinatorAction action in Main.LocalPlayer.WarpPlayer().actions)
        {
            FieldInfo[] fields = action.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (FieldInfo field in fields)
            {
                string key = action.Name + '.' + field.Name;
                saveData.Add(key, field.GetValue(action));
            }
        }
    }

    private static void SetData()
    {
        //foreach (WarpinatorAction action in Main.LocalPlayer.WarpPlayer().actions)
        //{
        //    FieldInfo[] fields = action.GetType().GetFields();
        //    foreach (FieldInfo field in fields.Where(n => n.MemberType == MemberTypes.Field))
        //    {
        //        string key = action.Name + '.' + field.Name;
        //        field.SetValue(action, saveData[key]);
        //    }
        //}
    }
}
