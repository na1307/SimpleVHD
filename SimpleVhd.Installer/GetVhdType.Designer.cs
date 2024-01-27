namespace SimpleVhd.Installer;

partial class GetVhdType {
    /// <summary> 
    /// 필수 디자이너 변수입니다.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// 사용 중인 모든 리소스를 정리합니다.
    /// </summary>
    /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
    protected override void Dispose(bool disposing) {
        if (disposing && (components != null)) {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region 구성 요소 디자이너에서 생성한 코드

    /// <summary> 
    /// 디자이너 지원에 필요한 메서드입니다. 
    /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
    /// </summary>
    private void InitializeComponent() {
        radioButton1 = new RadioButton();
        radioButton2 = new RadioButton();
        SuspendLayout();
        // 
        // radioButton1
        // 
        radioButton1.AutoSize = true;
        radioButton1.Checked = true;
        radioButton1.Location = new Point(3, 3);
        radioButton1.Name = "radioButton1";
        radioButton1.Size = new Size(117, 19);
        radioButton1.TabIndex = 0;
        radioButton1.TabStop = true;
        radioButton1.Text = "고정 크기 (Fixed)";
        radioButton1.UseVisualStyleBackColor = true;
        radioButton1.CheckedChanged += radioButton1_CheckedChanged;
        // 
        // radioButton2
        // 
        radioButton2.AutoSize = true;
        radioButton2.Location = new Point(3, 28);
        radioButton2.Name = "radioButton2";
        radioButton2.Size = new Size(150, 19);
        radioButton2.TabIndex = 1;
        radioButton2.TabStop = true;
        radioButton2.Text = "동적 확장 (Expandable)";
        radioButton2.UseVisualStyleBackColor = true;
        radioButton2.CheckedChanged += radioButton2_CheckedChanged;
        // 
        // GetVhdType
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(radioButton2);
        Controls.Add(radioButton1);
        Name = "GetVhdType";
        Size = new Size(710, 350);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private RadioButton radioButton1;
    private RadioButton radioButton2;
}
