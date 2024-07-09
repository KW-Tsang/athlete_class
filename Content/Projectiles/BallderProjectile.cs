using System;
using AthleteClass.Common.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;

namespace AthleteClass.Content.Projectiles;

public class BallderProjectile : ModProjectile
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
        
        Projectile.DamageType = ModContent.GetInstance<AthleticDamageClass>();
        Projectile.hostile = false;
        Projectile.friendly = true;
        Projectile.penetrate = 2;
    }

    public override void OnSpawn(IEntitySource source)
    {
        // set initial velocity
        Projectile.velocity.X = 10;
        Projectile.velocity.Y = 0;
        TurnTo(Main.MouseWorld);

        Projectile.rotation = 2 * (float)Math.PI * Main._rand.NextFloat();
        rotationDirection = Projectile.direction == 1 ? 0.02f : -0.02f;
    }

    private void TurnTo(Vector2 v)
    {
        float speed = Projectile.velocity.Length();
        Projectile.velocity = (v - Projectile.Center).SafeNormalize(Vector2.Zero) * speed;
    }

    public override void AI()
    {
        // set player rapier direction
        //Main.player[Projectile.owner].heldProj = Projectile.whoAmI;
            
        // select AI
        if (Projectile.ai[0] == 0f)
        {
            ThrowAI();
        }
        else
        {
            ReturnAI();
        }
    }

    private void ThrowAI()
    {
        Projectile.velocity.X *= 0.99f;
        Projectile.velocity.Y += 0.15f;
        if (Projectile.velocity.Y > 16f)
            Projectile.velocity.Y = 16f;

        Projectile.rotation += rotationDirection;
    }

    private void ReturnAI()
    {
        TurnTo(Owner.Center);
        Projectile.velocity *= 1.025f;
        Projectile.rotation -= rotationDirection;
        
        // catch
        if (Main.myPlayer == Projectile.owner &&
            Projectile.getRect().Intersects(Owner.getRect()))
        {
            Projectile.Kill();
        }
    }

    private void BounceBack()
    {
        Projectile.friendly = false;
        Projectile.tileCollide = false;
        
        // switch AI
        Projectile.ai[0] = 1f;
        Projectile.velocity.X = 10;
        Projectile.velocity.Y = 0;
        TurnTo(Owner.Center);
    }

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
        BounceBack();
        return false;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        Owner.GetModPlayer<CharismaPlayer>().AddCharisma(5);
        BounceBack();
        base.OnHitNPC(target, hit, damageDone);
    }
}