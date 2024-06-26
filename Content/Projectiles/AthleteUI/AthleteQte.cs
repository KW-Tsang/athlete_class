using System;
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
    private CharismaPlayer ChaPla => Main.player[Projectile.owner].GetModPlayer<CharismaPlayer>();

    private QTIndicator[] Indicators = new QTIndicator[3];
    private int currInd;
    private bool prevDwn;

    private Vector2 origin = new Vector2(26f, 26f);

    public override void SetDefaults()
    {
        Projectile.width = Projectile.height = 52;
        Projectile.friendly = false;
        Projectile.hostile = false;
        Projectile.tileCollide = false;
        Projectile.timeLeft = 150;

        Projectile.aiStyle = 0;
        Projectile.ai[0] = 0f;
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

        currInd = 0;
        prevDwn = true;
        ChaPla.successfulTrick = true;
    }

    public override void OnKill(int timeLeft)
    {
        AthleteZoom.Scale = 1f;
    }

    public override void AI()
    {
        Projectile.velocity = Owner.velocity;
        Projectile.Center = Owner.Center;
        int time = (int)Projectile.ai[0];


        if (currInd < 3)
        {
            // on click
            if (Main.mouseLeft && !prevDwn)
            {
                QTIndicator qti = Indicators[currInd++];
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

                    SoundEngine.PlaySound(SoundID.Item4, Owner.Center);
                }
                else
                {
                    ChaPla.successfulTrick = false;
                    SoundEngine.PlaySound(SoundID.Item16, Owner.Center);
                }

                qti.DoDraw = false;
            }
            
            // too late
            else if (!Indicators[currInd].DoDraw)
            {
                currInd++;
                SoundEngine.PlaySound(SoundID.Item16, Owner.Center);
            }
        }

        // zoom
        if (time < 20)
        {
            AthleteZoom.Scale += 0.025f;
        }
        if (time > 140)
        {
            AthleteZoom.Scale -= 0.05f;
        }

        foreach (QTIndicator ind in Indicators)
        {
            //skip
            if(ind.DoDraw)
             ind.Update(time);
        }

        Projectile.ai[0]++;
        prevDwn = Main.mouseLeft;
    }

    public override bool PreDraw(ref Color lightColor)
    {
        // calculate offset
        Vector2 offSet = new Vector2(0f, Owner.gfxOffY)
                         - Main.screenPosition;

        foreach (QTIndicator ind in Indicators)
        {
            // skip
            if (!ind.DoDraw)
            {
                continue;
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

        return false;
    }


    private class QTIndicator
    {
        public Vector2 Position;
        private int ClickTime;
        
        public bool DoDraw = true;
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
            DoDraw = time <= ClickTime;
        }

        public bool SuccessfulHit(int time) => time > ClickTime - 12 && time < ClickTime;

    }
}