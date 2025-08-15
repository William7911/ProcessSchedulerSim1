using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ProcessSchedulerSim
{
    public sealed class GanttControl : Control
    {
        public List<ScheduledSlice> Data { get; } = new();
        public int TimeScale { get; set; } = 40; // px per time unit
        public int RowHeight { get; set; } = 34;

        public GanttControl()
        {
            DoubleBuffered = true;
            BackColor = Color.White;
            ForeColor = Color.Black;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.Clear(BackColor);
            using var axisPen = new Pen(Color.Gray, 1);
            using var borderPen = new Pen(Color.Black, 1);
            using var font = new Font(FontFamily.GenericSansSerif, 9f);

            // axis
            g.DrawLine(axisPen, 50, 10, 50, Height - 10);
            g.DrawLine(axisPen, 50, Height - 30, Width - 10, Height - 30);

            int y = 40;
            foreach (var s in MergeAdjacent(Data))
            {
                int x = 50 + s.Start * TimeScale;
                int w = Math.Max(1, (s.End - s.Start) * TimeScale);
                var rect = new Rectangle(x, y, w, RowHeight);
                var color = ColorFromPID(s.PID);
                using var fill = new SolidBrush(Color.FromArgb(220, color));
                g.FillRectangle(fill, rect);
                g.DrawRectangle(borderPen, rect);
                g.DrawString($"{s.PID} [{s.Start}-{s.End}]", font, Brushes.Black, rect.X+4, rect.Y+4);
            }

            int max = Data.Count == 0 ? 10 : Data.Max(s => s.End) + 1;
            for (int t = 0; t <= max; t++)
            {
                int tx = 50 + t * TimeScale;
                g.DrawLine(axisPen, tx, Height - 30, tx, Height - 25);
                g.DrawString(t.ToString(), font, Brushes.Black, tx - 4, Height - 24);
            }
        }

        private static List<ScheduledSlice> MergeAdjacent(List<ScheduledSlice> data)
        {
            var res = new List<ScheduledSlice>();
            foreach (var s in data.OrderBy(d => d.Start))
            {
                if (res.Count == 0) { res.Add(new ScheduledSlice { PID = s.PID, Start = s.Start, End = s.End }); continue; }
                var last = res[^1];
                if (last.PID == s.PID && last.End == s.Start) last.End = s.End;
                else res.Add(new ScheduledSlice { PID = s.PID, Start = s.Start, End = s.End });
            }
            return res;
        }

        private static Color ColorFromPID(string pid)
        {
            int hash = pid.Aggregate(17, (acc, ch) => acc * 31 + ch);
            var r = (hash & 0xFF);
            var g = (hash >> 8) & 0xFF;
            var b = (hash >> 16) & 0xFF;
            return Color.FromArgb(120 + (r % 136), 120 + (g % 136), 120 + (b % 136));
        }
    }
}
