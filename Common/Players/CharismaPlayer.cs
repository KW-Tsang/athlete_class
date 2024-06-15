using Terraria.ModLoader;

namespace AthleteClass.Common.Players;

public class CharismaPlayer : ModPlayer
{
    public bool successfulTrick;

    public override void Initialize()
    {
        successfulTrick = false;
    }

    public override void ResetEffects()
    {
        successfulTrick = false;
    }
}