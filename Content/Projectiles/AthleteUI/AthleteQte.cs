using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.UI.Minimap;

namespace AthleteClass.Content.Projectiles.AthleteUI;

public class AthleteQte : ModProjectile
{
    private Player Owner => Main.player[Projectile.owner];

    private Vector2[] Hits = new Vector2[3];

    private Vector2 origin = new Vector2(22f, 22f);

    public override void SetDefaults()
    {
        Projectile.width = Projectile.height = 44;
        Projectile.friendly = false;
        Projectile.hostile = false;
        Projectile.tileCollide = false;
        Projectile.timeLeft = 180;

        Projectile.aiStyle = 0;
    }

    public override void OnSpawn(IEntitySource source)
    {
        float curDegree = (float) Math.PI * 2 * Main._rand.NextFloat();
        float turnDegree = (float) Math.PI * 2 / 3;
        for (int i = 0; i < 3; i++)
        {
            Hits[i] = (curDegree - 0.5f + Main._rand.NextFloat()
                ).ToRotationVector2().SafeNormalize(Vector2.Zero) * 75;
            curDegree += turnDegree;
        }
    }

    public override void AI()
    {
        Projectile.velocity = Owner.velocity;
        Projectile.Center = Owner.Center;
    }

    public override bool PreDraw(ref Color lightColor)
    {
        Vector2 offSet = new Vector2(0f, Main.player[Projectile.owner].gfxOffY)
                         - Main.screenPosition;
        foreach (Vector2 h in Hits)
            Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, 
                Owner.Center + h + offSet, 
                null, Color.White, 0f, origin, 1f, SpriteEffects.None);
        
        return false;
    }
}