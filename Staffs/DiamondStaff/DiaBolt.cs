using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace RStaffsMod.Staffs.DiamondStaff
{
    public class DiaBolt : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.width = 6;
            Projectile.height = 6;
            DrawOffsetX = -2;
            DrawOriginOffsetY = -2;
            Projectile.penetrate = 3;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 720;
        }
        public override void AI()
        {
            Projectile.ai[0]++;
            Projectile.ai[1]++;
            if(Projectile.ai[1] >= 3)
            {
                Dust dust;
                dust = Dust.NewDustPerfect(Projectile.Center, DustID.GemDiamond, new Vector2(0, 0));
                dust.noGravity = true;
                Projectile.ai[1] = 0;
            }
            Projectile.rotation = Projectile.velocity.ToRotation();

            //Wait 10 frames to start homing
            if(Projectile.ai[0] >= 10)
            {
                float d = 128;
                bool targetfound = false;
                Vector2 targetcenter = Projectile.position;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy())
                    {
                        float dpt = Vector2.Distance(Projectile.Center, npc.Center);
                        bool inRange = dpt < d;
                        if ((inRange && !targetfound) || dpt < d)
                        {
                            d = dpt;
                            targetfound = true;
                            targetcenter = npc.Center;
                        }
                    }
                }
                if (targetfound)
                {
                    Projectile.velocity = ((Vector2.Normalize(targetcenter - Projectile.Center)) + Projectile.oldVelocity * 0.87f);

                    float velx = Math.Abs(Projectile.velocity.X);
                    float vely = Math.Abs(Projectile.velocity.Y);
                    if (velx > 6.5f)
                    {
                        float direction = Math.Abs(Projectile.velocity.X) / Projectile.velocity.X;
                        Projectile.velocity.X = 6.5f * direction;
                    }
                    if (vely > 6.5f)
                    {
                        float direction = Math.Abs(Projectile.velocity.Y) / Projectile.velocity.Y;
                        Projectile.velocity.Y = 6.5f * direction;
                    }
                }

                else
                {
                    Projectile.velocity = new Vector2(6.5f, 0).RotatedBy(Projectile.rotation);
                }
            }
            
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            Texture2D texture = Mod.Assets.Request<Texture2D>("Staffs/DiamondStaff/DiaBolt").Value;
            Rectangle source = new Rectangle(0, 0, texture.Width, texture.Height);
            Vector2 value = new Vector2(Projectile.width, Projectile.height) / 2f;
            Vector2 origin = source.Size() / 2f;
            Color color = Projectile.GetAlpha(Color.GhostWhite);
            DrawData data = new(texture, Projectile.position + value - Main.screenPosition, source, color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
            Main.EntitySpriteDraw(data);
            return false;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
        public override void Kill(int timeLeft)
        {
            for(int i = 0; i < 4; i++)
            {
                Vector2 speed = new Vector2(0, -3).RotatedBy(MathHelper.PiOver2 * i);
                Dust dust;
                dust = Dust.NewDustPerfect(Projectile.position, DustID.GemDiamond, speed, 0, default, 1.4f);
                dust.noGravity = true;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.ai[0] = 0;
            target.immune[Projectile.owner] = 10;
            if (target.life <= 0) Projectile.Kill();
        }
    }
}
