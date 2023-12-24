using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;
using System;
using Terraria.DataStructures;

namespace RStaffsMod.Staffs.EmeraldStaff
{
    public class EmBolt : ModProjectile
    {
        private float dev;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = true;
            Projectile.width = 14;
            Projectile.height = 14;
            DrawOffsetX = -4;
            DrawOriginOffsetY = -4;
            Projectile.penetrate = 1;
            Projectile.usesLocalNPCImmunity = true;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Lighting.AddLight(Projectile.position, 0.56f, 1f, 0.32f);
            Projectile.ai[0]++;
            Projectile.ai[1]++;

            if (Projectile.ai[1] == 1) dev = Main.rand.NextFloat(-0.37f, 0.37f);
            else if (Projectile.ai[1] > 5 && Projectile.ai[1] <= 25) Projectile.velocity += new Vector2(0, dev).RotatedBy(Projectile.rotation);

            if (Projectile.ai[0] >= 480)
            {
                Projectile.alpha += 5;
                if(Projectile.alpha > 255)
                {
                    Projectile.alpha = 255;
                    Projectile.Kill();
                }
            }
            for (int i = 0; i < 2; i++)
            {
                Dust dust;
                dust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemEmerald)];
                dust.noGravity = true;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {

            Texture2D texture = Mod.Assets.Request<Texture2D>("Staffs/EmeraldStaff/EmBoltTrail").Value;
            Rectangle source = new Rectangle(0, 0, texture.Width, texture.Height);
            Vector2 origin = source.Size() / 2f;
            Vector2 value = new Vector2(Projectile.width, Projectile.height) / 2;
            Color color = Projectile.GetAlpha(Color.GhostWhite);
            color.A = 205;
            for (int k = Projectile.oldPos.Length - 1; k > 0; k--)
            {
                Vector2 vector = Projectile.oldPos[k] + value;
                if (!(vector == value))
                {
                    Vector2 vector2 = Projectile.oldPos[k - 1] + value;
                    float rot = (vector2 - vector).ToRotation() - MathHelper.Pi / 2;
                    Color color1 = color * (0.9f - (float)k / (float)Projectile.oldPos.Length);
                    DrawData drawData = new(texture, vector - Main.screenPosition, source, color1, rot, origin, 1f, SpriteEffects.None, 0);
                    /*
                    if(Main.netMode != NetmodeID.Server)
                    {
                        Main.spriteBatch.End();
                        Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                        GameShaders.Misc["EmShader"].Apply();
                    }
                    */
                    Main.EntitySpriteDraw(drawData);
                }
            }
            return true;
        }
        public override void PostDraw(Color lightColor)
        {
            if (Main.netMode != NetmodeID.Server)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

            for (int i = 0; i < 20; i++)
            {
                Vector2 speed = new Vector2(5.5f, 0).RotatedByRandom(180);
                Dust dust;
                dust = Main.dust[Dust.NewDust(Projectile.Center, 1, 1, DustID.GemEmerald, speed.X, speed.Y, 0, default, 1.3f)];
                dust.noGravity = true;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            for (int i = 0; i < 15; i++)
            {
                Vector2 speed = new Vector2(5.5f, 0).RotatedByRandom(180);
                Dust dust;
                dust = Main.dust[Dust.NewDust(Projectile.Center, 1, 1, DustID.GemEmerald, speed.X, speed.Y, 0, default, 1.3f)];
                dust.noGravity = true;
            }
            return true;
        }
    }
}
