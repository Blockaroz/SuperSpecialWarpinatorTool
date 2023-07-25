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
        public static TextureAsset[] WiresMini = new TextureAsset[]
        {
            new TextureAsset(TexturePath + "UI/Wires/WiresMini_" + 0),
            new TextureAsset(TexturePath + "UI/Wires/WiresMini_" + 1),
            new TextureAsset(TexturePath + "UI/Wires/WiresMini_" + 2),
            new TextureAsset(TexturePath + "UI/Wires/WiresMini_" + 3)
        };        
        
        public static TextureAsset[] WireTypes = new TextureAsset[]
        {
            new TextureAsset(TexturePath + "UI/Wires/WireType_" + 0),
            new TextureAsset(TexturePath + "UI/Wires/WireType_" + 1),
            new TextureAsset(TexturePath + "UI/Wires/WireType_" + 2),
            new TextureAsset(TexturePath + "UI/Wires/WireType_" + 3),
            new TextureAsset(TexturePath + "UI/Wires/WireType_" + 4)
        };

        public static TextureAsset[] FlowerButton = new TextureAsset[]
        {
            new TextureAsset(TexturePath + "UI/FlowerButton_" + 0),
            new TextureAsset(TexturePath + "UI/FlowerButton_" + 1),
        };        
               
        public static TextureAsset[] PageButton = new TextureAsset[]
        {
            new TextureAsset(TexturePath + "UI/PageButton_" + 0),
            new TextureAsset(TexturePath + "UI/PageButton_" + 1),
        };        
        
        public static TextureAsset[] DragButton = new TextureAsset[]
        {
            new TextureAsset(TexturePath + "UI/FlowerButtonDrag_" + 0),
            new TextureAsset(TexturePath + "UI/FlowerButtonDrag_" + 1),
        };

        public static TextureAsset[] ButtonArrow = new TextureAsset[]
        {
            new TextureAsset(TexturePath + "UI/ButtonArrow_" + 0),
            new TextureAsset(TexturePath + "UI/ButtonArrow_" + 1),
        };

        public static TextureAsset ToggleButton = new TextureAsset(TexturePath + "UI/ToggleButton");

        public static TextureAsset HitboxIndicator = new TextureAsset(TexturePath + "UI/HitboxIndicator");

        public static TextureAsset SelectorButton = new TextureAsset(TexturePath + "UI/SelectorButton");
        public static TextureAsset SelectorCursor = new TextureAsset(TexturePath + "UI/SelectorCursor");

        public static TextureAsset Slider = new TextureAsset(TexturePath + "UI/SliderBar");
        public static TextureAsset SliderButton = new TextureAsset(TexturePath + "UI/SliderButton");
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
        public static SoundStyle MenuTickSelectOff = new SoundStyle(SoundPath + "UI/MenuTickSelect") with { MaxInstances = 0, Pitch = -0.25f };

        public static SoundStyle MenuPageSelect = new SoundStyle(SoundPath + "UI/MenuPageSelect") with { MaxInstances = 0 };

        public static SoundStyle MenuOpen = new SoundStyle(SoundPath + "UI/MenuOpen") with { MaxInstances = 0 };
        public static SoundStyle MenClose = new SoundStyle(SoundPath + "UI/MenuClose") with { MaxInstances = 0 };        
        
        public static SoundStyle MenuSmallOpen = new SoundStyle(SoundPath + "UI/MenuSmallOpen") with { MaxInstances = 0 };
        public static SoundStyle MenuSmallClose = new SoundStyle(SoundPath + "UI/MenuSmallClose") with { MaxInstances = 0 };
    }
}