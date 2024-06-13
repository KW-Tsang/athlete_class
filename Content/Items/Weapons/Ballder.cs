using AthleteClass.Content.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AthleteClass.Content.Items.Weapons;

public class Ballder : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 48;
        Item.height = 48;
        Item.scale = 0.75f;
        Item.noMelee = true;
        Item.noUseGraphic = true;
        
        Item.value = 0;
        
        //placeholder
        Item.damage = 50;
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

    public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] < 1;
}