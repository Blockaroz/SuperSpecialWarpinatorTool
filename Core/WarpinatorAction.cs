using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Utils;
using SuperSpecialWarpinatorTool;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace SuperSpecialWarpinatorTool.Core;

public abstract class WarpinatorAction : ILocalizedModType, ILoadable
{
    public string LocalizationCategory => "WarpinatorActions";

    public Mod Mod { get; private set; }

    public int Type { get; private set; }

    public string Name => GetType().Name;

    public string FullName => GetType().FullName;

    public LocalizedText DisplayName { get; private set; }

    public virtual string Texture => $"{Mod.Name}/Assets/Textures/WarpinatorActions/" + Name;

    private protected Texture2D iconTexture;

    public virtual bool HasPerformableAction => true;

    public bool Selected { get; internal set; }

    public List<IMenuElement> MenuElements { get; private set; }

    public virtual List<IMenuElement> AddMenuElements() => new List<IMenuElement>();

    public WarpinatorAction NewInstance() => (WarpinatorAction)MemberwiseClone();

    public void Load(Mod mod)
    {
        Mod = mod;
        DisplayName = Language.GetOrRegister(Mod.GetLocalizationKey(LocalizationCategory + '.' + Name), () => Regex.Replace(Name, "([A-Z])", " $1").Trim());
        Type = SuperSpecialWarpinatorTool.actions.Count;
        iconTexture = new TextureAsset(Texture);

        OnLoad(mod);

        SetDefaults();
        MenuElements = AddMenuElements();

        ModTypeLookup<WarpinatorAction>.Register(this);

        SuperSpecialWarpinatorTool.actions.Add(Name, this);
    }

    public void Unload() => OnUnload();

    public virtual void OnLoad(Mod mod) { }

    public virtual void OnUnload() { }

    public virtual void SetDefaults() { }

    public virtual void DrawIcon(SpriteBatch spriteBatch, Vector2 center, Color color, float scale)
    {
        spriteBatch.Draw(iconTexture, center, iconTexture.Frame(), color, 0, iconTexture.Size() * 0.5f, scale, 0, 0);
    }

    public virtual void Update(Player player) { }

    public virtual void Perform(Player player, Item item) { }
}
