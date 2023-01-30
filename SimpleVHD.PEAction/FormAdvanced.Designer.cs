namespace SimpleVHD.PEAction;

partial class FormAdvanced : Dialog {
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
            this.advancedEntry1 = new SimpleVHD.PEAction.AdvancedEntry();
            this.advancedEntry2 = new SimpleVHD.PEAction.AdvancedEntry();
            this.advancedEntry3 = new SimpleVHD.PEAction.AdvancedEntry();
            this.SuspendLayout();
            // 
            // advancedEntry1
            // 
            this.advancedEntry1.Location = new System.Drawing.Point(12, 12);
            this.advancedEntry1.Name = "advancedEntry1";
            this.advancedEntry1.Size = new System.Drawing.Size(460, 90);
            this.advancedEntry1.TabIndex = 1;
            this.advancedEntry1.Text = "이 경우는 부모 VHD와 자식 VHD의 정보 불일치가 원인일 수 있습니다. VHD 재구축 작업을 실행하면 해결할 수 있습니다. VHD 포맷에서는 " +
    "초기화만으로도 해결할 수 있습니다.";
            this.advancedEntry1.Title = "VHDX 포맷에서 VHD_BOOT_INITIALIZATION_FAILED 블루스크린이 발생하는 경우";
            this.advancedEntry1.Start += new System.EventHandler(this.advancedEntry1_Start);
            // 
            // advancedEntry2
            // 
            this.advancedEntry2.Location = new System.Drawing.Point(12, 108);
            this.advancedEntry2.Name = "advancedEntry2";
            this.advancedEntry2.Size = new System.Drawing.Size(460, 90);
            this.advancedEntry2.TabIndex = 2;
            this.advancedEntry2.Text = "이 경우는 용량 부족이 원인일 수 있습니다. VHD를 동적 확장 형식으로 변환하는 응급 복원 작업을 실행하여 해결할 수 있습니다.";
            this.advancedEntry2.Title = "차등 스타일에서 고정 크기 형식으로 변환한 후 부팅이 불가능한 경우";
            this.advancedEntry2.Start += new System.EventHandler(this.advancedEntry2_Start);
            // 
            // advancedEntry3
            // 
            this.advancedEntry3.Location = new System.Drawing.Point(12, 204);
            this.advancedEntry3.Name = "advancedEntry3";
            this.advancedEntry3.Size = new System.Drawing.Size(460, 90);
            this.advancedEntry3.TabIndex = 3;
            this.advancedEntry3.Text = "이 경우는 용량 부족이 원인일 수 있습니다. 단순 스타일로 전환하여 문제를 해결할 수 있습니다.";
            this.advancedEntry3.Title = "차등 스타일로 전환한 후 부팅이 불가능한 경우";
            this.advancedEntry3.Start += new System.EventHandler(this.advancedEntry3_Start);
            // 
            // FormAdvanced
            // 
            this.ClientSize = new System.Drawing.Size(484, 361);
            this.Controls.Add(this.advancedEntry3);
            this.Controls.Add(this.advancedEntry2);
            this.Controls.Add(this.advancedEntry1);
            this.Name = "FormAdvanced";
            this.Text = "문제 해결사";
            this.Controls.SetChildIndex(this.advancedEntry1, 0);
            this.Controls.SetChildIndex(this.advancedEntry2, 0);
            this.Controls.SetChildIndex(this.advancedEntry3, 0);
            this.ResumeLayout(false);

    }

    #endregion
    private AdvancedEntry advancedEntry1;
    private AdvancedEntry advancedEntry2;
    private AdvancedEntry advancedEntry3;
}