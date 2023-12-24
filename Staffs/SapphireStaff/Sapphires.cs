using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace RStaffsMod.Staffs.SapphireStaff
{
    public class Sapphires : ModProjectile
    {
        public int t,d;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.height = 10;
            Projectile.width = 10;
            DrawOffsetX = -4;
            DrawOriginOffsetY = -4;
            Projectile.penetrate = -1;
        }
        public override void AI()
        {
            Projectile.ai[0]++;
            t += 2;
            if(t > 90 && Projectile.ai[0] < 528)
            {
                t = 90;
            }
            Lighting.AddLight(Projectile.position, 0.090f, 0.576f, 0.917f);
            Player player = Main.player[Projectile.owner];
            double deg = (double)Projectile.ai[1]; 
            double rad = deg * (Math.PI / 180);
            double dist = t;
            Projectile.position.X = player.Center.X - (int)(Math.Cos(rad) * dist) - Projectile.width / 2;
            Projectile.position.Y = player.Center.Y - (int)(Math.Sin(rad) * dist) - Projectile.height / 2; 
            Projectile.ai[1] += 2.2f;

            if (Projectile.ai[0] >= 528) t += 6;
            if (Projectile.ai[0] >= 588) Projectile.alpha += 5;
            if (Projectile.ai[0] >= 900)
            {
                Projectile.Kill();
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
            
            if(target.knockBackResist != 0f)
            {
                float xcom = ((Projectile.Center.X - player.Center.X) * 0.05f) * target.knockBackResist;
                float ycom = ((Projectile.Center.Y - player.Center.Y) * 0.05f) * target.knockBackResist;
                if (new Vector2(xcom, ycom) != Vector2.Zero)
                {
                    if(xcom > (4.8f * target.knockBackResist))
                    {
                        xcom = 4.8f * target.knockBackResist;
                    }
                    else if (xcom < (-4.8f * target.knockBackResist))
                    {
                        xcom = -4.8f * target.knockBackResist;
                    }
                    if(ycom > (4.8f * target.knockBackResist))
                    {
                        ycom = 4.8f * target.knockBackResist;
                    }
                    else if (ycom < (-4.8f * target.knockBackResist))
                    {
                        ycom = -4.8f * target.knockBackResist;
                    }
                    if (!hit.Crit) target.velocity = new Vector2(xcom, ycom);
                    else if (hit.Crit) target.velocity = new Vector2(xcom, ycom) * 1.5f;
                    
                    for (int i = 1; i <= 30; i++)
                    {
                        Dust dust;
                        dust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemSapphire, xcom * 5, ycom * 5, 1)];
                        dust.noGravity = true;
                    }
                }
            }
            
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Mod.Assets.Request<Texture2D>("Staffs/SapphireStaff/SapTrail").Value;
            Vector2 value = new Vector2(Projectile.width, Projectile.height) / 2f;
            Color color = Color.White;
            color.A = 150;
            for (int k = Projectile.oldPos.Length - 1; k > 0; k--)
            {
                Vector2 vector = Projectile.oldPos[k] + value;
                if (!(vector == value))
                {
                    Vector2 vector2 = Projectile.oldPos[k - 1] + value;
                    float rot = (vector2 - vector).ToRotation() - (float)Math.PI / 2f;
                    if (t < 72)
                    {
                        d = (int)Vector2.Distance(vector2, vector);
                    }
                    else if (t >= 72)
                    {
                        d = (int)Vector2.Distance(vector2, vector) + 8;
                    }
                    Rectangle source = new Rectangle(0, 0, texture.Width, texture.Height + d);
                    Vector2 origin = source.Size() / 2;
                    float scale = 1f - (k * 0.04f);
                    Color color2 = color * (1f - (float)k / (float)Projectile.oldPos.Length);
                    DrawData drawData = new(texture, vector - Main.screenPosition, source, color2, rot, origin, scale, SpriteEffects.None, 1);
                    Main.EntitySpriteDraw(drawData);
                }
            }
            return true;
        }
    }
}
