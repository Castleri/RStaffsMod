﻿using Terraria;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;

namespace RStaffsMod.Staffs.EmeraldStaff
{
    public class FieldStarter : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_294";
        public int starter1 = ModContent.ProjectileType<FieldStarter1>();
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.width = 8;
            Projectile.height = 8;
            
        }
        public override void AI()
        {
            Projectile.ai[0]++;
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.velocity += new Vector2(0, 0.40f).RotatedBy(Projectile.rotation);

            //Check colliding between the two projectiles
            
            var checkcollide = Main.projectile.Where(starter => starter.active && starter.type == starter1 && starter.active && (starter.Hitbox.Intersects(Projectile.Hitbox)));
            if (checkcollide.Any() && Projectile.ai[0] > 10)
            {
                foreach (var proj in checkcollide)
                {
                    proj.Kill();
                    Projectile.NewProjectile(Projectile.GetSource_Death(),Projectile.position, Vector2.Zero, ModContent.ProjectileType<EmField>(), 0, 0, Projectile.owner);
                    SoundEngine.PlaySound(SoundID.Item4);
                    for (int a = 0; a < 8; a++)
                    {
                        for (int b = 1; b < 5; b++)
                        {
                            Vector2 speed = new Vector2(0, -(3 + b)).RotatedBy(MathHelper.PiOver4 * a);
                            Dust dust;
                            dust = Dust.NewDustPerfect(Projectile.position, DustID.GemEmerald, speed, 0, default, 1.4f);
                            dust.noGravity = true;
                        }
                    }
                    Projectile.Kill();
                }
            }



            Dust trail;
            trail = Dust.NewDustPerfect(Projectile.position, DustID.GemEmerald,null,0,default,1f);
            trail.noGravity = true;
        }
    }
}
