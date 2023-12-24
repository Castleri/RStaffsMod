using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace RStaffsMod.Staffs.SapphireStaff
{
    public class SapBolt : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.ignoreWater = false;
        }
        public override void AI()
        {

            Projectile.rotation += 0.85f * Projectile.direction;

            Lighting.AddLight(Projectile.position, 0f, 0.48f, 0.896f);
            Dust dust;
            dust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemSapphire)];
            dust.noGravity = true;


        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Sapphires>()] < 1)
            {
                float numproj = 8;
                float rotation = MathHelper.ToRadians(180);
                for (int a = 0; a < numproj; a++)
                {
                    Vector2 Speed = new Vector2(0.1f, 0.1f).RotatedBy(MathHelper.Lerp(-rotation, rotation, a / (numproj)));
                    Projectile.NewProjectile(Projectile.GetSource_Death(),Projectile.Center, Speed, ModContent.ProjectileType<Sapphires>(), (int)(Projectile.damage * 0.8f), 0, Projectile.owner, 0, (float)(a * 45));
                }
                SoundEngine.PlaySound(SoundID.Item28);
                for (int i = 0; i < 20; i++)
                {
                    Dust.NewDust(player.position, player.width, player.height, DustID.GemSapphire);
                }
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            return true;
        }
    }
}
