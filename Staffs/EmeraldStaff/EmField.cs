using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;
using System;
using System.Linq;

namespace RStaffsMod.Staffs.EmeraldStaff
{
    public class EmField : ModProjectile
    {
        public float t,n;
        public bool can = true;
        public bool field = true;
        public int fstarter = ModContent.ProjectileType<FieldStarter>();
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = true;
            Projectile.width = 14;
            Projectile.height = 14;
            DrawOffsetX = -4;
            DrawOriginOffsetY = -4;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 4000;
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.position, 0.56f, 1f, 0.32f);
            Projectile.ai[1]++;
            t++;


            Player player = Main.player[Projectile.owner];
            Player player2 = Main.player[Main.myPlayer];
            if (t >= 20 && player.ownedProjectileCounts[Projectile.type] > 1)
            {
                Projectile.Kill();
            }
            if (t >= 20)
            {

                Projectile.ai[0]++;
                Projectile.rotation += 0.025f;
                can = false;
                Projectile.penetrate = -1;

                if(Projectile.ai[0] >= 30)
                {
                    for (int i = 1; i < 360; i += 2)
                    {
                        double deg = i * 2;
                        double rad = deg * (Math.PI / 180);
                        double dist = 640;

                        float posX = Projectile.Center.X - (int)(Math.Cos(rad) * dist);
                        float posY = Projectile.Center.Y - (int)(Math.Sin(rad) * dist);

                        Dust dust2;
                        Vector2 position = new Vector2(posX, posY);
                        dust2 = Dust.NewDustPerfect(position, 89, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 1f);
                        dust2.noGravity = true;

                    }
                    Projectile.ai[0] = 0;
                }
                float dpp = Vector2.Distance(player2.Center, Projectile.Center);
                if (dpp <= 640)
                {
                    if (!player2.HasBuff(ModContent.BuffType<Buffs.EmBuff>()))
                    {
                        player2.AddBuff(ModContent.BuffType<Buffs.EmBuff>(), 18000);
                    }
                }
                else
                {
                    player2.ClearBuff(ModContent.BuffType<Buffs.EmBuff>());
                }
            }
            var check = Main.projectile.Where(starter => starter.active && starter.type == fstarter && starter.active);
            if (player.altFunctionUse == 2 && player.HeldItem.type == ItemID.EmeraldStaff && check.Any())
            {
                Projectile.timeLeft = 279;
            }

            if (Projectile.timeLeft <= 280)
            {
                Projectile.alpha += 15;
                if (Projectile.alpha >= 255) Projectile.Kill();
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
    }
}
