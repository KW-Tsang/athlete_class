using System;
using AthleteClass.Common;
using AthleteClass.Common.Players;
using Humanizer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Threading;
using SteelSeries.GameSense.DeviceZone;
using Terraria.ModLoader;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;

namespace AthleteClass.Content.Projectiles.AthleteUI;

public class AthleteQte : ModProjectile
{
    private Player Owner => Main.player[Projectile.owner];

    private QTIndicator[] Indicators = new QTIndicator[3];
    private int currInd;

    private int SuccessfulTricks;
    
    private bool prevDwn;

    // offsets for drawing
    private Vector2 origin = new (26f, 26f);
    private Vector2 offSet => new Vector2(0f, Owner.gfxOffY) - Main.screenPosition;

    public override void SetDefaults()
    {
        Projectile.width = Projectile.height = 52;
        Projectile.friendly = false;
        Projectile.hostile = false;
        Projectile.tileCollide = false;
        Projectile.timeLeft = 140;

        Projectile.aiStyle = 0;
        Projectile.ai[0] = 0f;

        Projectile.netUpdate = false;
    }

    public override void OnSpawn(IEntitySource source)
    {
        float curDegree = (float) Math.PI * 2 * Main._rand.NextFloat();
        float turnDegree = (float) Math.PI * 2 / 3;
        for (int i = 0; i < 3; i++)
        {
            Indicators[i] = new QTIndicator(curDegree - 0.5f + Main._rand.NextFloat(),
                50 + 30*i);
            curDegree += turnDegree;
        }
        
        // zoom
        Main.instance.CameraModifiers.Add(new ZoomCameraModifier(
            Projectile.timeLeft + 10, "athlete_qte", 1.5f, 20, 10));

        currInd = 0;
        prevDwn = true;
        SuccessfulTricks = 0;
    }
    

    public override void AI()
    {
        Projectile.velocity = Owner.velocity;
        Projectile.Center = Owner.Center;
        int time = (int)Projectile.ai[0];


        if (currInd < 3)
        {
            QTIndicator qti = Indicators[currInd];
            // on click
            if (Main.mouseLeft && !prevDwn)
            {
                
                if (qti.SuccessfulHit(time))
                {
                    // summon dust on successful hit
                    float n = 12 + Main._rand.NextFloat() * 3;
                    float turnDegree = (float)Math.PI * 2 / n;
                    float curDegree = (float)Math.PI * 2 * Main._rand.NextFloat();
                    for (int i = (int)n; i > 0; i--)
                    {
                        Dust dust = Dust.NewDustDirect(Owner.Center + qti.Position - origin,
                            Projectile.width, Projectile.height, DustID.YellowStarDust);
                        dust.velocity = curDegree.ToRotationVector2() * 2f;
                        dust.noGravity = true;
                        curDegree += turnDegree;
                    }

                    SuccessfulTricks++;
                    SoundEngine.PlaySound(SoundID.Item4, Owner.Center);
                    qti.State = 1;
                }
                else
                {
                    SoundEngine.PlaySound(SoundID.Item16, Owner.Center);
                    qti.State = 2;
                }

                currInd++;
            }
            
            // too late
            else if (qti.OverTime(time))
            {
                SoundEngine.PlaySound(SoundID.Item16, Owner.Center);
                qti.State = 2;
                currInd++;
            }
        }

        foreach (QTIndicator ind in Indicators)
        {
            // update qti if necessary
            if(ind.State == 0)
             ind.Update(time);
        }

        Projectile.ai[0]++;
        prevDwn = Main.mouseLeft;
    }

    public override void OnKill(int timeLeft)
    {
        //int trickTime = 180;
        Owner.GetModPlayer<CharismaPlayer>().TrickTimeLeft = 60 * SuccessfulTricks;
    }

    public override bool PreDraw(ref Color lightColor)
    {

        foreach (QTIndicator ind in Indicators)
        {
            drawQTI(ind);
        }

        return false;
    }


    private void drawQTI(QTIndicator ind)
    {
        // skip
        if (ind.State == 1)
        {
            return;
        }
        
        // fail
        if (ind.State == 2)
        {
            Main.EntitySpriteDraw((Texture2D) ModContent.Request<Texture2D>(
                    "AthleteClass/Content/Projectiles/AthleteUI/AthleteQte_fail") ,
                Owner.Center + ind.Position + offSet,
                null, Color.White, 0f, origin, 1f, SpriteEffects.None);
            
            return;
        }
        
        // inner circle
        Main.EntitySpriteDraw((Texture2D) ModContent.Request<Texture2D>(
                "AthleteClass/Content/Projectiles/AthleteUI/AthleteQte_timer") ,
            Owner.Center + ind.Position + offSet, 
            null, Color.White * 0.35f, 0f, origin, ind.Scale, SpriteEffects.None);
        
        // outer circle
        Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value,
            Owner.Center + ind.Position + offSet,
            null, Color.White, 0f, origin, 1f, SpriteEffects.None);
    }


    private class QTIndicator
    {
        public Vector2 Position;
        private int ClickTime;
        
        // 0 = active, 1 = success, 2 = fail
        public int State = 0;
        public float Scale = 0f;

        public QTIndicator(float angle, int clickTime)
        {
            Position = angle.ToRotationVector2() * 100;
            ClickTime = clickTime;
        }

        public void Update(int time)
        {
            if (time > ClickTime - 40 && time < ClickTime)
            {
                Scale += 0.025f;
            }
        }

        public bool SuccessfulHit(int time) => time > ClickTime - 12 && time < ClickTime;
        public bool OverTime(int time) => time > ClickTime;

    }
}