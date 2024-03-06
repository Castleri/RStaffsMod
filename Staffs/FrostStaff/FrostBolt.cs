using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace RStaffsMod.Staffs.FrostStaff
{
    public class FrostBolt : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Magic;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 2;
            Projectile.friendly = true;
            DrawOffsetX = -2;
            DrawOriginOffsetY = -8;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Mod.Assets.Request<Texture2D>("Staffs/FrostStaff/FrostBoltShine").Value;
            Rectangle source = new Rectangle(0, 0, texture.Width, texture.Height);
            Vector2 value = new Vector2(Projectile.width, Projectile.height) / 2f;
            Vector2 origin = source.Size() / 2f;
            Color color = Color.LightCyan;
            color.A = 255;
            DrawData data = new(texture, Projectile.position + value - Main.screenPosition, source, color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
            Main.EntitySpriteDraw(data);
            return true;
        }
    }
}
