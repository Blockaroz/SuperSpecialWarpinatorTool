using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperSpecialWarpinatorTool;
using Terraria;
using Terraria.Audio;

public struct ScrollBar
{
    public float value;
    public Vector2 position;
    public int height;

    public float viewArea;
    public float maxViewArea;

    private bool oldHoverArea;
    public bool hoverArea;
    public bool oldHoverBar;
    public bool hoverBar;

    private int dragOffset;
    private bool dragging;
    public bool moving;

    public void Draw(SpriteBatch spriteBatch, Color color)
    {
        Utils.DrawSplicedPanel(spriteBatch, AssetDirectory.Textures_UI.WarpPanel[1], (int)position.X, (int)position.Y, 12, height, 4, 4, 4, 4, color);

        float scrollHeight = viewArea / maxViewArea * height;
        Color scrollColor = hoverBar ? Color.White : Color.White * 0.85f;
        Utils.DrawSplicedPanel(spriteBatch, AssetDirectory.Textures_UI.ScrollButton, (int)position.X, (int)(position.Y + height * (value / maxViewArea)), 12, (int)scrollHeight, 4, 4, 4, 4, scrollColor.MultiplyRGBA(color));
    }

    public void Update(Player player, Vector2 mousePos, bool alreadyDragging, bool withSound = true)
    {
        float scrollHeight = viewArea / maxViewArea * (height - 4);

        Rectangle scrollArea = new Rectangle((int)position.X, (int)position.Y, 12, height);
        Rectangle barArea = new Rectangle((int)position.X, (int)(position.Y + height * (value / maxViewArea)), 12, (int)(scrollHeight));

        moving = false;
        hoverArea = scrollArea.Contains(mousePos.ToPoint());
        hoverBar = barArea.Contains(mousePos.ToPoint());

        if (hoverArea)
        {
            if (player.WarpPlayer().mouseLeft)
            {
                if (hoverBar && !dragging && !alreadyDragging)
                {
                    dragging = true;
                    dragOffset = (int)(mousePos.Y - barArea.Y);
                }
                else if (!hoverBar)
                {
                    float yPos = mousePos.Y - scrollArea.Y - ((int)scrollHeight >> 1);
                    value = MathHelper.Clamp(yPos / height * maxViewArea, 0f, maxViewArea - viewArea);
                }
            }
        }

        if (dragging)
        {
            moving = true;
            hoverBar = true;
            player.mouseInterface = true;

            float yPos = mousePos.Y - scrollArea.Y - dragOffset;
            value = MathHelper.Clamp(yPos / height * maxViewArea, 0f, maxViewArea - viewArea);

            if (!player.WarpPlayer().mouseLeftHold)
            {
                dragging = false;
                dragOffset = 0;
            }
        }

        value = MathHelper.Clamp(value, 0f, maxViewArea - viewArea);

        if (hoverBar && !oldHoverBar && withSound)
            SoundEngine.PlaySound(AssetDirectory.Sounds_UI.MenuTick);

        oldHoverArea = hoverArea;
        oldHoverBar = hoverBar;
    }
}