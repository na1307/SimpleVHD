﻿namespace SimpleVhd.Installer;

partial class FormCheckRequirements {
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
        SuspendLayout();
        // 
        // FormCheckRequirements
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(284, 111);
        LabelText = "요구 사항을 확인하는 중입니다.\r\n\r\n잠시만 기다려 주세요...";
        Name = "FormCheckRequirements";
        Text = "요구 사항 확인 중";
        ResumeLayout(false);
    }

    #endregion
}