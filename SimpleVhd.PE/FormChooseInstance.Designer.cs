namespace SimpleVhd.PE;

partial class FormChooseInstance {
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
        var dataGridViewCellStyle1 = new DataGridViewCellStyle();
        var dataGridViewCellStyle2 = new DataGridViewCellStyle();
        var dataGridViewCellStyle3 = new DataGridViewCellStyle();
        dataGridView1 = new DataGridView();
        ColumnDirectory = new DataGridViewTextBoxColumn();
        ColumnParentFile = new DataGridViewTextBoxColumn();
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
        dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle1.BackColor = SystemColors.Control;
        dataGridViewCellStyle1.Font = new Font("맑은 고딕", 9F);
        dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
        dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
        dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
        dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dataGridView1.Columns.AddRange(new DataGridViewColumn[] { ColumnDirectory, ColumnParentFile });
        dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle2.BackColor = SystemColors.Window;
        dataGridViewCellStyle2.Font = new Font("맑은 고딕", 9F);
        dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
        dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
        dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
        dataGridView1.Location = new Point(12, 12);
        dataGridView1.MultiSelect = false;
        dataGridView1.Name = "dataGridView1";
        dataGridView1.ReadOnly = true;
        dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle3.BackColor = SystemColors.Control;
        dataGridViewCellStyle3.Font = new Font("맑은 고딕", 9F);
        dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
        dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
        dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
        dataGridView1.RowHeadersVisible = false;
        dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dataGridView1.Size = new Size(560, 212);
        dataGridView1.TabIndex = 1;
        // 
        // ColumnDirectory
        // 
        ColumnDirectory.DataPropertyName = "Directory";
        ColumnDirectory.HeaderText = "디렉토리 경로";
        ColumnDirectory.Name = "ColumnDirectory";
        ColumnDirectory.ReadOnly = true;
        ColumnDirectory.Width = 350;
        // 
        // ColumnParentFile
        // 
        ColumnParentFile.DataPropertyName = "ParentFile";
        ColumnParentFile.HeaderText = "파일 이름";
        ColumnParentFile.Name = "ColumnParentFile";
        ColumnParentFile.ReadOnly = true;
        ColumnParentFile.Width = 180;
        // 
        // FormChooseInstance
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(584, 261);
        Controls.Add(dataGridView1);
        Name = "FormChooseInstance";
        Text = "인스턴스 선택";
        Controls.SetChildIndex(dataGridView1, 0);
        ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private DataGridView dataGridView1;
    private DataGridViewTextBoxColumn ColumnDirectory;
    private DataGridViewTextBoxColumn ColumnParentFile;
}