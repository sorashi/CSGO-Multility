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
using System.Windows.Threading;
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
            // await UpdateManager.GitHubUpdateManager("https://github.com/Sorashi/CSGO-Multility")
            try {
                using (var mgr = new UpdateManager(@"C:\Users\praza\Documents\GitHub\CSGO-Multility\Multility\Releases")) {
                    SquirrelAwareApp.HandleEvents(
                        onInitialInstall: v => mgr.CreateShortcutForThisExe(),
                        onAppUpdate: v => mgr.CreateShortcutForThisExe(),
                        onAppUninstall: v => mgr.RemoveShortcutForThisExe(),
                        onFirstRun: () => main.ShowTheWelcomeWizard = true);
                    await mgr.UpdateApp();
                }
            }
            catch (Exception ex) {
                // update fail is ignored
#if DEBUG
                MessageBox.Show("There was an error trying to reach for the update information.\n" + ex.Message);
#endif
            }
            await Task.Delay(4000); // simulates the application loading process
            main.Show();
            this.Close();
        }
    }
}
