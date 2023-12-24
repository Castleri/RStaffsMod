using System;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Terraria.Audio;
using System.Collections.Generic;
    

namespace RStaffsMod.Staffs.ThunderZap
{
    public class Zap : ModProjectile
    {
        BoltHelper helper;
        public List<int> list = new List<int>();
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Magic;
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 19;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            if (zapped)
            {
                Texture2D mid = Mod.Assets.Request<Texture2D>("Staffs/ThunderZap/Zap").Value;
                Texture2D end = Mod.Assets.Request<Texture2D>("Staffs/ThunderZap/ZapEnd").Value;

                if (helper != null)   helper.Draw(Main.spriteBatch, end, mid);
                //Main.NewText(Main.screenPosition - Main.screenLastPosition);
            }
            //Main.spriteBatch.End();
            return false;
        }
        bool zapped,dont;
        Vector2 save1, save2;
        int timer;
        public override void AI()
        {
            if (list == null)
            {
                list.Add((int)Projectile.ai[0]);
            }
            if (!zapped && !dont)
            {
                list.Add((int)Projectile.ai[0]);
                save1 = Main.npc[(int)Projectile.ai[0]].Center;
                save2 = Main.screenPosition;
                bool foundone = false;
                float currentdistance = 128;
                NPC gotothisnpc = Main.npc[(int)Projectile.ai[0]];
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (list.Contains(i)) continue;
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy())
                    {
                        float dist = Vector2.Distance(Projectile.position,Main.npc[i].Center);
                        bool inrange = dist < currentdistance;
                        if((inrange && !foundone) || inrange)
                        {
                            foundone = true;
                            currentdistance = dist;
                            gotothisnpc = Main.npc[i];
                        }
                    }
                }
                if(foundone)
                {
                    Projectile.position = gotothisnpc.Center;
                    list.Add((int)Projectile.ai[0]);
                    dont = false;
                }
                else
                {
                    Projectile.position += Vector2.One.RotatedBy(Main.rand.NextFloat(0,(float)Math.PI * 2)) * 22;
                    dont = true;
                }
                zapped = true;
            }
            if (zapped)
            {
                if (helper == null || (timer == 0 && !dont))
                {
                    helper = new(save1 - Main.screenPosition, Projectile.position - Main.screenPosition);
                }
                else helper.Update(offset: (save1 - save2) - (save1 - Main.screenPosition));
                timer++;
                if (timer >= 4)
                {
                    timer = 0;
                    zapped = false;
                }
            }
        }
        public override bool? CanHitNPC(NPC target)
        {
            if (list.Contains(target.whoAmI)) return false;
            return base.CanHitNPC(target);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.ai[0] = target.whoAmI;
        }
    }
}
