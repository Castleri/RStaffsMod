using Terraria.ModLoader;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace RStaffsMod
{
    public class GProj : GlobalProjectile
    {
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if(projectile.type == ProjectileID.ThunderStaffShot)
            {
                int npchitted = target.whoAmI;
                if(Main.myPlayer == projectile.owner)
                {
                    SoundStyle zap = new SoundStyle("RStaffsMod/Assets/Sounds/Zap");
                    SoundEngine.PlaySound(zap.WithVolumeScale(Main.soundVolume), target.position);
                    Projectile.NewProjectile(Projectile.GetSource_None(), target.Center, Vector2.Zero, ModContent.ProjectileType<Staffs.ThunderZap.Zap>(), (int)(projectile.damage * 0.40f), 0, projectile.owner, npchitted);
                }
            }
        }
    }
}
