using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace WorldMapper.Pages
{
    public partial class ControlsPage
    {
        public ControlsPage()
        {
            InitializeComponent();
        }

        private void TextBox_KeyEnterUpdate(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;

            var tBox = (TextBox)sender;
            var prop = TextBox.TextProperty;
            var binding = BindingOperations.GetBindingExpression(tBox, prop);
            binding?.UpdateSource();
        }
    }
}
