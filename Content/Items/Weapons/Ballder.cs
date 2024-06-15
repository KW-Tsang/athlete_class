using AthleteClass.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AthleteClass.Content.Items.Weapons;

public class Ballder : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 36;
        Item.height = 36;
        Item.noMelee = true;
        Item.noUseGraphic = true;
        
        Item.value = 0;
        
        //placeholder
        Item.damage = 10;
        Item.useTime = 20;
        Item.useAnimation = 20;
        Item.useStyle = ItemUseStyleID.Rapier;
        Item.knockBack = 6;
        Item.rare = ItemRarityID.Blue;
        Item.UseSound = SoundID.Item1;
        Item.autoReuse = true;

        Item.DamageType = ModContent.GetInstance<AthleticDamageClass>();
        Item.shoot = ModContent.ProjectileType<BallderProjectile>();
    }

    public override bool AltFunctionUse(Player player)
    {
        return true;
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type,
        int damage, float knockback)
    {
        /*if (player.altFunctionUse == 2)
        {
            return false;
        }*/
        return true;
    }

    public override bool CanUseItem(Player player)
    {
        if (player.altFunctionUse == 2)
        {
            Item.damage = 20;
            Item.useTime = 60;
        }
        else
        {
            Item.damage = 10;
            Item.useTime = 20;
        }
        return player.ownedProjectileCounts[Item.shoot] < 1;
    }
}