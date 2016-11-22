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
        int currIndex;

        bool lCtrl;
        bool rCtrl;
        bool shift;

        string newFileName;

        public EditorUI()
        {
            InitializeComponent();
            this.core = new CoreCommunication();
            core.StartInputLoop();
            this.currentCommandId = 0;
            this.currIndex = 0;
            this.currentTabIndex = -1;
            this.tabs = new List<Tab>();
            this.lCtrl = false;
            this.rCtrl = false;
            this.newFileName = "";

            textBox.VerticalScrollBarVisibility = ScrollBarVisibility.Visible; // Gives us a scrollbar
            textBox.IsReadOnly = true; // Eliminates ghosting
            textBox.IsReadOnlyCaretVisible = true;

            OpenInitalTab();
        }

        // Grabs command to update/render UI
        private void UpdateRender()
        {
            // Process commands
            ICommand command = core.RecieveCommand();
            UpdateRender(command);
        }

        // Updates UI State and re-renders
        private void UpdateRender(ICommand command)
        {
            this.currentTabIndex = tabBar.SelectedIndex;
            this.currIndex = textBox.CaretIndex;

            if (command != null)
            {
                switch (command.GetCommandType()) // Process the command
                {
                    case "new_tab_response":
                        tabs.Add(new Tab(command.GetParameterFromKey("result")));
                        currentTabIndex = tabs.Count - 1;
                        tabBar.SelectedIndex = currentTabIndex;

                        tabs[currentTabIndex].fileName = this.newFileName;
                        this.newFileName = "";

                        ScrollCommand sc = new ScrollCommand(0, 1000); // Makes sure that all text is visible
                        EditCommand ec = new EditCommand(GetID(), tabs[currentTabIndex].tabName, sc);
                        core.SendCommand(ec, false);
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
                tabBar.Items.Add(tab.tabName); // Add each tab to the tabbar
            }

            tabBar.SelectedIndex = currentTabIndex;

            // Update textbox
            textBox.Text = "";
            textBox.CaretIndex = this.currIndex;
            if (currentTabIndex != -1)
            {
                textBox.Text = tabs[currentTabIndex].GetText();//.Replace("\n", "\r\n");
                textBox.CaretIndex = this.currIndex;
            }

            // Update cursor
            var cursorIndex = this.currIndex;
            var oldCursorX = this.cursorX;
            var oldCursorY = this.cursorY;
            var currY = 0;
            var currX = 0;
            foreach (var line in textBox.Text.Split('\n'))
            {
                currY++;
                if (currX + (line.Length + 1) > cursorIndex)
                {
                    this.cursorY = currY - 1;
                    this.cursorX = cursorIndex - currX;
                    this.currIndex = cursorIndex;
                    break;
                }
                currX += line.Length;
                cursorIndex--;
            }

            // Click on new cursor position if it has changed
            if (oldCursorX != cursorX || oldCursorY != cursorY)
            {
                ClickCommand cc = new ClickCommand(cursorY, cursorX, 0, 1);
                EditCommand ec = new EditCommand(GetID(), tabs[currentTabIndex].tabName, cc);
                core.SendCommand(ec, false);
                Thread.Sleep(100);
                UpdateRender();
            }

            // Get next command
            ICommand nextCommand = core.RecieveCommand();
            if (nextCommand != null)
            {
                UpdateRender(nextCommand);
            }
        }

        // Opens README.md in exe folder
        private void OpenInitalTab()
        {
            NewTab();

            string filename = Environment.CurrentDirectory + "\\README.md";
            filename = filename.Replace("\\", "\\\\");
            var oc = new OpenCommand(-1, filename);
            var ec = new EditCommand(GetID(), tabs[0].tabName, oc);
            core.SendCommand(ec, false);

            Thread.Sleep(100);

            UpdateRender();

            tabs[0].fileName = filename;
        }

        // Creates a new tab
        public void NewTab()
        {
            var ntc = new NewTabCommand(GetID());
            core.SendCommand(ntc, false);
            Thread.Sleep(100);
            UpdateRender();
        }

        // Gets current ID and increment ID
        private int GetID()
        {
            this.currentCommandId++;
            return currentCommandId - 1;
        }

        // Close the app
        private void closeWindow(object sender, EventArgs e)
        {
            core.Dispose();
            System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvokeShutdown(System.Windows.Threading.DispatcherPriority.Send); // I have no idea what this does, but the app won't close without it
        }

        // New Tab button pressed
        private void NewTab(object sender, RoutedEventArgs e)
        {
            NewTab();
        }

        // Closes currently open tab
        // TODO: Close in the core too
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

        // Opens a tab
        private void openTab(object sender, RoutedEventArgs e)
        {
            NewTab();

            var fDialog = new Microsoft.Win32.OpenFileDialog();
            fDialog.ShowDialog(); // Shows a file dialog and gets the filename selected

            while (fDialog.FileName == null)
            {
                continue;
            }

            var fileName = fDialog.FileName.Replace("\\", "\\\\"); // Weird core thing

            this.newFileName = fileName;

            var oc = new OpenCommand(-1, fileName);
            var ec = new EditCommand(GetID(), tabs[tabs.Count - 1].tabName, oc);
            core.SendCommand(ec, false);

            Thread.Sleep(1000);

            UpdateRender();
        }

        // Saves current tab
        private void saveTab(object sender, RoutedEventArgs e)
        {
            string fileName = "";
            if (tabs[currentTabIndex].fileName == null)
            {
                var fDialog = new Microsoft.Win32.SaveFileDialog();
                fDialog.ShowDialog(); // Shows save dialog and gets filename selected

                while (fDialog.FileName == null)
                {
                    continue;
                }

                fileName = fDialog.FileName.Replace("\\", "\\\\"); // Weird core thing
            }
            else
            {
                fileName = tabs[currentTabIndex].fileName;
            }

            tabs[currentTabIndex].fileName = fileName;
            var sc = new SaveCommand(-1, fileName);
            var ec = new EditCommand(GetID(), tabs[currentTabIndex].tabName, sc);
            core.SendCommand(ec, false);
        }

        // Fired when the tab is changed
        private void tabChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                currentTabIndex = tabBar.SelectedIndex;
                UpdateRender();
            }
        }

        // Updates UI State and renders when event is fired
        private void updateTrigger(object sender, MouseButtonEventArgs e)
        {
            UpdateRender();
        }

        // Updates UI State and renders when event is fired
        private void updateTrigger(object sender, MouseEventArgs e)
        {
            UpdateRender();
        }

        // Fired when a key is pressed
        private void keyPressed(object sender, KeyEventArgs e)
        {
            UpdateRender();
            if (e.Key == Key.LeftCtrl)
            {
                lCtrl = true;
                rCtrl = false;
            }
            else if (e.Key == Key.RightCtrl)
            {
                rCtrl = true;
                lCtrl = false;
            }
            else if (e.Key == Key.Escape)
            {
                rCtrl = false;
                lCtrl = false;
            }
            else if (e.Key == Key.Delete)
            {
                DeleteBackwardCommand dbc = new DeleteBackwardCommand();
                EditCommand ec = new EditCommand(GetID(), tabs[currentTabIndex].tabName, dbc);
                core.SendCommand(ec, false);
                Thread.Sleep(300);
                UpdateRender();
            }
            else if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
            {
                shift = true;
            }
            else if (e.Key == Key.U && rCtrl)
            {
                UpdateRender();
            }
            else if (e.Key == Key.O && lCtrl)
            {
                openTab(null, null);
            }
            else if (e.Key == Key.S && lCtrl)
            {
                saveTab(null, null);
            }
            else if (e.Key == Key.Back)
            {
                textBox.CaretIndex -= 1;
                UpdateRender();

                DeleteBackwardCommand dbc = new DeleteBackwardCommand();
                EditCommand ec = new EditCommand(GetID(), tabs[currentTabIndex].tabName, dbc);
                core.SendCommand(ec, false);
                Thread.Sleep(300);
                UpdateRender();
            }
            else if (e.Key == Key.Enter)
            {
                // Handled by preview
            }
            else
            {
                string input = e.Key.ToString();
                if (input.Length == 2)
                    input = input.Remove(0, 1);
                if (!shift)
                    input = input.ToLower();
                InsertCommand ic = new InsertCommand(input);
                EditCommand ec = new EditCommand(GetID(), tabs[currentTabIndex].tabName, ic);
                var command = core.SendCommand(ec, true);
                //textBox.CaretIndex -= 1;
                if (textBox.Text == "")
                {
                    // Do nothing
                }
                else if (tabs[currentTabIndex].fileName == "")
                {
                    textBox.CaretIndex = textBox.CaretIndex + 1;
                    UpdateRender();
                }
                else
                {
                    textBox.CaretIndex += 1;
                    UpdateRender();
                }
                UpdateRender(command);
            }
        }

        // Fired when a key is depressed
        private void keyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
            {
                shift = false;
            }
        }

        // Fired when control keys are pressed
        private void previewPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                //textBox.CaretIndex -= 2;
                UpdateRender();

                DeleteBackwardCommand dbc = new DeleteBackwardCommand();
                EditCommand ec = new EditCommand(GetID(), tabs[currentTabIndex].tabName, dbc);
                core.SendCommand(ec, false);
                Thread.Sleep(300);
                textBox.CaretIndex -= 1;
                UpdateRender();
            }
            else if (e.Key == Key.Delete)
            {
                DeleteBackwardCommand dbc = new DeleteBackwardCommand();
                EditCommand ec = new EditCommand(GetID(), tabs[currentTabIndex].tabName, dbc);
                core.SendCommand(ec, false);
                Thread.Sleep(300);
                UpdateRender();
            }
            else if (e.Key == Key.Enter)
            {
                InsertNewlineCommand inlc = new InsertNewlineCommand();
                EditCommand ec = new EditCommand(GetID(), tabs[currentTabIndex].tabName, inlc);
                var command = core.SendCommand(ec, true);
                UpdateRender(command);
                textBox.CaretIndex += 1;
            }
        }
    }
}
