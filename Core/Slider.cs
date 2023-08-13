using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperSpecialWarpinatorTool;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;

public struct Slider
{
    public float value;
    public Rectangle area;
    public bool hovered;
    private bool oldHovered;
    private bool dragging;
    public bool moving;

    public void DrawSlider(SpriteBatch spriteBatch, Color color, Color barColor, bool gradient = false, bool rainbow = false)
    {
        Texture2D texture = AssetDirectory.Textures.Slider;
        Texture2D buttonTexture = AssetDirectory.Textures.SliderButton;
        Rectangle baseFrame = texture.Frame(1, 4, 0, 0);
        Rectangle hoverFrame = texture.Frame(1, 4, 0, 0);
        Rectangle colorFrame = texture.Frame(1, 4, 0, rainbow ? 3 : 2);

        spriteBatch.Draw(texture, area.Location.ToVector2(), baseFrame, color, 0, Vector2.Zero, 1f, 0, 0);
        if (hovered || moving)
            spriteBatch.Draw(texture, area.Location.ToVector2(), hoverFrame, color.MultiplyRGBA(Main.OurFavoriteColor), 0, Vector2.Zero, 1f, 0, 0);

        if (gradient || rainbow)
            spriteBatch.Draw(texture, area.Location.ToVector2(), colorFrame, barColor.MultiplyRGBA(color), 0, Vector2.Zero, 1f, 0, 0);
        else
            spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(area.X + 4, area.Y + 4, area.Width - 8, 4), barColor.MultiplyRGBA(color));

        float buttonVal = Utils.GetLerpValue(-4, area.Width + 4, value * area.Width, true);
        spriteBatch.Draw(buttonTexture, area.Location.ToVector2() + new Vector2(texture.Width * buttonVal, baseFrame.Height / 2.33f), buttonTexture.Frame(), color, 0, buttonTexture.Size() * 0.5f, 1f, 0, 0);
    }

    public void UpdateSlider(Player player, Vector2 mousePos, bool alreadyDragging, bool withSound = true)
    {
        hovered = false;
        moving = false;

        if (area.Contains(mousePos.ToPoint()) && !alreadyDragging)
        {
            hovered = true;
            if (!oldHovered)
                SoundEngine.PlaySound(AssetDirectory.Sounds.MenuTick);

            if (player.WarpPlayer().mouseLeftHold)
            {
                moving = true;
                dragging = true;
            }

            if (Player.GetMouseScrollDelta() != 0 && !dragging)
            {
                moving = true;
                dragging = false;
                value += Player.GetMouseScrollDelta() * 0.01f;
            }
        }

        if (dragging)
        {
            player.mouseInterface = true;
            value = Utils.GetLerpValue(0, area.Width, mousePos.X - area.X, true);
            if (!player.WarpPlayer().mouseLeftHold)
                dragging = false;
        }

        value = MathHelper.Clamp(value, 0f, 1f);

        oldHovered = hovered;
    }
}