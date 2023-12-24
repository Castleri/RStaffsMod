using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using RStaffsMod;

namespace RStaffsMod.Staffs.ThunderZap
{
    public class BoltHelper
    {
        public List<Line> parts = new List<Line>();
        public float Alpha { get; set; }
        public float FadeOutRate { get; set; }
        public Vector2 ScreenChange { get; set; }
        public Color tone { get; set; }
        public bool IsComplete { get { return Alpha <= 0; } }
        public BoltHelper(Vector2 source, Vector2 dest) : this(source, dest, new Color(95, 205, 228)) { }
        public BoltHelper(Vector2 source, Vector2 dest, Color color)
        {
            parts = CreateBolt(source, dest, 4);
            tone = color;
            Alpha = 0.75f;
            FadeOutRate = 0.04f;
            ScreenChange = Vector2.Zero;
        }
        public void Draw(SpriteBatch spriteBatch, Texture2D end, Texture2D body)
        {
            if (Alpha <= 0)
                return;

            foreach (var segment in parts)
                segment.Draw(spriteBatch, tone * (Alpha), end, body, ScreenChange);
        }
        public virtual void Update(Vector2 offset)
        {
            Alpha -= FadeOutRate;
            ScreenChange = offset;
        }
        protected static List<Line> CreateBolt(Vector2 source, Vector2 dest, float thickness)
        {
            var results = new List<Line>();
            Vector2 tangent = dest - source;
            Vector2 normal = Vector2.Normalize(new Vector2(tangent.Y, -tangent.X));
            float length = tangent.Length();
            List<float> positions = new List<float>();
            positions.Add(0);
            for (int i = 0; i < length / 4; i++)
                positions.Add(Main.rand.NextFloat(0, 1));
            positions.Sort();
            const float Sway = 80;
            const float Jaggedness = 1 / Sway;
            Vector2 prevPoint = source;
            float prevDisplacement = 0;
            for (int i = 1; i < positions.Count; i++)
            {
                float pos = positions[i];
                // used to prevent sharp angles by ensuring very close positions also have small perpendicular variation. 
                float scale = (length * Jaggedness) * (pos - positions[i - 1]);
                // defines an envelope. Points near the middle of the bolt can be further from the central line. 
                float envelope = pos > 0.95f ? 20 * (1 - pos) : 1;
                float displacement = Main.rand.NextFloat(-Sway, Sway);
                displacement -= (displacement - prevDisplacement) * (1 - scale);
                displacement *= envelope;
                Vector2 point = source + pos * tangent + displacement * normal;
                results.Add(new Line(prevPoint, point, thickness));
                prevPoint = point;
                prevDisplacement = displacement;
            }
            results.Add(new Line(prevPoint, dest, thickness));
            return results;
        }
    }
    public class Line
    {
        public Vector2 A;
        public Vector2 B;
        public float thickness;

        public Line() { }
        public Line(Vector2 a, Vector2 b, float thickness)
        {
            A = a;
            B = b;
            this.thickness = thickness;
        }
        public void Draw (SpriteBatch spriteBatch, Color color, Texture2D end, Texture2D body, Vector2 offset)
        {
            Vector2 tangent = B - A;
            float rotation = (B - A).ToRotation();

            const float ImageWidth = 3;
            float thickscale = thickness / ImageWidth;

            Vector2 originExtremes = new (end.Width,end.Height / 2f);
            Vector2 originBody = new (0, body.Height / 2f);
            Vector2 bodyScale = new(tangent.Length(), thickscale);

            spriteBatch.Draw(body, A - offset, null, color, rotation, originBody, bodyScale, SpriteEffects.None, 0f);
            spriteBatch.Draw(end, A - offset, null, color, rotation, originExtremes, thickscale, SpriteEffects.None, 0f);
            spriteBatch.Draw(end, A - offset, null, color, rotation + MathHelper.Pi, originExtremes, thickscale, SpriteEffects.None, 0f);
        }
    }
}
