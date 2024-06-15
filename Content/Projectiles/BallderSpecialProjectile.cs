using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;

namespace AthleteClass.Content.Projectiles;

public class BallderSpecialProjectile : ModProjectile
{
    private float rotationDirection;

    private Player Owner => Main.player[Projectile.owner];
    
    public override void SetDefaults()
    {
        Projectile.width = 36;
        Projectile.height = 36;
        Projectile.timeLeft = 300;
        
        // set custom AI
        Projectile.aiStyle = 0;
        Projectile.ai[0] = 0f;
        
        Projectile.DamageType = ModContent.GetInstance<AthleticDamageClass>();
        Projectile.hostile = false;
        Projectile.friendly = true;
        Projectile.penetrate = 1;
    }

    public override void OnSpawn(IEntitySource source)
    {
        // set initial velocity
        Projectile.velocity.X = 1;
        Projectile.velocity = (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero) * 13;

        Projectile.rotation =(float)Math.PI * 2 * Main._rand.NextFloat();
        rotationDirection = Projectile.direction == 1 ? 0.026f : -0.026f;
    }

    public override void AI()
    {
        Projectile.velocity.X *= 0.995f;
        Projectile.velocity.Y += 0.07f;
        if (Projectile.velocity.Y > 16f)
            Projectile.velocity.Y = 16f;

        Projectile.rotation += rotationDirection;
    }

    public override void OnKill(int timeLeft)
    {
        //summon dust
        float n = 15 + Main._rand.NextFloat() * 5;
        float turnDegree = (float) Math.PI * 2 / n;
        float curDegree = (float)Math.PI * 2 * Main._rand.NextFloat();
        for (int i = (int)n; i > 0; i--)
        {
            Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Stone);
            dust.velocity = curDegree.ToRotationVector2() * 2f;
            curDegree += turnDegree;
        }
        
        SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
    }
    
    public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
        // adjustment for tile collision
        width = height = 24;
        return true;
    }
}