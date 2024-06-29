using AthleteClass.Common.Players;
using AthleteClass.Content.Projectiles;
using AthleteClass.Content.Projectiles.AthleteUI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AthleteClass.Content.Items.Weapons;

public class Ballder : ModItem
{
    private int[] ShootedProjectile = {
        ModContent.ProjectileType<BallderProjectile>(),
        ModContent.ProjectileType<BallderSpecialProjectile>()
    };

    private int QTE = ModContent.ProjectileType<AthleteQte>();
    
    public override void SetDefaults()
    {
        Item.width = 36;
        Item.height = 36;
        Item.noMelee = true;
        Item.noUseGraphic = true;
        
        Item.value = 0;
        
        //placeholder
        Item.damage = 7;
        Item.useTime = 20;
        Item.useAnimation = 20;
        Item.useStyle = ItemUseStyleID.Rapier;
        Item.knockBack = 6f;
        Item.rare = ItemRarityID.Blue;
        Item.UseSound = SoundID.Item1;
        Item.autoReuse = true;

        Item.DamageType = ModContent.GetInstance<AthleticDamageClass>();
        Item.shoot = ShootedProjectile[0];
        Item.shootSpeed = 20f;
    }

    public override bool AltFunctionUse(Player player)
    {
        return true;
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type,
        int damage, float knockback)
    {
        // summon qte on right click
        if (player.altFunctionUse == 2)
        {
            Projectile.NewProjectile(source, player.Center, Vector2.Zero, QTE, 0, 0f);
            return false;
        }
        
        // shoot special projectile 
        if (player.GetModPlayer<CharismaPlayer>().successfulTrick)
        {
            Projectile.NewProjectile(source, player.Center, Vector2.Zero, ShootedProjectile[1],
                16, 6f, player.whoAmI);
            
            // reset trick
            player.GetModPlayer<CharismaPlayer>().successfulTrick = false;
            return false;
        }

        return true;
    }

    public override bool CanUseItem(Player player)
    {
        if (player.altFunctionUse == 2)
        {
            Item.useTime = 140;
            //Item.useStyle = ItemUseStyleID.Thrust;
            Item.useAnimation = 140;
            Item.noUseGraphic = false;
        }
        else
        {
            Item.useTime = 20;
            //Item.useStyle = ItemUseStyleID.Rapier;
            Item.useAnimation = 20;
            Item.noUseGraphic = true;
        }

        return player.ownedProjectileCounts[ShootedProjectile[0]] < 1 &&
               player.ownedProjectileCounts[QTE] < 1 &&
               player.ownedProjectileCounts[ShootedProjectile[1]] < 1;
    }
}