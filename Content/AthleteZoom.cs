using Terraria;
using Terraria.Graphics;
using Terraria.ModLoader;

namespace AthleteClass.Content;

public class AthleteZoom : ModSystem
{
    public static float Scale = 1f;
    
    public override void ModifyTransformMatrix(ref SpriteViewMatrix Transform)
    {
        Transform.Zoom *= Scale;
    }
}