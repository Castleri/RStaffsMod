using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RStaffsMod.Staffs.AmethystStaff
{
    public class AmeCrystal : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.height = 10;
            Projectile.width = 10;
            DrawOriginOffsetY = -2;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.extraUpdates = 2;
            Projectile.alpha = 255;
        }
        public override void AI()
        {
            Projectile.alpha -= 25;
            Lighting.AddLight(Projectile.position, 0.64f, 0f, 0.92f);
            Projectile.ai[0]++;
            if (Projectile.ai[0] >= 50)
            {
                Projectile.velocity.Y += 0.35f;
            }
            if (Projectile.velocity.Y >= 14.5f)
            {
                Projectile.velocity.Y = 14.5f;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
            for (int i = 1; i <= 7; i++)
            {
                float x = Main.rand.NextFloat(-1, 1);
                float y = Main.rand.NextFloat(-1, 1);
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemAmethyst, x, y, 1);
            }
            return true;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
            for (int i = 1; i <= 7; i++)
            {
                float x = Main.rand.NextFloat(-1, 1);
                float y = Main.rand.NextFloat(-1, 1);
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemAmethyst, x, y, 1);
            }
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)

        {
            if (target.defense > Projectile.damage) modifiers.FlatBonusDamage += 5;
        }
    }
}
