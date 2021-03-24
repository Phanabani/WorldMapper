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

            var tBox = (TextBox) sender;
            var prop = TextBox.TextProperty;
            var binding = BindingOperations.GetBindingExpression(tBox, prop);
            binding?.UpdateSource();
        }

        private void StartOverlay_OnClick(object sender, RoutedEventArgs e)
        {
            StartOverlayWindow();
        }

        private void StopOverlay_OnClick(object sender, RoutedEventArgs e)
        {
            StopOverlayWindow();
        }

        private void StartOverlayWindow()
        {
            _overlayWindow = new OverlayWindow()
            {
                Owner = Application.Current.MainWindow,
                ControlsData = Resources["ControlsData"] as ControlsData
            };
            _overlayWindow.Show();
        }

        private void StopOverlayWindow()
        {
            _overlayWindow?.Close();
        }
    }
}
