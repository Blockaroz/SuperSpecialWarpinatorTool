using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperSpecialWarpinatorTool;

public readonly record struct AssetDirectory
{
    public static string TexturePath = $"{nameof(SuperSpecialWarpinatorTool)}/Assets/Textures/";
    public static string SoundPath = $"{nameof(SuperSpecialWarpinatorTool)}/Assets/Sounds/";

    public struct Textures_UI
    {
        public static TextureAsset[] WarpPanel = TextureAsset.LoadArray(TexturePath + "UI/WarpPanel_", 2);

        public static TextureAsset[] FlowerButton = TextureAsset.LoadArray(TexturePath + "UI/FlowerButton_", 2);

        public static TextureAsset[] PageButton = TextureAsset.LoadArray(TexturePath + "UI/PageButton_", 2);

        public static TextureAsset[] DragButton = TextureAsset.LoadArray(TexturePath + "UI/FlowerButtonDrag_", 2);

        public static TextureAsset[] ButtonArrow = TextureAsset.LoadArray(TexturePath + "UI/ButtonArrow_", 2);

        public static TextureAsset[] HitboxIndicator = TextureAsset.LoadArray(TexturePath + "UI/HitboxIndicator_", 2);

        public static TextureAsset[] WiresMini = TextureAsset.LoadArray(TexturePath + "UI/Wires/WiresMini_", 4);

        public static TextureAsset[] WireTypes = TextureAsset.LoadArray(TexturePath + "UI/Wires/WireType_", 5);

        public static TextureAsset ToggleButton = new TextureAsset(TexturePath + "UI/ToggleButton");

        public static TextureAsset SelectorButton = new TextureAsset(TexturePath + "UI/SelectorButton");

        public static TextureAsset SelectorCursor = new TextureAsset(TexturePath + "UI/SelectorCursor");

        public static TextureAsset SpecialCursor = new TextureAsset(TexturePath + "UI/SpecialCursor");

        public static TextureAsset Slider = new TextureAsset(TexturePath + "UI/SliderBar");
        public static TextureAsset SliderButton = new TextureAsset(TexturePath + "UI/SliderButton");

        public static TextureAsset ScrollButton = new TextureAsset(TexturePath + "UI/ScrollButton");

        public struct Pages
        {
            public static TextureAsset Settings = new TextureAsset(TexturePath + "UI/Pages/Settings");
            public static TextureAsset Info = new TextureAsset(TexturePath + "UI/Pages/Info");
            public static TextureAsset NPCs = new TextureAsset(TexturePath + "UI/Pages/NPCs");
            public static TextureAsset Projectiles = new TextureAsset(TexturePath + "UI/Pages/Projectiles");
            public static TextureAsset Cursor = new TextureAsset(TexturePath + "UI/Pages/Cursor");
        }
    }

    public struct Textures_Player
    {
        public static TextureAsset WarpinatorHandsOn = new TextureAsset(TexturePath + "Player/SuperSpecialWarpinator_HandsOn");
        public static TextureAsset WarpinatorHandsOff = new TextureAsset(TexturePath + "Player/SuperSpecialWarpinator_HandsOff");
        public static TextureAsset WarpinatorTank = new TextureAsset(TexturePath + "Player/SuperSpecialWarpinator_Tank");
    }

    public struct Sounds_UI
    {
        public static SoundStyle MenuTick = new SoundStyle(SoundPath + "UI/MenuTick") with { MaxInstances = 0, PitchVariance = 0.1f };
        public static SoundStyle MenuTickSelect = new SoundStyle(SoundPath + "UI/MenuTickSelect") with { MaxInstances = 0 };
        public static SoundStyle MenuTickDeselect = new SoundStyle(SoundPath + "UI/MenuTickDeselect") with { MaxInstances = 0 };

        public static SoundStyle MenuPageSelect = new SoundStyle(SoundPath + "UI/MenuPageSelect") with { MaxInstances = 0 };

        public static SoundStyle MenuOpen = new SoundStyle(SoundPath + "UI/MenuOpen") with { MaxInstances = 0 };
        public static SoundStyle MenuClose = new SoundStyle(SoundPath + "UI/MenuClose") with { MaxInstances = 0 };        
        
        public static SoundStyle MenuSmallOpen = new SoundStyle(SoundPath + "UI/MenuSmallOpen") with { MaxInstances = 0 };
        public static SoundStyle MenuSmallClose = new SoundStyle(SoundPath + "UI/MenuSmallClose") with { MaxInstances = 0 };

        public static SoundStyle HookEntity = new SoundStyle(SoundPath + "UI/MenuTickSelect") with { MaxInstances = 0 };
        public static SoundStyle UnhookEntity = new SoundStyle(SoundPath + "UI/MenuTickDeselect") with { MaxInstances = 0 };
    }
}