namespace SimpleVhd.PE;

partial class Working {
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
        label1 = new Label();
        SuspendLayout();
        // 
        // label1
        // 
        label1.Dock = DockStyle.Fill;
        label1.Location = new Point(0, 0);
        label1.Name = "label1";
        label1.Size = new Size(500, 100);
        label1.TabIndex = 0;
        label1.Text = " 작업을 진행 중입니다... 잠시만 기다려 주세요.";
        label1.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // Working
        // 
        AutoScaleMode = AutoScaleMode.None;
        ClientSize = new Size(500, 100);
        ControlBox = false;
        Controls.Add(label1);
        FormBorderStyle = FormBorderStyle.None;
        Name = "Working";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Working";
        TopMost = true;
        ResumeLayout(false);
    }

    #endregion

    private Label label1;
}