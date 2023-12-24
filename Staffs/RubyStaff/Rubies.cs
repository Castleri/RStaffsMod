using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace RStaffsMod.Staffs.RubyStaff
{
    public class Rubies : ModProjectile
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.Ruby; 
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = true;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.penetrate = 1;
            DrawOffsetX = -2;
            DrawOriginOffsetY = -4;
        }
        public override void AI()
        {
            Projectile.ai[0]++;
            if(Projectile.ai[0] >= 9)
            {
                Projectile.velocity.Y += 0.4f;
                if (Projectile.velocity.Y > 16f)
                {
                    Projectile.velocity.Y = 16f;
                }
            }
            Projectile.rotation += 0.2f * Projectile.direction;
        }
        public override bool? CanHitNPC(NPC target)
        {
            if(Projectile.ai[0] >= 2)
            {
                return null;
            }
            else
            {
                return false;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.NewProjectile(Projectile.GetSource_Death(),Projectile.position, Vector2.Zero, ModContent.ProjectileType<RubyExplosion>(), Projectile.damage, 2, Projectile.owner);
            target.immune[Projectile.owner] = 8;
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position, Vector2.Zero, ModContent.ProjectileType<RubyExplosion>(), Projectile.damage, 2, Projectile.owner);

            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            return base.OnTileCollide(oldVelocity);
        }
    }
}
