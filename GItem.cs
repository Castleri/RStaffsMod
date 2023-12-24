using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace RStaffsMod
{
    public class GItem : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            int id = item.type;
            switch (id)
            {
                case ItemID.AmethystStaff:
                    item.damage = 8;
                    item.useTime = 16;
                    item.useAnimation = 16;
                    item.shoot = ModContent.ProjectileType<Staffs.AmethystStaff.AmeCrystal>();
                    item.mana = 4;
                    item.shootSpeed = 5.5f;
                    item.autoReuse = true;
                    item.UseSound = null;
                    item.rare = ItemRarityID.Blue;
                    break;
                case ItemID.TopazStaff:
                    item.damage = 15;
                    item.channel = true;
                    item.mana = 4;
                    item.autoReuse = true;
                    item.useTime = 36;
                    item.useAnimation = 36;
                    item.shoot = ModContent.ProjectileType<Staffs.TopazStaff.TopBolt>();
                    item.rare = ItemRarityID.Blue;
                    break;
                case ItemID.SapphireStaff:
                    item.autoReuse = true;
                    item.shoot = ModContent.ProjectileType<Staffs.SapphireStaff.SapBolt>();
                    item.useTime = 24;
                    item.useAnimation = 24;
                    item.rare = ItemRarityID.Blue;
                    break;
                case ItemID.EmeraldStaff:
                    item.damage = 20;
                    item.useTime = 9;
                    item.useAnimation = 27;
                    item.reuseDelay = 25;
                    item.rare = ItemRarityID.Blue;
                    item.shoot = ModContent.ProjectileType<Staffs.EmeraldStaff.EmBolt>();
                    item.shootSpeed = 9.75f;
                    item.knockBack = 4.5f;
                    break;
                case ItemID.RubyStaff:
                    item.autoReuse = true;
                    item.damage = 25;
                    item.shootSpeed = 8.5f;
                    item.UseSound = SoundID.Item45;
                    item.useTime = 24;
                    item.useAnimation = 24;
                    item.shoot = ModContent.ProjectileType<Staffs.RubyStaff.RuBolt>();
                    item.rare = ItemRarityID.Blue;
                    item.knockBack = 6.25f;
                    break;
                case ItemID.DiamondStaff:
                    item.damage = 17;
                    item.useAnimation = 21;
                    item.useTime = 21;
                    item.shoot = ModContent.ProjectileType<Staffs.DiamondStaff.DiaBolt>();
                    item.shootSpeed = 6.5f;
                    item.autoReuse = true;
                    item.knockBack = 4f;
                    item.rare = ItemRarityID.Blue;
                    item.mana = 7;
                    break;
                case ItemID.AmberStaff:
                    item.shoot = ModContent.ProjectileType<Staffs.AmberStaff.Crawler>();
                    item.knockBack = 1.75f;
                    item.damage = 13;
                    item.shootSpeed = 9.75f;
                    item.mana = 15;
                    item.useTime = 24;
                    item.useAnimation = 24;
                    break;
                case ItemID.ThunderStaff:
                    item.damage -= 2;
                    break;

            }
            base.SetDefaults(item);
        }
        public override bool AltFunctionUse(Item item, Player player)
        {
            int id = item.type;
            switch (id)
            {
                case ItemID.EmeraldStaff:
                    return true;
            }
            return base.AltFunctionUse(item, player);
        }

        public override bool CanUseItem(Item item, Player player)
        {
            int id = item.type;
            switch (id)
            {
                case ItemID.TopazStaff:
                    if (player.channel)
                    {
                        item.useAnimation -= 2;
                        item.useTime -= 2;
                        item.shootSpeed += 0.4f;
                        if (item.useTime == 16)
                        {
                            SoundEngine.PlaySound(SoundID.Item28);
                            for (int i = 0; i < 20; i++)
                            {
                                Dust.NewDust(player.position, player.width, player.height, DustID.GemTopaz);
                            }
                        }
                        if (item.useTime < 16) item.useTime = 16;
                        if (item.useAnimation < 16) item.useAnimation = 16;
                        if (item.shootSpeed > 9.2f) item.shootSpeed = 9.2f;
                    }
                    else if (!player.channel)
                    {
                        item.useAnimation = 36;
                        item.useTime = 36;
                        item.shootSpeed = 6.5f;
                    }
                    return true;
            }
            return base.CanUseItem(item, player);
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int id = item.type;
            switch (id)
            {
                case ItemID.AmethystStaff:
                    for (int i = 0; i < 2; i++)
                    {
                        Vector2 OffsetAm = Vector2.Normalize(velocity) * 25f;
                        if (!Collision.CanHit(position, 0, 0, position + OffsetAm, 0, 0))
                        {
                            position += OffsetAm;
                        }
                        Vector2 Speed = velocity.RotatedByRandom(MathHelper.ToRadians(10));
                        Projectile.NewProjectile(source, position, Speed, type, damage, knockback, player.whoAmI);
                    }
                    return false;
                case ItemID.TopazStaff:
                    Vector2 OffsetTo = Vector2.Normalize(velocity) * (item.useTime + 15);
                    if (Collision.CanHit(position, 0, 0, position + OffsetTo, 0, 0))
                    {
                        position += OffsetTo;
                    }
                    return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
                case ItemID.SapphireStaff:
                    Vector2 OffsetSap = Vector2.Normalize(velocity) * 60f;
                    if (Collision.CanHit(position, 0, 0, position + OffsetSap, 0, 0))
                    {
                        position += OffsetSap;
                    }
                    return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
                case ItemID.EmeraldStaff:
                    Vector2 OffsetEm = Vector2.Normalize(velocity) * 60f;
                    if (Collision.CanHit(position, 0, 0, position + OffsetEm, 0, 0))
                    {
                        position += OffsetEm;
                    }
                    if (player.altFunctionUse == 2)
                    {
                        if (player.ownedProjectileCounts[ModContent.ProjectileType<Staffs.EmeraldStaff.FieldStarter>()] >= 1)
                        {
                            return false;
                        }
                        for (int i = -1; i < 2; i += 2)
                        {
                            if (i == -1)
                            {
                                type = ModContent.ProjectileType<Staffs.EmeraldStaff.FieldStarter>();
                            }
                            else if (i == 1)
                            {
                                type = ModContent.ProjectileType<Staffs.EmeraldStaff.FieldStarter1>();
                            }
                            Vector2 speed = velocity.RotatedBy(0.785 * i);
                            Projectile.NewProjectile(source, position, speed, type, 0, 0, player.whoAmI);
                        }

                        return false;
                    }
                    return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
                case ItemID.RubyStaff:
                    Vector2 OffsetRu = Vector2.Normalize(velocity) * 60f;
                    if (Collision.CanHit(position, 0, 0, position + OffsetRu, 0, 0))
                    {
                        position += OffsetRu;
                    }
                    return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
                case ItemID.DiamondStaff:
                    Vector2 OffsetDia = Vector2.Normalize(velocity) * 55f;
                    if (Collision.CanHit(position, 0, 0, position + OffsetDia, 0, 0))
                    {
                        position += OffsetDia;
                    }
                    return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
                case ItemID.AmberStaff:
                    Vector2 OffsetAmb = Vector2.Normalize(velocity) * 35f;
                    if (Collision.CanHit(position, 0, 0, position + OffsetAmb, 0, 0))
                    {
                        position += OffsetAmb;
                    }
                    return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            int id = item.type;
            switch (id)
            {
                case ItemID.AmethystStaff:
                    var line739 = new TooltipLine(Mod, "", "Shoots 2 Amethyst Crystals with low range that ignore 5 defense");
                    tooltips.Add(line739);
                    break;
                case ItemID.TopazStaff:
                    var line740 = new TooltipLine(Mod, "", "Gains more speed the more you keep using it");
                    tooltips.Add(line740);
                    break;
                case ItemID.SapphireStaff:
                    var line741 = new TooltipLine(Mod, "", "Casts a spinning bolt that creates Sapphires around you\nto protect you when it hits an enemy");
                    tooltips.Add(line741);
                    break;
                case ItemID.EmeraldStaff:
                    var line742 = new TooltipLine(Mod, "", "Shoots emerald bolts with random curved paths\nRight Click to create a magic field that increases your magic powers");
                    tooltips.Add(line742);
                    break;
                case ItemID.RubyStaff:
                    var line743 = new TooltipLine(Mod, "", "Shoots a fast bolt with a small area of damage\nReleases explosive rubies when it kills an enemy");
                    tooltips.Add(line743);
                    break;
                case ItemID.DiamondStaff:
                    var line744 = new TooltipLine(Mod, "", "Casts small bolts that homes to close enemies");
                    tooltips.Add(line744);
                    break;
                case ItemID.AmberStaff:
                    var line3377 = new TooltipLine(Mod, "", "Summons Tomb Crawlers that bite enemies\nTomb Crawlers lose life when they hit enemies");
                    tooltips.Add(line3377);
                    break;
                case ItemID.ThunderStaff:
                    var line4062 = new TooltipLine(Mod, "", "Bolts Zap nearby enemies quickly");
                    tooltips.Add(line4062);
                    break;
            }
            base.ModifyTooltips(item, tooltips);
        }
    }
}
