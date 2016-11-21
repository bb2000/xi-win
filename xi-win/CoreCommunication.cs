using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace xi_win
{
    public class CoreCommunication : IDisposable
    {
        StreamReader stdout;
        StreamWriter stdin;
        Process process;
        String inputBuffer;
        Thread inputThread;
        public string xicoreexe;
        volatile bool inputTaskRunning;

        public CoreCommunication()
        {
            string xi_core = Environment.CurrentDirectory + "\\xi-core.exe"; // Gets xi-core filename in same folder as .exe
                        
            // Construct class after calling other constructor
            CoreCommunication a = new CoreCommunication(xi_core);
            this.stdin = a.stdin;
            this.stdout = a.stdout;
            this.process = a.process;
            this.inputBuffer = a.inputBuffer;
            this.inputThread = a.inputThread;
            this.inputTaskRunning = a.inputTaskRunning;
            this.xicoreexe = xi_core;
        }

        public CoreCommunication(string xi_core)
        {
            // Starts xi core
            Process process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.FileName = xi_core;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            try
            {
                process.Start(); // Starts core
            }
            catch (Exception e)
            {
                throw e;
            }

            // Gets the process stdin and stdout
            StreamWriter stdin = process.StandardInput;
            StreamReader stdout = process.StandardOutput;

            // Constructs class
            this.stdout = stdout;
            this.stdin = stdin;
            this.process = process;
            this.inputBuffer = "";
            this.inputThread = null;
            this.xicoreexe = null;
        }

        // Loop that gets inputs from the core
        internal void InputLoop()
        {
            while (inputTaskRunning)
            {
                var inputTask = Task.Run(() => stdout.Read());
                if (inputTask.Wait(TimeSpan.FromMilliseconds(10000))) // 10 second timeout for the loop
                {
                    if (inputTask.Result != -1)
                    {
                        char inputChar = Convert.ToChar(inputTask.Result);
                        inputBuffer = inputBuffer + inputChar;
                    }
                }
            }
        }

        // Starts input loop
        public void StartInputLoop()
        {
            if (inputThread != null)
            {
                return;
            }
            else
            {
                try
                {
                    inputThread = new Thread(InputLoop);
                    inputTaskRunning = true;
                    inputThread.Start();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        // Sends a raw string to the core
        public String SendRawCommand(String command, bool waitForResponse)
        {
            if (command.Last() != '\n')
            {
                command = command + '\n';
            }

            stdin.Write(command);
            stdin.Flush();

            if (!waitForResponse)
            {
                return null;
            }

            return RecieveRawCommand();
        }

        // Send a command to the core
        public ICommand SendCommand(ICommand command, bool waitForResponse)
        {
            string response = SendRawCommand(command.ToJSON(), waitForResponse);

            if (response == null)
            {
                return null;
            }
            else
            {
                return CommandParser.Parse(response);
            }
        }

        // Checks for and recieves command from the core
        public ICommand RecieveCommand()
        {
            if (inputBuffer.Contains('\n'))
            {
                var nlIndex = inputBuffer.IndexOf('\n');
                string response = inputBuffer.Substring(0, nlIndex);
                inputBuffer = inputBuffer.Remove(0, nlIndex + 1);
                return CommandParser.Parse(response);
            }
            else
            {
                return null;
            }
        }

        // Checks for and receives a raw string from the core
        public string RecieveRawCommand()
        {
            while (!inputBuffer.Contains('\n'))
            {
                continue;
            }
            if (inputBuffer.Contains('\n'))
            {
                var nlIndex = inputBuffer.IndexOf('\n');
                string response = inputBuffer.Substring(0, nlIndex);
                inputBuffer = inputBuffer.Remove(0, nlIndex + 1);
                return response;
            }
            else
            {
                return null;
            }
        }

        // Sends command with default waitForResponse variable
        public ICommand SendCommand(ICommand command)
        {
            return SendCommand(command, true);
        }

        // Debug function for sending commands
        public void DebugSendCommand(ICommand command, bool waitForResponse)
        {
            Console.Write(SendRawCommand(command.ToJSON(), waitForResponse));
            Console.ReadLine();
            this.CheckForCommand(); // Hacky solution to error missing return value problem; TODO: FIX THIS
        }

        // Checks for a command from the core
        public String CheckForCommand()
        {
            if (inputBuffer.Length == 0 || inputBuffer.Last() != '\n')
            {
                return null;
            }
            else
            {
                String command = inputBuffer;
                inputBuffer = "";
                return inputBuffer;
            }
        }

        // End the core process and kill the I/O threads
        public void Dispose()
        {
            inputTaskRunning = false;
            Thread.Sleep(100);
            stdin.Close();
            stdout.Close();
        }
    }
}
