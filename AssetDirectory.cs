using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperSpecialWarpinatorTool;

public record struct AssetDirectory
{
    public static void Load(Mod mod)
    {
        Textures.Load(mod);
        Sounds.Load(mod);
    }
    
    public static void Unload()
    {
        foreach (object possibleAsset in typeof(AssetDirectory).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
            if (possibleAsset is TextureAsset asset)
                asset.Dispose();
    }

    public record struct Textures
    {
        public static void Load(Mod mod)
        {
            string TexturePath = mod.Name + "/Assets/Textures/";

            WarpinatorHandsOn = new TextureAsset(TexturePath + "Player/SuperSpecialWarpinator_HandsOn");
            WarpinatorHandsOff = new TextureAsset(TexturePath + "Player/SuperSpecialWarpinator_HandsOff");
            WarpinatorTank = new TextureAsset(TexturePath + "Player/SuperSpecialWarpinator_Tank");
            WarpPanel = TextureAsset.LoadArray(TexturePath + "UI/WarpPanel_", 4);
            FlowerButton = TextureAsset.LoadArray(TexturePath + "UI/FlowerButton_", 2);
            PageButton = TextureAsset.LoadArray(TexturePath + "UI/PageButton_", 2);
            DragButton = TextureAsset.LoadArray(TexturePath + "UI/FlowerButtonDrag_", 2);
            ButtonArrow = TextureAsset.LoadArray(TexturePath + "UI/ButtonArrow_", 2);
            HitboxIndicator = TextureAsset.LoadArray(TexturePath + "UI/HitboxIndicator_", 2);
            WiresMini = TextureAsset.LoadArray(TexturePath + "UI/Wires/WiresMini_", 4);
            WireTypes = TextureAsset.LoadArray(TexturePath + "UI/Wires/WireType_", 5);
            ToggleButton = new TextureAsset(TexturePath + "UI/ToggleButton");
            SelectorButton = new TextureAsset(TexturePath + "UI/SelectorButton");
            SelectorCursor = new TextureAsset(TexturePath + "UI/SelectorCursor");
            SpecialCursor = new TextureAsset(TexturePath + "UI/SpecialCursor");
            Slider = new TextureAsset(TexturePath + "UI/SliderBar");
            SliderButton = new TextureAsset(TexturePath + "UI/SliderButton");
            ScrollButton = new TextureAsset(TexturePath + "UI/ScrollButton");

            Pages.Settings = new TextureAsset(TexturePath + "UI/Pages/Settings");
            Pages.Menu = new TextureAsset(TexturePath + "UI/Pages/Menu");
            Pages.Info = new TextureAsset(TexturePath + "UI/Pages/Info");
            Pages.NPCs = new TextureAsset(TexturePath + "UI/Pages/NPCs");
            Pages.Projectiles = new TextureAsset(TexturePath + "UI/Pages/Projectiles");
            Pages.Cursor = new TextureAsset(TexturePath + "UI/Pages/Cursor");
            Pages.Butcher = new TextureAsset(TexturePath + "UI/Pages/Butcher");
        }

        public static TextureAsset WarpinatorHandsOn;
        public static TextureAsset WarpinatorHandsOff;
        public static TextureAsset WarpinatorTank;

        public static TextureAsset[] WarpPanel;
        public static TextureAsset[] FlowerButton;
        public static TextureAsset[] PageButton;
        public static TextureAsset[] DragButton;
        public static TextureAsset[] ButtonArrow;
        public static TextureAsset[] HitboxIndicator;
        public static TextureAsset[] WiresMini;
        public static TextureAsset[] WireTypes;
        public static TextureAsset ToggleButton;
        public static TextureAsset SelectorButton;
        public static TextureAsset SelectorCursor;
        public static TextureAsset SpecialCursor;

        public static TextureAsset Slider;
        public static TextureAsset SliderButton;
        public static TextureAsset ScrollButton;

        public record struct Pages
        {
            public static TextureAsset Settings;
            public static TextureAsset Menu;
            public static TextureAsset Info;
            public static TextureAsset NPCs;
            public static TextureAsset Projectiles;
            public static TextureAsset Cursor;
            public static TextureAsset Butcher;
        }
    }

    public record struct Sounds
    {
        public static void Load(Mod mod)
        {
            string SoundPath = mod.Name + "/Assets/Sounds/";
            MenuTick = new SoundStyle(SoundPath + "UI/MenuTick") with { MaxInstances = 0, PitchVariance = 0.1f };
            MenuTickSelect = new SoundStyle(SoundPath + "UI/MenuTickSelect") with { MaxInstances = 0 };
            MenuTickDeselect = new SoundStyle(SoundPath + "UI/MenuTickDeselect") with { MaxInstances = 0 };
            MenuPageSelect = new SoundStyle(SoundPath + "UI/MenuPageSelect") with { MaxInstances = 0 };
            MenuOpen = new SoundStyle(SoundPath + "UI/MenuOpen") with { MaxInstances = 0 };
            MenuClose = new SoundStyle(SoundPath + "UI/MenuClose") with { MaxInstances = 0 };
            MenuSmallOpen = new SoundStyle(SoundPath + "UI/MenuSmallOpen") with { MaxInstances = 0 };
            MenuSmallClose = new SoundStyle(SoundPath + "UI/MenuSmallClose") with { MaxInstances = 0 };
            HookEntity = new SoundStyle(SoundPath + "UI/MenuTickSelect") with { MaxInstances = 0 };
            UnhookEntity = new SoundStyle(SoundPath + "UI/MenuTickDeselect") with { MaxInstances = 0 };
        }

        public static SoundStyle MenuTick;
        public static SoundStyle MenuTickSelect;
        public static SoundStyle MenuTickDeselect;
        public static SoundStyle MenuPageSelect;
        public static SoundStyle MenuOpen;
        public static SoundStyle MenuClose;
        public static SoundStyle MenuSmallOpen;
        public static SoundStyle MenuSmallClose;
        public static SoundStyle HookEntity;
        public static SoundStyle UnhookEntity;
    }
}