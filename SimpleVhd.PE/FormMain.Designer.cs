namespace SimpleVhd.PE;

partial class FormMain {
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
        button1 = new Button();
        button2 = new Button();
        button3 = new Button();
        button4 = new Button();
        button5 = new Button();
        SuspendLayout();
        // 
        // button1
        // 
        button1.Location = new Point(12, 399);
        button1.Name = "button1";
        button1.Size = new Size(460, 50);
        button1.TabIndex = 0;
        button1.Text = "끝내기";
        button1.UseVisualStyleBackColor = true;
        button1.Click += button1_Click;
        // 
        // button2
        // 
        button2.Font = new Font("맑은 고딕", 12F);
        button2.Image = Properties.Resources.restore;
        button2.Location = new Point(12, 12);
        button2.Name = "button2";
        button2.Padding = new Padding(0, 0, 0, 25);
        button2.Size = new Size(227, 150);
        button2.TabIndex = 1;
        button2.Text = "VHD 자동 복원";
        button2.TextAlign = ContentAlignment.BottomCenter;
        button2.UseVisualStyleBackColor = true;
        button2.Click += button2_Click;
        // 
        // button3
        // 
        button3.Location = new Point(245, 12);
        button3.Name = "button3";
        button3.Size = new Size(227, 150);
        button3.TabIndex = 2;
        button3.Text = "button3";
        button3.UseVisualStyleBackColor = true;
        button3.Click += button3_Click;
        // 
        // button4
        // 
        button4.Location = new Point(12, 168);
        button4.Name = "button4";
        button4.Size = new Size(227, 150);
        button4.TabIndex = 3;
        button4.Text = "button4";
        button4.UseVisualStyleBackColor = true;
        button4.Click += button4_Click;
        // 
        // button5
        // 
        button5.Location = new Point(245, 168);
        button5.Name = "button5";
        button5.Size = new Size(227, 150);
        button5.TabIndex = 4;
        button5.Text = "button5";
        button5.UseVisualStyleBackColor = true;
        button5.Click += button5_Click;
        // 
        // FormMain
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(484, 461);
        ControlBox = false;
        Controls.Add(button5);
        Controls.Add(button4);
        Controls.Add(button3);
        Controls.Add(button2);
        Controls.Add(button1);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        Name = "FormMain";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "SimpleVHD PE";
        ResumeLayout(false);
    }

    #endregion

    private Button button1;
    private Button button2;
    private Button button3;
    private Button button4;
    private Button button5;
}
