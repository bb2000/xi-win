using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace xi_win
{
    public class CoreCommunication
    {
        StreamReader stdout;
        StreamWriter stdin;
        Process process;
        String inputBuffer;
        Thread inputThread;

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
        }

        internal void InputLoop()
        {
            while (true)
            {
                char inputChar = Convert.ToChar(stdout.Read());
                inputBuffer = inputBuffer + inputChar;
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
                    inputThread.Start();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public void StopInputLoop()
        {
            if (inputThread == null)
            {
                return;
            }
            else
            {
                try
                {
                    inputThread.Abort();
                    inputThread = null;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public String SendRawCommand(String command)
        {
            if (command.Last() != '\n')
            {
                command = command + '\n';
            }

            stdin.Write(command);
            stdin.Flush();

            while (inputBuffer.Length == 0 || inputBuffer.Last() != '\n')
            {
                continue;
            }

            String commandResult = inputBuffer;
            inputBuffer = "";

            return commandResult;
        }
    }
}
