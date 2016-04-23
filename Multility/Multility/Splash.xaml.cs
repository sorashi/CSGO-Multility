using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Squirrel;

namespace Multility
{
    /// <summary>
    /// Interaction logic for Splash.xaml
    /// </summary>
    public partial class Splash : Window
    {
        public Splash()
        {
            InitializeComponent();
        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            var main = new MainWindow();
            try
            {
                using (var mgr = await UpdateManager.GitHubUpdateManager("https://github.com/Sorashi/CSGO-Multility"))
                {
                    SquirrelAwareApp.HandleEvents(
                        onInitialInstall: v => mgr.CreateShortcutForThisExe(),
                        onAppUpdate: v => mgr.CreateShortcutForThisExe(),
                        onAppUninstall: v => mgr.RemoveShortcutForThisExe(),
                        onFirstRun: () => main.ShowTheWelcomeWizard = true);
                    await mgr.UpdateApp();
                }
            }
            catch
            {
                // update fail is ignored
            }
            await Task.Delay(3000); // simulates the application loading process
            main.Show();
            this.Close();
        }
    }
}
