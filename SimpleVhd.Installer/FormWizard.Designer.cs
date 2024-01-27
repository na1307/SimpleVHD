namespace SimpleVhd.Installer;

partial class FormWizard {
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
        buttonNext = new Button();
        buttonOK = new Button();
        buttonCancel = new Button();
        label1 = new Label();
        label2 = new Label();
        panel1 = new Panel();
        buttonPrevious = new Button();
        SuspendLayout();
        // 
        // buttonNext
        // 
        buttonNext.Location = new Point(566, 426);
        buttonNext.Name = "buttonNext";
        buttonNext.Size = new Size(75, 23);
        buttonNext.TabIndex = 0;
        buttonNext.Text = "다음";
        buttonNext.UseVisualStyleBackColor = true;
        buttonNext.Click += buttonNext_Click;
        // 
        // buttonOK
        // 
        buttonOK.DialogResult = DialogResult.OK;
        buttonOK.Location = new Point(566, 426);
        buttonOK.Name = "buttonOK";
        buttonOK.Size = new Size(75, 23);
        buttonOK.TabIndex = 1;
        buttonOK.Text = "확인";
        buttonOK.UseVisualStyleBackColor = true;
        buttonOK.Visible = false;
        // 
        // buttonCancel
        // 
        buttonCancel.DialogResult = DialogResult.Cancel;
        buttonCancel.Location = new Point(647, 426);
        buttonCancel.Name = "buttonCancel";
        buttonCancel.Size = new Size(75, 23);
        buttonCancel.TabIndex = 2;
        buttonCancel.Text = "취소";
        buttonCancel.UseVisualStyleBackColor = true;
        // 
        // label1
        // 
        label1.Font = new Font("맑은 고딕", 12F, FontStyle.Bold);
        label1.Location = new Point(12, 9);
        label1.Name = "label1";
        label1.Size = new Size(710, 25);
        label1.TabIndex = 3;
        label1.Text = "제목";
        label1.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // label2
        // 
        label2.Location = new Point(12, 34);
        label2.Name = "label2";
        label2.Size = new Size(710, 50);
        label2.TabIndex = 4;
        label2.Text = "설명";
        // 
        // panel1
        // 
        panel1.Location = new Point(12, 87);
        panel1.Name = "panel1";
        panel1.Size = new Size(710, 333);
        panel1.TabIndex = 5;
        panel1.ControlAdded += panel1_ControlAdded;
        // 
        // buttonPrevious
        // 
        buttonPrevious.Location = new Point(485, 426);
        buttonPrevious.Name = "buttonPrevious";
        buttonPrevious.Size = new Size(75, 23);
        buttonPrevious.TabIndex = 6;
        buttonPrevious.Text = "이전";
        buttonPrevious.UseVisualStyleBackColor = true;
        buttonPrevious.Visible = false;
        buttonPrevious.Click += buttonPrevious_Click;
        // 
        // FormWizard
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(734, 461);
        Controls.Add(buttonPrevious);
        Controls.Add(panel1);
        Controls.Add(label2);
        Controls.Add(label1);
        Controls.Add(buttonCancel);
        Controls.Add(buttonOK);
        Controls.Add(buttonNext);
        HelpButton = true;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FormWizard";
        StartPosition = FormStartPosition.CenterParent;
        Text = "FormWizard";
        ResumeLayout(false);
    }

    #endregion

    private Button buttonNext;
    private Button buttonOK;
    private Button buttonCancel;
    private Label label1;
    private Label label2;
    private Panel panel1;
    private Button buttonPrevious;
}