#nullable enable
namespace ProjectV.Processor;

public partial class FormInput {
    public string Input => textBox1.Text;

    public FormInput(string title, string content) {
        InitializeComponent();
        Text = title;
        label1.Text = content;
    }
}