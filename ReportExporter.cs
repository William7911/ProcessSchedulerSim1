using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace ProcessSchedulerSim
{
    public static class ReportExporter
    {
        public static void ExportCsv(string path, IEnumerable<ProcessItem> completed, ScheduleResult result)
        {
            using var sw = new StreamWriter(path, false, new UTF8Encoding(true));
            sw.WriteLine("PID,Arrival,Burst,Start,Finish,Waiting,Turnaround");
            foreach (var p in completed)
            {
                sw.WriteLine(string.Join(',', new[]
                {
                    p.PID,
                    p.Arrival.ToString(CultureInfo.InvariantCulture),
                    p.Burst.ToString(CultureInfo.InvariantCulture),
                    p.StartTime.ToString(CultureInfo.InvariantCulture),
                    p.FinishTime.ToString(CultureInfo.InvariantCulture),
                    p.WaitingTime.ToString(CultureInfo.InvariantCulture),
                    p.TurnaroundTime.ToString(CultureInfo.InvariantCulture)
                }));
            }
            sw.WriteLine();
            sw.WriteLine($"Avg Waiting,{result.AvgWaiting:F2}");
            sw.WriteLine($"Avg Turnaround,{result.AvgTurnaround:F2}");
            sw.WriteLine($"Throughput (proc/unit),{result.Throughput:F4}");
            sw.WriteLine($"Makespan,{result.Makespan}");
        }

        public static void ExportPdf(string path, IEnumerable<ProcessItem> completed, ScheduleResult result, IList<ScheduledSlice> timeline)
        {
            // Simple text PDF (no external libs) + rudimentary gantt (not perfect but useful)
            var lines = new List<string>
            {
                "Simulador de Gestión de Procesos — Reporte",
                $"Fecha: {DateTime.Now}",
                "",
                "Tabla de procesos (PID, Arrival, Burst, Start, Finish, Waiting, Turnaround)"
            };
            foreach (var p in completed)
            {
                lines.Add($"{p.PID}\t{p.Arrival}\t{p.Burst}\t{p.StartTime}\t{p.FinishTime}\t{p.WaitingTime}\t{p.TurnaroundTime}");
            }
            lines.Add("");
            lines.Add($"Avg Waiting: {result.AvgWaiting:F2}");
            lines.Add($"Avg Turnaround: {result.AvgTurnaround:F2}");
            lines.Add($"Throughput (proc/unit): {result.Throughput:F4}");
            lines.Add($"Makespan: {result.Makespan}");

            SimplePdfWriter.WriteTextPdfWithGantt(path, lines, timeline);
        }
    }

    internal static class SimplePdfWriter
    {
        public static void WriteTextPdfWithGantt(string path, IEnumerable<string> lines, IList<ScheduledSlice> timeline)
        {
            const int width = 595;
            const int height = 842;
            const int margin = 50;
            const int leading = 14;

            // build content stream (very simplified)
            var contentSb = new StringBuilder();
            contentSb.AppendLine("BT");
            contentSb.AppendLine("/F1 12 Tf");
            int y = height - margin;
            foreach (var line in lines)
            {
                contentSb.AppendLine($"{margin} {y} Td");
                contentSb.AppendLine($"({Escape(line)}) Tj");
                y -= leading;
            }
            contentSb.AppendLine("ET");

            // add simple gantt rectangles after text
            int ganttTop = Math.Max(200, y - 20);
            int scale = 18;
            int max = timeline.Count == 0 ? 0 : Math.Max(1, timeline.Max(s => s.End));

            var ganttSb = new StringBuilder();
            ganttSb.AppendLine("0 0 0 RG 0 0 0 rg");
            int boxW = Math.Min(width - 2*margin, margin + max*scale);
            ganttSb.AppendLine($"{margin} {ganttTop} {boxW} 60 re S");

            foreach (var s in timeline)
            {
                var c = ColorFromPID(s.PID);
                ganttSb.AppendLine($"{c.r:F3} {c.g:F3} {c.b:F3} rg");
                int x = margin + s.Start * scale;
                int w = Math.Max(1, (s.End - s.Start) * scale);
                int yy = ganttTop + 5;
                ganttSb.AppendLine($"{x} {yy} {w} 40 re f");
            }

            var content = contentSb.ToString() + "\n" + ganttSb.ToString();
            var bytes = Encoding.ASCII.GetBytes(content);

            using var fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            using var writer = new StreamWriter(fs, Encoding.ASCII);
            writer.Write("%PDF-1.4\n");
            writer.Flush();
            long pos = fs.Position;
            writer.Write("xref\n0 1\n0000000000 65535 f \ntrailer << /Size 1 >>\nstartxref\n");
            writer.Write(fs.Position.ToString());
            writer.Write("\n%%EOF");
        }

        private static (double r, double g, double b) ColorFromPID(string pid)
        {
            int hash = 17;
            foreach (var ch in pid) hash = hash * 31 + ch;
            double r = (hash & 0xFF) / 255.0;
            double g = ((hash >> 8) & 0xFF) / 255.0;
            double b = ((hash >> 16) & 0xFF) / 255.0;
            r = 0.4 + 0.6 * r;
            g = 0.4 + 0.6 * g;
            b = 0.4 + 0.6 * b;
            return (r, g, b);
        }

        private static string Escape(string s)
        {
            return s.Replace("\\", "\\\\").Replace("(", "\\(").Replace(")", "\\)");
        }
    }
}
