using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace RStaffsMod.Staffs.AmberStaff
{
    public class Crawler : ModProjectile
    {
        private int Wait;
        
        private float maxSpeed;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DontAttachHideToAlpha[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }
        public override void SetDefaults() 
        {
            Projectile.DamageType = DamageClass.Magic;
            Projectile.width = 22;
            Projectile.height = 22;
            DrawOffsetX = -8;
            DrawOriginOffsetY = -12;
            Projectile.alpha = 12;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.frame = 0;
            Projectile.tileCollide = false;
            Projectile.hide = true;
            maxSpeed = 5.25f;
            Projectile.timeLeft = 600;
        }
        private int c;
        public override void AI()
        {
            if (didHit)
            {
                c++;
                if(c >= 6) { c = 0; didHit = false; }
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.ai[1]++;
            CheckCollideBlocks();
            Targetting();
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCsAndTiles.Add(index);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            //Manual Drawing for the crawlers to be centered
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Rectangle source = new Rectangle(0, (texture.Height / 3) * Projectile.frame, texture.Width, texture.Height / 3);
            Vector2 origin = source.Size() / 2f;
            Vector2 value = new Vector2(Projectile.width, Projectile.height) / 2;
            DrawData drawData = new(texture, Projectile.position + value - Main.screenPosition, source, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);

            Main.EntitySpriteDraw(drawData);
            
            return false;
        }
        public override void PostDraw(Color lightColor)
        {
            //Manual Drawing for the "life bar"
            Texture2D texture = TextureAssets.Hb1.Value;
            Texture2D texture2 = TextureAssets.Hb2.Value;
            Rectangle source = new Rectangle(0, 0, texture.Width - (int)(Projectile.ai[1] / 16.66f), texture.Height);
            Rectangle source2 = new Rectangle(0, 0, texture2.Width, texture2.Height);
            Vector2 origin = source.Size() / 2f;
            Vector2 origin2 = source2.Size() / 2f;
            Vector2 value = new Vector2(Projectile.width, Projectile.height) / 2;
            Color color = new Color(0f, 0f, 0f);
            Color tileColor = Lighting.GetColor((int)Projectile.position.X / 16, (int)Projectile.position.Y / 16);

            //Changing color depending on the life
            if (texture.Width - (int)(Projectile.ai[1] / 16.66f) > 28)
            {
                color.G = 150;
            }
            else if (texture.Width - (int)(Projectile.ai[1] / 16.66f) > 10)
            {
                color.G = 170;
                color.R = 255;
            }
            else
            {
                color.R = 165;
            }
            color.A = 150;
            if (tileColor.R <= 30 && tileColor.G <= 30 && tileColor.B <= 30) color *= 0;
            DrawData dat1 = new(texture2, Projectile.position + value + new Vector2(0, 20) - Main.screenPosition, source2, color, 0, origin2, Projectile.scale, SpriteEffects.None, 0f);
            DrawData dat2 = new(texture, Projectile.position + value + new Vector2(0 - (int)(Projectile.ai[1] / 28.66f), 20) - Main.screenPosition, source, color, 0, origin, Projectile.scale, SpriteEffects.None, 0f);
            //spriteBatch.Draw(texture, projectile.position + value + new Vector2(0 - (int)(projectile.ai[1] / 28.66f), 20) - Main.screenPosition, source, color, 0, origin, projectile.scale, SpriteEffects.None, 0f);
            Main.EntitySpriteDraw(dat1);
            Main.EntitySpriteDraw(dat2);
        }
        private bool didHit;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            didHit = true;
            Projectile.frame = 2;
            Projectile.netUpdate = true;
            switch (target.type)
            {
                case 30:
                    Projectile.Kill();
                    break;
                default:
                    if (hit.Crit)
                    {
                        float damage1 = hit.Damage;
                        Projectile.ai[1] += (int)damage1;
                        Projectile.timeLeft -= (int)damage1;
                        CombatText.NewText(new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height), Color.Red, (int)damage1);
                    }
                    else
                    {
                        float damage1 = hit.Damage * 1.5F;
                        Projectile.ai[1] += damage1;
                        Projectile.timeLeft -= (int)damage1;
                        CombatText.NewText(new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height), Color.OrangeRed, (int)damage1);
                    }
                    break;
            }
        }
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath1,Projectile.position);
            Gore.NewGore(Projectile.GetSource_Death(), Projectile.position, Projectile.velocity, GoreID.TombCrawlerHead);
            Gore.NewGore(Projectile.GetSource_Death(), Projectile.position, Projectile.velocity, GoreID.TombCrawlerTail);
        }

        private void CheckCollideBlocks()
        {
            //Since the crawlers have tileCollide = false, manual check to see if the hitbox is colliding with tiles to see if the
            //search and targeting takes place
            if (Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
            {
                Projectile.frame = 0;
                Projectile.ai[0]++;
                if (Projectile.ai[0] >= 15)
                {
                    Projectile.ai[0] = 0;
                    SoundEngine.PlaySound(SoundID.WormDig with { Volume = Main.soundVolume * 0.4f }, Projectile.position);
                }
                Wait++;
            }
            //If it is not colliding, gravity takes place
            else
            {
                if (!didHit)
                    Projectile.frame = 1;

                Projectile.ai[0] = 0;
                Projectile.velocity.Y += 0.2f;
                Wait = 0;
            }
        }
        private void Targetting()
        {
            if (Wait > 8)
            {
                bool found = false;
                Vector2 centergo = Projectile.position;
                Player player = Main.player[Projectile.owner];
                float distance = 640;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy())
                    {
                        float dpt = Vector2.Distance(player.Center, npc.Center);
                        bool inRange = dpt < distance;
                        if ((inRange && !found) || dpt < distance)
                        {
                            distance = dpt;
                            found = true;
                            centergo = npc.Center;
                        }
                    }
                }
                if (found)
                {
                    float accel;
                    accel = 0.6f;
                    maxSpeed = 5.25f;
                    Projectile.velocity = Vector2.Normalize(centergo - Projectile.Center) * accel + Projectile.oldVelocity;
                }

                else
                {
                    maxSpeed = 4f;
                    Projectile.velocity = (Vector2.Normalize(player.Center - Projectile.Center) * 0.4f) + Projectile.oldVelocity;
                }

                if (Projectile.velocity.X > maxSpeed) Projectile.velocity.X = maxSpeed;
                else if (Projectile.velocity.X < -maxSpeed) Projectile.velocity.X = -maxSpeed;
                if (Projectile.velocity.Y > maxSpeed) Projectile.velocity.Y = maxSpeed;
                else if (Projectile.velocity.Y < -maxSpeed) Projectile.velocity.Y = -maxSpeed;

            }
        }
    }
}
