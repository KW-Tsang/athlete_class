using Terraria.ModLoader;

namespace AthleteClass.Common.Players;

public class CharismaPlayer : ModPlayer
{
    public int TrickTimeLeft;

    public override void Initialize()
    {
        TrickTimeLeft = -1;
    }

    public override void ResetEffects()
    {
        if (TrickTimeLeft >= 0)
        {
            TrickTimeLeft--;
        }
    }
}