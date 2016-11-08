using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace xi_win
{
    class Program
    {
        static void Main(string[] args)
        {
            var core = new CoreCommunication();
            core.StartInputLoop();

            NewTabCommand ntc = new NewTabCommand(0);
            core.DebugSendCommand(ntc, true);

            //DeleteTabCommand dtc = new DeleteTabCommand(0, "1");
            //core.DebugSendCommand(dtc, false);

            OpenCommand oc = new OpenCommand(1, "C:\\\\Users\\\\walshb\\\\OneDrive\\\\Personal\\\\Path Variable SLP-BB-WalshB.txt");
            SaveCommand sc = new SaveCommand(1, "C:\\\\Users\\\\walshb\\\\OneDrive\\\\Personal\\\\Path Variable SLP-BB-WalshB.txt");
            //OpenCommand oc = new OpenCommand(1, "D:\\\\Bradyn Walsh\\\\OneDrive\\\\Personal\\\\Path Variable SLP-BB-WalshB.txt");
            //OpenCommand oc = new OpenCommand(1, "C:\\Users\\walshb\\OneDrive\\School\\2016 (Year 10)\\Food Technology\\Food Trucks\\Food Truck Investigation Assignment.docx");
            //core.DebugSendCommand(oc, false);

            RenderLinesCommand rlc = new RenderLinesCommand(0, 1, 3);
            EditCommand ec = new EditCommand(1, "0", oc);
            core.DebugSendCommand(ec, true);
            //core.CheckForCommand();
            

            EditCommand ec2 = new EditCommand(2, "0", rlc);
            core.DebugSendCommand(ec2, true);

            InsertCommand ic = new InsertCommand("a");
            EditCommand ec3 = new EditCommand(3, "0", ic);
            core.DebugSendCommand(ec3, true);

            InsertNewlineCommand inc = new InsertNewlineCommand();
            EditCommand ec32 = new EditCommand(5, "0", inc);
            core.DebugSendCommand(ec32, true);

            EditCommand ec4 = new EditCommand(4, "0", sc);
            core.DebugSendCommand(ec4, true);

            core.Dispose();
        }
    }
}
