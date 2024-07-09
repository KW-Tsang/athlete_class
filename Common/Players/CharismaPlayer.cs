using Terraria.ModLoader;

namespace AthleteClass.Common.Players;

public class CharismaPlayer : ModPlayer
{
    private float Charisma;
    private int CharismaLevel;

    public float CharismaDrainRate;
    private int DefaultCharismaThreshold = 100;
    public int CharismaThreshold;

    public int TrickScore;
    public int TrickTimeLeft;

    public override void Initialize()
    {
        TrickScore = 0;
        TrickTimeLeft = -1;
        Charisma = 0;
        CharismaLevel = 0;

        CharismaThreshold = DefaultCharismaThreshold;
    }

    public override void ResetEffects() => ResetStats();
    public override void UpdateDead() => ResetStats();
    private void ResetStats()
    {
        CharismaDrainRate = 1/60f;
    }

    public void AddCharisma(int x)
    {
        Charisma += x;
        if (Charisma > 100 + CharismaLevel * 70)
        {
            Charisma = 100 + CharismaLevel * 70;
        }
    }

    public void PurgeCharisma(int x)
    {
        Charisma = 0;
    }
    
    public override void PostUpdateMiscEffects()
    {
        // special time
        if (TrickTimeLeft >= 0)
        {
            TrickTimeLeft--;
            return;
        }
        
        // charisma
        if (Charisma > 0)
            Charisma -= CharismaDrainRate;

        if (Charisma <= 0)
        {
            Charisma = 0;
            CharismaLevel = 0;
        }
    }
}