using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace RStaffsMod.Staffs.TopazStaff
{
    public class TopBolt : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.penetrate = 5;
            DrawOffsetX = -8;
            DrawOriginOffsetY = -2;
            Projectile.alpha = 50;
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.position, 0.9f, 0.735f, 0f);
            Projectile.rotation = Projectile.velocity.ToRotation();
            Dust dust;
            dust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemTopaz, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
            dust.noGravity = true;

            if (++Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= 3)
                {
                    Projectile.frame = 0;
                }
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.penetrate > 1) target.immune[Projectile.owner] = 10;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            return true;
        }
    }
}
