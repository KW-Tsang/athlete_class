using System.Drawing;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace AthleteClass.Common.UI;

internal class CharismaBar : UIState
{
    private UIElement area;
    private UIImage frame;
    private Color gradientA;
    private Color gradientB;

    public override void OnInitialize()
    {
        // container
        area = new UIElement();
        area.Left.Set(Main.ScreenSize.X/2, 1f);
        area.Top.Set(Main.ScreenSize.Y/2, 1f);
        area.Height.Set(100, 1f);
        area.Width.Set(28, 1f);
        
        // frame
        frame = new UIImage(ModContent.Request<Texture2D>("AthleteClass/Common/UI/CharismaBar"));
        frame.Left.Set(0, 0f);
        frame.Top.Set(0, 0f);
        frame.Height.Set(100, 1f);
        frame.Width.Set(28, 1f);

        // Gradient colours
        gradientA = Color.FromArgb(0, 0, 0);
        gradientB = Color.FromArgb(255, 255, 255);
        
        area.Append(frame);
        Append(area);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
    }
}