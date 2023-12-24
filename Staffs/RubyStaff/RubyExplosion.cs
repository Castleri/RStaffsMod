using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RStaffsMod.Staffs.RubyStaff
{
    public class RubyExplosion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = true;
            Projectile.width = 128;
            Projectile.height = 128;
            Projectile.timeLeft = 15;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 14;
            Projectile.alpha = 55;
            Projectile.scale = 0.7f;
            DrawOffsetX = -19;
            DrawOriginOffsetY = -16;
        }
        public override void AI()
        {
            Projectile.ai[0]++;
            if (++Projectile.frameCounter >= 3)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
        }
        public override bool? CanHitNPC(NPC target)
        {
            if (Projectile.ai[0] < 6)
            {
                return null;
            }
            else return false;
        }
    }
}
