namespace ProjectV.Processor;

partial class FormAdvanced : FormDialog {
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

    #region Windows Form 디자이너에서 생성한 코드

    /// <summary>
    /// 디자이너 지원에 필요한 메서드입니다. 
    /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
    /// </summary>
    private void InitializeComponent() {
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(397, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 25);
            this.button1.TabIndex = 1;
            this.button1.Text = "시작";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(379, 100);
            this.label1.TabIndex = 2;
            this.label1.Text = "VHDX 포맷에서 VHD_BOOT_INITIALIZATION_FAILED 블루스크린이 발생하는 경우\r\n\r\n이 경우는 부모 VHD와 자식 VHD의 " +
    "정보 불일치가 원인일 수 있습니다. VHD 재구축 작업을 실행하면 해결할 수 있습니다. VHD 포맷에서는 초기화만으로도 해결할 수 있습니다.";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(397, 112);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 25);
            this.button2.TabIndex = 1;
            this.button2.Text = "시작";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 109);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(379, 100);
            this.label2.TabIndex = 2;
            this.label2.Text = "차등 스타일에서 고정 크기 형식으로 변환한 후 부팅이 불가능한 경우\r\n\r\n이 경우는 용량 부족이 원인일 수 있습니다. VHD를 동적 확장 형식으로" +
    " 변환하는 응급 복원 작업을 실행하여 해결할 수 있습니다.";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(397, 212);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 25);
            this.button3.TabIndex = 1;
            this.button3.Text = "시작";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 209);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(379, 100);
            this.label3.TabIndex = 2;
            this.label3.Text = "차등 스타일로 전환한 후 부팅이 불가능한 경우\r\n\r\n이 경우는 용량 부족이 원인일 수 있습니다. 단순 스타일로 전환하여 문제를 해결할 수 있습니다" +
    ".";
            // 
            // FormAdvanced
            // 
            this.ClientSize = new System.Drawing.Size(484, 361);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "FormAdvanced";
            this.Text = "문제 해결사";
            this.Controls.SetChildIndex(this.button1, 0);
            this.Controls.SetChildIndex(this.button2, 0);
            this.Controls.SetChildIndex(this.button3, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.ResumeLayout(false);

    }

    #endregion

    private Button button1;
    private Label label1;
    private Button button2;
    private Label label2;
    private Button button3;
    private Label label3;
}