using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RStaffsMod.Staffs.RubyStaff
{
    public class RuBolt : ModProjectile
    {
        public Vector2 pos;
        public int style;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Magic;
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            DrawOffsetX = -4;
            DrawOriginOffsetY = -4;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 1;
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.position, 0.8f, 0.088f, 0.088f);
            Projectile.ai[0]++;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            if (++Projectile.frameCounter >= 7)
            {
                Projectile.frame++;
                if (Projectile.frame >= 2)
                {
                    Projectile.frame = 0;
                }
                Projectile.frameCounter = 0;
            }
            Dust dust;
            dust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemRuby, -Projectile.velocity.X * 0.4f, -Projectile.velocity.Y * 0.4f)];
            dust.noGravity = true;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {  
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            for (int i = 0; i < 25; i++)
            {
                Vector2 speed = new Vector2(5.5f, 0).RotatedByRandom(180);
                Dust dust;
                dust = Main.dust[Dust.NewDust(Projectile.Center, 1, 1, DustID.GemRuby, speed.X, speed.Y, 0, default, 1.6f)];
                dust.noGravity = true; 
            }

            if (target.life <= 0 && !NPCID.Sets.ProjectileNPC[target.type])
            {
                for (int i = 0; i < 5; i++)
                {
                    float speedX = -3 + Main.rand.Next(0, 6);
                    float speedY = -9 + Main.rand.Next(0, 7);
                    Vector2 speed = new Vector2(speedX, speedY);
                    Projectile.NewProjectile(Projectile.GetSource_Death(),Projectile.position, speed, ModContent.ProjectileType<Rubies>(), (int)(Projectile.damage * 0.3f), 0, Projectile.owner);
                }
            }
            target.immune[Projectile.owner] = 5;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            for (int i = 0; i < 25; i++)
            {
                Vector2 speed = new Vector2(5.5f, 0).RotatedByRandom(180);
                Dust dust;
                dust = Main.dust[Dust.NewDust(Projectile.Center, 1, 1, DustID.GemRuby, speed.X, speed.Y, 0, default, 1.3f)];
                dust.noGravity = true;
            }
            
            return true;
        }
        public override void Kill(int timeLeft)
        {
            Projectile.NewProjectile(Projectile.GetSource_Death(),Projectile.Center, Vector2.Zero, ModContent.ProjectileType<BoltEx>(), (int)(Projectile.damage * 0.5f), Projectile.knockBack, Projectile.owner);
        }
    }
}
