using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ProcessSchedulerSim
{
    public partial class MainForm : Form
    {
        private readonly Random _rnd = new();
        private ScheduleResult? _last;
        private int _memAnim = 0;
        private bool _memUp = true;

        private List<ScheduledSlice> _timeline = new();
        private Dictionary<string, int> _executedSoFar = new();
        private int _now = 0;
        private int _sliceIndex = -1;
        private int _sliceElapsed = 0;
        private int _sliceDuration = 0;

        public MainForm()
        {
            InitializeComponent();
            // seed example processes
            AddProcessRow("P1", 0, 7);
            AddProcessRow("P2", 2, 4);
            AddProcessRow("P3", 4, 1);
        }

        private void AddProcessRow(string pid, int arrival, int burst)
        {
            grid.Rows.Add(pid, arrival, burst);
        }

        private List<ProcessItem> ReadGrid()
        {
            var list = new List<ProcessItem>();
            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.IsNewRow) continue;
                var pid = Convert.ToString(row.Cells[0].Value) ?? string.Empty;
                if (string.IsNullOrWhiteSpace(pid)) continue;
                if (!int.TryParse(Convert.ToString(row.Cells[1].Value), out int arrival)) arrival = 0;
                if (!int.TryParse(Convert.ToString(row.Cells[2].Value), out int burst)) burst = 1;
                list.Add(new ProcessItem { PID = pid.Trim(), Arrival = Math.Max(0, arrival), Burst = Math.Max(1, burst) });
            }
            return list.OrderBy(p => p.Arrival).ToList();
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            var idx = grid.Rows.Count + 1;
            AddProcessRow($"P{idx}", _rnd.Next(0, 10), _rnd.Next(1, 10));
        }

        private void BtnRandom_Click(object? sender, EventArgs e)
        {
            grid.Rows.Clear();
            int n = _rnd.Next(3, 7);
            int t = 0;
            for (int i = 1; i <= n; i++)
            {
                t += _rnd.Next(0, 3);
                AddProcessRow($"P{i}", t, _rnd.Next(1, 9));
            }
        }

        private void BtnRun_Click(object? sender, EventArgs e)
        {
            var list = ReadGrid();
            if (list.Count == 0)
            {
                MessageBox.Show("Agrega al menos un proceso.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string algo = Convert.ToString(cboAlgo.SelectedItem) ?? "FCFS";
            ScheduleResult result;
            if (algo.StartsWith("FCFS")) result = Scheduler.RunFCFS(list);
            else if (algo.StartsWith("SJF")) result = Scheduler.RunSJF(list);
            else result = Scheduler.RunRR(list, (int)nudQuantum.Value);

            _last = result;
            _timeline = result.Timeline.ToList();

            gantt.Data.Clear();
            gantt.Data.AddRange(_timeline);
            gantt.Invalidate();

            lblAvgW.Text = $"Avg Waiting: {result.AvgWaiting:F2}";
            lblAvgT.Text = $"Avg Turnaround: {result.AvgTurnaround:F2}";
            lblThroughput.Text = $"Throughput: {result.Throughput:F4}";
            lblMakespan.Text = $"Makespan: {result.Makespan}";

            PrepareLiveTable(list);
            _executedSoFar = list.ToDictionary(p => p.PID, p => 0);
            _now = 0;
            _sliceIndex = -1;
            _sliceElapsed = 0;
            _sliceDuration = 0;
            lblNow.Text = "Tiempo actual: 0";
            lblSlice.Text = "Tramo actual: -";
            sliceBar.Value = 0;
            simTimer.Start();
        }

        private void PrepareLiveTable(List<ProcessItem> source)
        {
            liveGrid.Rows.Clear();
            foreach (var p in source)
            {
                liveGrid.Rows.Add(p.PID, "Nuevo", p.Burst, "-", "-");
            }
        }

        private void BtnClear_Click(object? sender, EventArgs e)
        {
            grid.Rows.Clear();
            gantt.Data.Clear();
            gantt.Invalidate();
            lblAvgW.Text = "Avg Waiting: -";
            lblAvgT.Text = "Avg Turnaround: -";
            lblThroughput.Text = "Throughput: -";
            lblMakespan.Text = "Makespan: -";
            _last = null;

            liveGrid.Rows.Clear();
            simTimer.Stop();
            _timeline.Clear();
            _executedSoFar.Clear();
            _now = 0;
            _sliceIndex = -1;
            _sliceElapsed = 0;
            _sliceDuration = 0;
            lblNow.Text = "Tiempo actual: 0";
            lblSlice.Text = "Tramo actual: -";
            sliceBar.Value = 0;
        }

        private void BtnExportCsv_Click(object? sender, EventArgs e)
        {
            if (_last == null || _last.Completed.Count == 0)
            {
                MessageBox.Show("Ejecuta una simulación antes de exportar.");
                return;
            }
            using var sfd = new SaveFileDialog { Filter = "CSV (*.csv)|*.csv", FileName = "reporte_planificacion.csv" };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ReportExporter.ExportCsv(sfd.FileName, _last.Completed, _last);
                    MessageBox.Show("CSV generado.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error exportando CSV: " + ex.Message);
                }
            }
        }

        private void BtnExportPdf_Click(object? sender, EventArgs e)
        {
            if (_last == null || _last.Completed.Count == 0)
            {
                MessageBox.Show("Ejecuta una simulación antes de exportar.");
                return;
            }
            using var sfd = new SaveFileDialog { Filter = "PDF (*.pdf)|*.pdf", FileName = "reporte_planificacion.pdf" };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ReportExporter.ExportPdf(sfd.FileName, _last.Completed, _last, _timeline);
                    MessageBox.Show("PDF generado.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error exportando PDF: " + ex.Message);
                }
            }
        }

        private void SimTimer_Tick(object? sender, EventArgs e)
        {
            if (_memUp) { _memAnim += 7; if (_memAnim >= 100) { _memAnim = 100; _memUp = false; } }
            else { _memAnim -= 9; if (_memAnim <= 0) { _memAnim = 0; _memUp = true; } }
            memBar.Value = Math.Max(memBar.Minimum, Math.Min(memBar.Maximum, _memAnim));

            if (_timeline.Count == 0) { simTimer.Stop(); return; }

            if (_sliceIndex < 0 || _sliceElapsed >= _sliceDuration)
            {
                _sliceIndex++;
                if (_sliceIndex >= _timeline.Count) { simTimer.Stop(); lblSlice.Text = "Tramo actual: (fin)"; return; }
                var s = _timeline[_sliceIndex];
                _now = Math.Max(_now, s.Start);
                _sliceElapsed = 0;
                _sliceDuration = s.End - s.Start;

                int waitBefore = 0;
                if (_sliceIndex > 0)
                {
                    var prevEnd = _timeline[_sliceIndex - 1].End;
                    waitBefore = Math.Max(0, s.Start - prevEnd);
                }

                lblSlice.Text = $"Tramo actual: {s.PID} [{s.Start}-{s.End}] (duración {s.End - s.Start}s, espera previa {waitBefore}s)";
                sliceBar.Value = 0;
                UpdateLiveStatesOnSliceStart(s);
            }
            else
            {
                _sliceElapsed++;
                sliceBar.Value = Math.Min(sliceBar.Maximum, (int)((_sliceElapsed * 100.0) / Math.Max(1, _sliceDuration)));
                _now++;
                lblNow.Text = $"Tiempo actual: {_now}";
            }

            UpdateRemainingForCurrent();
        }

        private void UpdateLiveStatesOnSliceStart(ScheduledSlice s)
        {
            foreach (DataGridViewRow row in liveGrid.Rows)
            {
                var pid = Convert.ToString(row.Cells[0].Value) ?? "";
                if (string.IsNullOrEmpty(pid)) continue;
                if (pid == s.PID)
                {
                    row.Cells[1].Value = "Ejecución";
                    if (row.Cells[3].Value?.ToString() == "-") row.Cells[3].Value = s.Start;
                }
                else
                {
                    if ((row.Cells[1].Value?.ToString() ?? "") != "Terminado")
                        row.Cells[1].Value = "Listo";
                }
            }
        }

        private void UpdateRemainingForCurrent()
        {
            if (_sliceIndex < 0 || _sliceIndex >= _timeline.Count) return;
            var s = _timeline[_sliceIndex];

            _executedSoFar.TryGetValue(s.PID, out int prev);
            int exec = prev + 1;
            if (_sliceElapsed == 0) exec = prev;
            _executedSoFar[s.PID] = Math.Min(exec, GetBurstOfPid(s.PID));

            foreach (DataGridViewRow row in liveGrid.Rows)
            {
                var pid = Convert.ToString(row.Cells[0].Value) ?? "";
                if (string.IsNullOrEmpty(pid)) continue;
                int burst = GetBurstOfPid(pid);
                _executedSoFar.TryGetValue(pid, out int ex);
                int remaining = Math.Max(0, burst - ex);
                row.Cells[2].Value = remaining;

                if (remaining == 0 && (row.Cells[1].Value?.ToString() ?? "") != "Terminado")
                {
                    row.Cells[1].Value = "Terminado";
                    int finish = _timeline.Where(t => t.PID == pid).Max(t => t.End);
                    row.Cells[4].Value = finish;
                }
            }
        }

        private int GetBurstOfPid(string pid)
        {
            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.IsNewRow) continue;
                var rpid = Convert.ToString(row.Cells[0].Value) ?? "";
                if (rpid == pid)
                {
                    int.TryParse(Convert.ToString(row.Cells[2].Value), out int burst);
                    return Math.Max(1, burst);
                }
            }
            return 1;
        }
    }
}