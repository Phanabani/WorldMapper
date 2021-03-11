using System.Windows.Threading;

namespace WorldMapper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var actualExc = e.Exception.InnerException;
            if (actualExc is null) return;
            // Console.WriteLine(actualExc.Message);
            // Console.WriteLine(actualExc.StackTrace);
        }
    }
}
