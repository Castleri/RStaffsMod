using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RStaffsMod.Staffs.RubyStaff
{
    public class BoltEx : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_294";
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Magic;
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.friendly = true;
            DrawOffsetX = -11;
            DrawOriginOffsetY = -11;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 3;
            Projectile.tileCollide = false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.life <= 0 && !NPCID.Sets.ProjectileNPC[target.type])
            {
                for (int i = 0; i < 5; i++)
                {
                    float speedX = -3 + Main.rand.Next(0, 6);
                    float speedY = -9 + Main.rand.Next(0, 7);
                    Vector2 speed = new Vector2(speedX, speedY);
                    Projectile.NewProjectile(Projectile.GetSource_Death(),Projectile.position, speed, ModContent.ProjectileType<Rubies>(), 
                        (int)(Projectile.damage * 0.7f), 0, Projectile.owner);
                }
            }
        }
    }
}
