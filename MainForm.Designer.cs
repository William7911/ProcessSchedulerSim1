using System.Windows.Forms;
using System.Drawing;

namespace ProcessSchedulerSim
{

    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null!;
        private DataGridView grid;
        private Button btnAdd, btnRandom, btnRun, btnClear, btnExportCsv, btnExportPdf;
        private ComboBox cboAlgo;
        private NumericUpDown nudQuantum;
        private Label lblAlgo, lblQuantum, lblAvgW, lblAvgT, lblThroughput, lblMakespan, lblCpuMem;
        private GanttControl gantt;
        private System.Windows.Forms.Timer simTimer;
        private ProgressBar memBar;

        private Label lblNow, lblSlice;
        private ProgressBar sliceBar;
        private DataGridView liveGrid;

        private FlowLayoutPanel pnlTop;
        private SplitContainer mainSplit;
        private SplitContainer bottomSplit;
        private FlowLayoutPanel pnlBottom;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            pnlTop = new FlowLayoutPanel();
            btnAdd = new Button();
            btnRandom = new Button();
            btnRun = new Button();
            btnClear = new Button();
            btnExportCsv = new Button();
            btnExportPdf = new Button();
            lblAlgo = new Label();
            cboAlgo = new ComboBox();
            lblQuantum = new Label();
            nudQuantum = new NumericUpDown();
            mainSplit = new SplitContainer();
            grid = new DataGridView();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            bottomSplit = new SplitContainer();
            gantt = new GanttControl();
            pnlBottom = new FlowLayoutPanel();
            lblCpuMem = new Label();
            lblAvgT = new Label();
            lblThroughput = new Label();
            lblAvgW = new Label();
            lblMakespan = new Label();
            memBar = new ProgressBar();
            lblNow = new Label();
            lblSlice = new Label();
            sliceBar = new ProgressBar();
            liveGrid = new DataGridView();
            dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn7 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn8 = new DataGridViewTextBoxColumn();
            simTimer = new System.Windows.Forms.Timer(components);
            pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudQuantum).BeginInit();
            ((System.ComponentModel.ISupportInitialize)mainSplit).BeginInit();
            mainSplit.Panel1.SuspendLayout();
            mainSplit.Panel2.SuspendLayout();
            mainSplit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
            ((System.ComponentModel.ISupportInitialize)bottomSplit).BeginInit();
            bottomSplit.Panel1.SuspendLayout();
            bottomSplit.SuspendLayout();
            pnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)liveGrid).BeginInit();
            SuspendLayout();
            // 
            // pnlTop
            // 
            pnlTop.AutoScroll = true;
            pnlTop.AutoSize = true;
            pnlTop.BackColor = Color.DarkCyan;
            pnlTop.Controls.Add(btnAdd);
            pnlTop.Controls.Add(btnRandom);
            pnlTop.Controls.Add(btnRun);
            pnlTop.Controls.Add(btnClear);
            pnlTop.Controls.Add(btnExportCsv);
            pnlTop.Controls.Add(btnExportPdf);
            pnlTop.Controls.Add(lblAlgo);
            pnlTop.Controls.Add(cboAlgo);
            pnlTop.Controls.Add(lblQuantum);
            pnlTop.Controls.Add(nudQuantum);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Location = new Point(0, 0);
            pnlTop.Name = "pnlTop";
            pnlTop.Padding = new Padding(8);
            pnlTop.Size = new Size(1902, 50);
            pnlTop.TabIndex = 2;
            pnlTop.WrapContents = false;
            // 
            // Añade botones para implemenatar mas procesos
            // 
            btnAdd.BackColor = SystemColors.ButtonHighlight;
            btnAdd.Cursor = Cursors.Hand;
            btnAdd.Location = new Point(11, 11);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(86, 28);
            btnAdd.TabIndex = 0;
            btnAdd.Text = "Proceso+";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += BtnAdd_Click;
            // 
            // Configuracion del boton random (aleatorios) 
            // 
            btnRandom.BackColor = SystemColors.ButtonHighlight;
            btnRandom.Cursor = Cursors.Hand;
            btnRandom.Location = new Point(103, 11);
            btnRandom.Name = "btnRandom";
            btnRandom.Size = new Size(104, 28);
            btnRandom.TabIndex = 1;
            btnRandom.Text = "Aleatorios";
            btnRandom.UseVisualStyleBackColor = false;
            btnRandom.Click += BtnRandom_Click;
            // 
            // Ejecuta el proyecto con los procesos que se hayan añadido
            // 
            btnRun.BackColor = SystemColors.ButtonHighlight;
            btnRun.Cursor = Cursors.Hand;
            btnRun.Location = new Point(213, 11);
            btnRun.Name = "btnRun";
            btnRun.Size = new Size(75, 28);
            btnRun.TabIndex = 2;
            btnRun.Text = "Iniciar";
            btnRun.UseVisualStyleBackColor = false;
            btnRun.Click += BtnRun_Click;
            // 
            // limpia los procesos añadidos
            // 
            btnClear.BackColor = SystemColors.ButtonHighlight;
            btnClear.Cursor = Cursors.Hand;
            btnClear.Location = new Point(294, 11);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(75, 27);
            btnClear.TabIndex = 3;
            btnClear.Text = "Limpiar";
            btnClear.UseVisualStyleBackColor = false;
            btnClear.Click += BtnClear_Click;
            // 
            // exporta los procesos a un archivo CVS
            // 
            btnExportCsv.BackColor = SystemColors.ButtonHighlight;
            btnExportCsv.Cursor = Cursors.Hand;
            btnExportCsv.Location = new Point(375, 11);
            btnExportCsv.Name = "btnExportCsv";
            btnExportCsv.Size = new Size(114, 28);
            btnExportCsv.TabIndex = 4;
            btnExportCsv.Text = "Exportar CVS";
            btnExportCsv.UseVisualStyleBackColor = false;
            btnExportCsv.Click += BtnExportCsv_Click;
            // 
            // exporta los procesos a un archivo PDF
            // 
            btnExportPdf.BackColor = SystemColors.ButtonHighlight;
            btnExportPdf.Cursor = Cursors.Hand;
            btnExportPdf.Location = new Point(495, 11);
            btnExportPdf.Name = "btnExportPdf";
            btnExportPdf.Size = new Size(118, 28);
            btnExportPdf.TabIndex = 5;
            btnExportPdf.Text = "Exportar PDF";
            btnExportPdf.UseVisualStyleBackColor = false;
            btnExportPdf.Click += BtnExportPdf_Click;
            // 
            // realiza la configuracion del label de algoritmo
            // 
            lblAlgo.Anchor = AnchorStyles.None;
            lblAlgo.BackColor = Color.DarkCyan;
            lblAlgo.Location = new Point(619, 13);
            lblAlgo.Name = "lblAlgo";
            lblAlgo.Size = new Size(100, 23);
            lblAlgo.TabIndex = 6;
            // 
            // Muestra los algoritmos disponibles
            // 
            cboAlgo.Items.AddRange(new object[] { "FCFS", "SJF (No apropiativo)", "Round Robin" });
            cboAlgo.Location = new Point(725, 11);
            cboAlgo.Name = "cboAlgo";
            cboAlgo.Size = new Size(121, 28);
            cboAlgo.TabIndex = 7;
            // 
            // Cuantos procesos se ejecutan en el quantum
            // 
            lblQuantum.Anchor = AnchorStyles.None;
            lblQuantum.BackColor = Color.White;
            lblQuantum.Location = new Point(852, 13);
            lblQuantum.Name = "lblQuantum";
            lblQuantum.Size = new Size(100, 23);
            lblQuantum.TabIndex = 8;
            lblQuantum.Text = "Quantum";
            lblQuantum.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cuanto tiempo dura el quantum
            // 
            nudQuantum.Location = new Point(958, 11);
            nudQuantum.Name = "nudQuantum";
            nudQuantum.Size = new Size(120, 27);
            nudQuantum.TabIndex = 9;
            // 
            // mainSplit
            // 
            mainSplit.Dock = DockStyle.Fill;
            mainSplit.Location = new Point(0, 50);
            mainSplit.Name = "mainSplit";
            mainSplit.Orientation = Orientation.Horizontal;
            // 
            // mainSplit.Panel1
            // 
            mainSplit.Panel1.Controls.Add(grid);
            // 
            // mainSplit.Panel2
            // 
            mainSplit.Panel2.Controls.Add(bottomSplit);
            mainSplit.Size = new Size(1902, 806);
            mainSplit.SplitterDistance = 568;
            mainSplit.TabIndex = 0;
            // 
            // grid
            // 
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.ColumnHeadersHeight = 29;
            grid.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2, dataGridViewTextBoxColumn3 });
            grid.Location = new Point(0, 0);
            grid.Name = "grid";
            grid.RowHeadersWidth = 51;
            grid.Size = new Size(1902, 460);
            grid.TabIndex = 0;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.HeaderText = "PID";
            dataGridViewTextBoxColumn1.MinimumWidth = 6;
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.HeaderText = "Arrival";
            dataGridViewTextBoxColumn2.MinimumWidth = 6;
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewTextBoxColumn3.HeaderText = "Burst";
            dataGridViewTextBoxColumn3.MinimumWidth = 6;
            dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // bottomSplit
            // 
            bottomSplit.Dock = DockStyle.Fill;
            bottomSplit.Location = new Point(0, 0);
            bottomSplit.Name = "bottomSplit";
            // 
            // bottomSplit.Panel1
            // 
            bottomSplit.Panel1.Controls.Add(gantt);
            bottomSplit.Panel1.Controls.Add(pnlBottom);
            bottomSplit.Size = new Size(1902, 234);
            bottomSplit.SplitterDistance = 1531;
            bottomSplit.TabIndex = 0;
            // 
            // gantt
            // 
            gantt.BackColor = Color.White;
            gantt.ForeColor = Color.Black;
            gantt.Location = new Point(472, 0);
            gantt.Name = "gantt";
            gantt.RowHeight = 34;
            gantt.Size = new Size(714, 224);
            gantt.TabIndex = 0;
            gantt.TimeScale = 40;
            // 
            // pnlBottom
            // 
            pnlBottom.AutoScroll = true;
            pnlBottom.Controls.Add(lblCpuMem);
            pnlBottom.Controls.Add(lblAvgT);
            pnlBottom.Controls.Add(lblThroughput);
            pnlBottom.Controls.Add(lblAvgW);
            pnlBottom.Controls.Add(lblMakespan);
            pnlBottom.Controls.Add(memBar);
            pnlBottom.Controls.Add(lblNow);
            pnlBottom.Controls.Add(lblSlice);
            pnlBottom.Controls.Add(sliceBar);
            pnlBottom.FlowDirection = FlowDirection.TopDown;
            pnlBottom.Location = new Point(0, 0);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Padding = new Padding(8);
            pnlBottom.Size = new Size(466, 237);
            pnlBottom.TabIndex = 0;
            pnlBottom.WrapContents = false;
            // 
            // lblCpuMem
            // 
            lblCpuMem.Location = new Point(11, 8);
            lblCpuMem.Name = "lblCpuMem";
            lblCpuMem.Size = new Size(100, 23);
            lblCpuMem.TabIndex = 4;
            // 
            // lblAvgT
            // 
            lblAvgT.Location = new Point(11, 31);
            lblAvgT.Name = "lblAvgT";
            lblAvgT.Size = new Size(100, 23);
            lblAvgT.TabIndex = 1;
            // 
            // lblThroughput
            // 
            lblThroughput.Location = new Point(11, 54);
            lblThroughput.Name = "lblThroughput";
            lblThroughput.Size = new Size(100, 23);
            lblThroughput.TabIndex = 2;
            // 
            // lblAvgW
            // 
            lblAvgW.Location = new Point(11, 77);
            lblAvgW.Name = "lblAvgW";
            lblAvgW.Size = new Size(100, 23);
            lblAvgW.TabIndex = 0;
            // 
            // lblMakespan
            // 
            lblMakespan.Location = new Point(11, 100);
            lblMakespan.Name = "lblMakespan";
            lblMakespan.Size = new Size(100, 23);
            lblMakespan.TabIndex = 3;
            // 
            // memBar
            // 
            memBar.Location = new Point(11, 126);
            memBar.Name = "memBar";
            memBar.Size = new Size(100, 23);
            memBar.TabIndex = 5;
            // 
            // lblNow
            // 
            lblNow.Location = new Point(11, 152);
            lblNow.Name = "lblNow";
            lblNow.Size = new Size(100, 23);
            lblNow.TabIndex = 6;
            // 
            // lblSlice
            // 
            lblSlice.Location = new Point(11, 175);
            lblSlice.Name = "lblSlice";
            lblSlice.Size = new Size(100, 23);
            lblSlice.TabIndex = 7;
            // 
            // sliceBar
            // 
            sliceBar.Location = new Point(11, 201);
            sliceBar.Name = "sliceBar";
            sliceBar.Size = new Size(100, 23);
            sliceBar.TabIndex = 8;
            // 
            // liveGrid
            // 
            liveGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            liveGrid.ColumnHeadersHeight = 29;
            liveGrid.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn4, dataGridViewTextBoxColumn5, dataGridViewTextBoxColumn6, dataGridViewTextBoxColumn7, dataGridViewTextBoxColumn8 });
            liveGrid.Dock = DockStyle.Bottom;
            liveGrid.Location = new Point(0, 856);
            liveGrid.Name = "liveGrid";
            liveGrid.ReadOnly = true;
            liveGrid.RowHeadersWidth = 51;
            liveGrid.Size = new Size(1902, 177);
            liveGrid.TabIndex = 1;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewTextBoxColumn4.HeaderText = "PID";
            dataGridViewTextBoxColumn4.MinimumWidth = 6;
            dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewTextBoxColumn5.HeaderText = "Estado";
            dataGridViewTextBoxColumn5.MinimumWidth = 6;
            dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            dataGridViewTextBoxColumn6.HeaderText = "Restante";
            dataGridViewTextBoxColumn6.MinimumWidth = 6;
            dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn7
            // 
            dataGridViewTextBoxColumn7.HeaderText = "Inicio";
            dataGridViewTextBoxColumn7.MinimumWidth = 6;
            dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            dataGridViewTextBoxColumn7.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn8
            // 
            dataGridViewTextBoxColumn8.HeaderText = "Fin";
            dataGridViewTextBoxColumn8.MinimumWidth = 6;
            dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            dataGridViewTextBoxColumn8.ReadOnly = true;
            // 
            // simTimer
            // 
            simTimer.Tick += SimTimer_Tick;
            // 
            // MainForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.DarkCyan;
            ClientSize = new Size(1902, 1033);
            Controls.Add(mainSplit);
            Controls.Add(liveGrid);
            Controls.Add(pnlTop);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Simulador de Gestión de Procesos — SO (v2)";
            pnlTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)nudQuantum).EndInit();
            mainSplit.Panel1.ResumeLayout(false);
            mainSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)mainSplit).EndInit();
            mainSplit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)grid).EndInit();
            bottomSplit.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)bottomSplit).EndInit();
            bottomSplit.ResumeLayout(false);
            pnlBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)liveGrid).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
    }
}
