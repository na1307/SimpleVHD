﻿using System.ComponentModel;

namespace SimpleVhd.Installer;

public partial class FormWizard : Form {
    private LinkedListNode<ISetupWizardPage> currentPage;

    public FormWizard(IEnumerable<ISetupWizardPage> pages) {
        InitializeComponent();
        currentPage = new LinkedList<ISetupWizardPage>(pages).First ?? throw new ArgumentException("pages should not be empty.", nameof(pages));
        panel1.Controls.Add(currentPage.Value.Panel);
        label1.Text = currentPage.Value.Title;
        label2.Text = currentPage.Value.Description;

        if (currentPage.Next == null) {
            buttonNext.Visible = false;
            buttonOK.Visible = true;
        }
    }

    protected override void OnHelpButtonClicked(CancelEventArgs e) {
        e.Cancel = true;
    }

    private void buttonNext_Click(object sender, EventArgs e) {
        panel1.Controls.Remove(currentPage.Value.Panel);

        if (currentPage.Next != null) {
            currentPage = currentPage.Next;
            panel1.Controls.Add(currentPage.Value.Panel);
        } else {
            throw new InvalidOperationException();
        }
    }

    private void buttonPrevious_Click(object sender, EventArgs e) {
        panel1.Controls.Remove(currentPage.Value.Panel);

        if (currentPage.Previous != null) {
            currentPage = currentPage.Previous;
            panel1.Controls.Add(currentPage.Value.Panel);
        } else {
            throw new InvalidOperationException();
        }
    }

    private void panel1_ControlAdded(object sender, ControlEventArgs e) {
        label1.Text = currentPage.Value.Title;
        label2.Text = currentPage.Value.Description;

        if (currentPage.Next != null) {
            buttonNext.Visible = true;
            buttonOK.Visible = false;
        } else {
            buttonNext.Visible = false;
            buttonOK.Visible = true;
        }

        buttonPrevious.Visible = currentPage.Previous != null;
    }
}
