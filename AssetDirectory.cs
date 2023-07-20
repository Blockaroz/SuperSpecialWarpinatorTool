using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperSpecialWarpinatorTool;

public readonly record struct AssetDirectory
{
    public static string TexturePath = $"{nameof(SuperSpecialWarpinatorTool)}/Assets/Textures/";

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
        public static TextureAsset DisplayButton = new TextureAsset(TexturePath + "UI/DisplayButton");
        public static TextureAsset Slider = new TextureAsset(TexturePath + "UI/SliderBar");
        public static TextureAsset SliderButton = new TextureAsset(TexturePath + "UI/SliderButton");
    }

    public struct Sounds_UI
    {
        public static SoundStyle MenuTick = SoundID.MenuTick.WithPitchOffset(0.7f);
        public static SoundStyle MenuTickSelect = SoundID.MenuTick.WithPitchOffset(0.33f);

        public static SoundStyle MenuOpen = SoundID.MenuOpen.WithPitchOffset(0.33f);
        public static SoundStyle MenuClose = SoundID.MenuClose.WithPitchOffset(0.33f);

        public static SoundStyle MiniMenuOpen = SoundID.MenuOpen.WithPitchOffset(0.7f);
        public static SoundStyle MiniMenuClose = SoundID.MenuClose.WithPitchOffset(0.7f);
    }
}