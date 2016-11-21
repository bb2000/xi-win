using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace xi_win
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Creates new editor window
            var window = new EditorUI();
            window.Show();

            // This is needed to make it all work. I have no idea why.
            System.Windows.Threading.Dispatcher.Run();
        }
    }
}
