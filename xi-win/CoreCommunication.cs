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
            // Gets the file name of the xi core executable from either default value or ENV variable
            string xi_core = Environment.GetEnvironmentVariable("XI-CORE", EnvironmentVariableTarget.User);
            if (xi_core == null)
            {
                throw new FileNotFoundException("XI-CORE ENV Variable Not Set");
            }
            
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
                process.Start();
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

        internal void InputLoop()
        {
            while (inputTaskRunning)
            {
                var inputTask = Task.Run(() => stdout.Read());
                if (inputTask.Wait(TimeSpan.FromMilliseconds(10000)))
                {
                    if (inputTask.Result != -1)
                    {
                        char inputChar = Convert.ToChar(inputTask.Result);
                        inputBuffer = inputBuffer + inputChar;
                    }
                }
            }
        }

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

        public string RecieveRawCommand()
        {
            while (!inputBuffer.Contains('\n'))
            {
                continue;
            }
            if (inputBuffer.Contains('\n'))
            {
                var nlIndex = inputBuffer.IndexOf('\n');
                return inputBuffer.Remove(0, nlIndex);
            }
            else
            {
                return null;
            }
        }

        public ICommand SendCommand(ICommand command)
        {
            return SendCommand(command, true);
        }

        public void DebugSendCommand(ICommand command, bool waitForResponse)
        {
            Console.Write(SendRawCommand(command.ToJSON(), waitForResponse));
            Console.ReadLine();
            this.CheckForCommand(); // Hacky solution to error missing return value problem; TODO: FIX THIS
        }

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

        public void Dispose()
        {
            inputTaskRunning = false;
            Thread.Sleep(100);
            stdin.Close();
            stdout.Close();
        }
    }
}
