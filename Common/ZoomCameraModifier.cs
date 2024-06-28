using Terraria.Graphics.CameraModifiers;
using AthleteClass.Content;
using Terraria;

namespace AthleteClass.Common;

public class ZoomCameraModifier : ICameraModifier
{
    public string UniqueIdentity { get; private set;  }
    
    private int TimeLeft;
    private int InTime;
    private int OutTime;
    
    private float ScaleIn;
    private float ScaleOut;

    public bool Finished { get; private set;  }

    public ZoomCameraModifier(int timeLeft, string uniqueIdentity = null, float scale = 1.5f, int easeIn = 20, int easeOut = 20)
    {
        TimeLeft = timeLeft;
        UniqueIdentity = uniqueIdentity;

        // check easing
        InTime = easeIn < 1 ? 1 : easeIn;
        OutTime = easeOut < 1 ? easeIn : easeOut;
        InTime = TimeLeft - InTime;
        
        // calculate scale increments
        ScaleIn = (scale - 1f) / easeIn;
        ScaleOut = (scale - 1f) / easeOut;

        Finished = false;
    }

    public void Update(ref CameraInfo cameraPosition)
    {
        if(Main.gamePaused)
            return;
        
        // zoom
        if (TimeLeft > InTime)
        {
            AthleteZoom.Scale += ScaleIn;
        }
        
        if (TimeLeft < OutTime)
        {
            AthleteZoom.Scale -= ScaleOut;
        }
        
        if (--TimeLeft <= 0)
        {
            AthleteZoom.Scale = 1f;
            Finished = true;
        }
    }
}