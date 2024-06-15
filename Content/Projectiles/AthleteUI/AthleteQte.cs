using System;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.UI.Minimap;

namespace AthleteClass.Content.Projectiles.AthleteUI;

public class AthleteQte : ModProjectile
{
    private Player Owner => Main.player[Projectile.owner];

    private Vector2[] Hits = new Vector2[3];

    public override void SetDefaults()
    {
        Projectile.friendly = false;
        Projectile.hostile = false;
        Projectile.timeLeft = 300;

        Projectile.aiStyle = 0;
    }

    public override void OnSpawn(IEntitySource source)
    {
        float curDegree = (float) Math.PI * 2 * Main._rand.NextFloat();
        float turnDegree = (float) Math.PI * 2 / 3;
        for (int i = 0; i < 3; i++)
        {
            Hits[0] = (curDegree - 0.5f + Main._rand.NextFloat()).ToRotationVector2() * 10;
        }
    }

    public override void AI()
    {
        
    }

    public override bool PreDraw(ref Color lightColor)
    {
        //Main.EntitySpriteDraw();
        return false;
    }
}