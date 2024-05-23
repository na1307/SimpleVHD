namespace SimpleVhd.PE;

partial class ChooseInstanceDialog {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
        if (disposing && (components != null)) {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
        dataGridView1 = new DataGridView();
        nameColumn = new DataGridViewTextBoxColumn();
        directoryColumn = new DataGridViewTextBoxColumn();
        fileNameColumn = new DataGridViewTextBoxColumn();
        ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
        SuspendLayout();
        // 
        // dataGridView1
        // 
        dataGridView1.AllowUserToAddRows = false;
        dataGridView1.AllowUserToDeleteRows = false;
        dataGridView1.AllowUserToResizeRows = false;
        dataGridView1.BackgroundColor = SystemColors.Control;
        dataGridView1.BorderStyle = BorderStyle.None;
        dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dataGridView1.Columns.AddRange(new DataGridViewColumn[] { nameColumn, directoryColumn, fileNameColumn });
        dataGridView1.Location = new Point(12, 12);
        dataGridView1.MultiSelect = false;
        dataGridView1.Name = "dataGridView1";
        dataGridView1.ReadOnly = true;
        dataGridView1.RowHeadersVisible = false;
        dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dataGridView1.Size = new Size(560, 212);
        dataGridView1.TabIndex = 1;
        // 
        // nameColumn
        // 
        nameColumn.DataPropertyName = "Name";
        nameColumn.HeaderText = "이름";
        nameColumn.Name = "nameColumn";
        nameColumn.ReadOnly = true;
        // 
        // directoryColumn
        // 
        directoryColumn.DataPropertyName = "Directory";
        directoryColumn.HeaderText = "디렉토리 경로";
        directoryColumn.Name = "directoryColumn";
        directoryColumn.ReadOnly = true;
        directoryColumn.Width = 300;
        // 
        // fileNameColumn
        // 
        fileNameColumn.DataPropertyName = "FileName";
        fileNameColumn.HeaderText = "파일 이름";
        fileNameColumn.Name = "fileNameColumn";
        fileNameColumn.ReadOnly = true;
        fileNameColumn.Width = 130;
        // 
        // ChooseInstanceDialog
        // 
        AutoScaleMode = AutoScaleMode.None;
        ClientSize = new Size(584, 261);
        Controls.Add(dataGridView1);
        Name = "ChooseInstanceDialog";
        Text = "인스턴스 선택";
        Controls.SetChildIndex(dataGridView1, 0);
        ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private DataGridView dataGridView1;
    private DataGridViewTextBoxColumn nameColumn;
    private DataGridViewTextBoxColumn directoryColumn;
    private DataGridViewTextBoxColumn fileNameColumn;
}