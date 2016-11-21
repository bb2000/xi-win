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

namespace xi_win
{
    /// <summary>
    /// Interaction logic for EditorUI.xaml
    /// </summary>
    public partial class EditorUI : Window
    {
        CoreCommunication core;
        List<Tab> tabs;
        int currentCommandId;
        int currentTabIndex;
        int cursorX;
        int cursorY;

        bool lCtrl;

        public EditorUI()
        {
            InitializeComponent();
            this.core = new CoreCommunication();
            core.StartInputLoop();
            this.currentCommandId = 0;
            this.currentTabIndex = -1;
            this.tabs = new List<Tab>();
            this.lCtrl = false;

            OpenInitalTab();
        }

        private void UpdateRender()
        {
            // Process commands
            ICommand command = core.RecieveCommand();
            UpdateRender(command);
        }

        private void UpdateRender(ICommand command)
        {
            this.currentTabIndex = tabBar.SelectedIndex;
            if (command != null)
            {
                switch (command.GetCommandType())
                {
                    case "new_tab_response":
                        tabs.Add(new Tab(command.GetParameterFromKey("result")));
                        currentTabIndex = tabs.Count - 1;
                        tabBar.SelectedIndex = currentTabIndex;
                        break;
                    case "error":
                        break;
                    case "update":
                        UpdateCommand uCommand = command as UpdateCommand;
                        tabs[currentTabIndex].ProcessUpdate(uCommand);
                        break;
                    default:
                        break;
                }
            }

            // Update tab bar
            while (tabBar.Items.Count != 0)
            {
                tabBar.Items.RemoveAt(0);
            }

            foreach (var tab in tabs)
            {
                tabBar.Items.Add(tab.tabName);
            }

            tabBar.SelectedIndex = currentTabIndex;

            // Update textbox
            if (currentTabIndex != -1)
                textBox.Text = tabs[currentTabIndex].GetText();//.Replace("\n", "\r\n");

            // Get next command
            ICommand nextCommand = core.RecieveCommand();
            if (nextCommand != null)
            {
                UpdateRender(nextCommand);
            }
        }

        private void OpenInitalTab()
        {
            
        }

        public void NewTab()
        {
            var ntc = new NewTabCommand(GetID());
            core.SendCommand(ntc, false);
            Thread.Sleep(100);
            UpdateRender();
        }

        private int GetID()
        {
            this.currentCommandId++;
            return currentCommandId - 1;
        }

        private void closeWindow(object sender, EventArgs e)
        {
            core.Dispose();
            System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvokeShutdown(System.Windows.Threading.DispatcherPriority.Send);
        }

        private void NewTab(object sender, RoutedEventArgs e)
        {
            NewTab();
        }

        private void closeTab(object sender, RoutedEventArgs e)
        {
            if (tabs.Count == 1)
            {
                closeWindow(null, null);
            }

            tabs.RemoveAt(currentTabIndex);
            this.currentTabIndex = currentTabIndex - 1;
            tabBar.SelectedIndex = this.currentTabIndex;
            UpdateRender();
        }

        private void openTab(object sender, RoutedEventArgs e)
        {
            NewTab();

            var fDialog = new Microsoft.Win32.OpenFileDialog();
            fDialog.ShowDialog();

            while (fDialog.FileName == null)
            {
                continue;
            }

            var fileName = fDialog.FileName.Replace("\\", "\\\\");

            var oc = new OpenCommand(-1, fileName);
            var ec = new EditCommand(GetID(), tabs[tabs.Count - 1].tabName, oc);
            core.SendCommand(ec, false);

            Thread.Sleep(1000);

            UpdateRender();
        }

        private void saveTab(object sender, RoutedEventArgs e)
        {
            string fileName = "";
            if (tabs[currentTabIndex].fileName == null)
            {
                var fDialog = new Microsoft.Win32.SaveFileDialog();
                fDialog.ShowDialog();

                while (fDialog.FileName == null)
                {
                    continue;
                }

                fileName = fDialog.FileName.Replace("\\", "\\\\");
            }
            else
            {
                fileName = tabs[currentTabIndex].fileName;
            }

            var sc = new SaveCommand(-1, fileName);
            var ec = new EditCommand(GetID(), tabs[currentTabIndex].tabName, sc);
            core.SendCommand(ec, false);
        }

        private void tabChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                currentTabIndex = tabBar.SelectedIndex;
                UpdateRender();
            }
        }

        private void updateTrigger(object sender, MouseButtonEventArgs e)
        {
            UpdateRender();
        }

        private void updateTrigger(object sender, MouseEventArgs e)
        {
            UpdateRender();
        }

        private void keyPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl)
            {
                lCtrl = true;
            }
            if (e.Key == Key.U && lCtrl)
            {
                UpdateRender();
            }
        }
    }
}
