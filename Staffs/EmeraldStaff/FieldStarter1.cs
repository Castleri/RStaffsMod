using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace RStaffsMod.Staffs.EmeraldStaff
{
    public class FieldStarter1 : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_294";
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
            Projectile.velocity += new Vector2(0, -0.40f).RotatedBy(Projectile.rotation);
            Dust trail;
            trail = Dust.NewDustPerfect(Projectile.position, DustID.GemEmerald, null, 0, default, 1f);
            trail.noGravity = true;
        }
    }
}
