using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xi_win
{
    // Interface for a command to be sent or received from xi-core
    public interface ICommand
    {
        String ToJSON(); // Convert command to JSON to be sent to core
        ICommand Parse(string command); // Parse incoming command

        int GetID(); // Get ID of command
        String GetCommandType(); // Returns type of command
        String GetParameterFromKey(string key); // Get one of the parameters of the command (differs from command to command)
    }

    public class CommandParser
    {
        public static ICommand Parse(string Command)
        {
            if (ErrorCommand.ParseStatic(Command) != null)
            {
                return ErrorCommand.ParseStatic(Command);
            }
            else if (UpdateCommand.ParseStatic(Command) != null)
            {
                return UpdateCommand.ParseStatic(Command);
            }
            else if (NewTabResponse.ParseStatic(Command) != null)
            {
                return NewTabResponse.ParseStatic(Command);
            }
            else
            {
                return null;
            }
        }

        public static string ParseObject(object element)
        {
            switch (element.GetType().ToString())
            {
                case "System.String":
                    return"\"" + element + "\"";
                    break;
                case "System.Int32":
                    return element.ToString();
                    break;
                case "System.Collections.Generic.List`1[System.Object]":
                    string result = "[";
                    foreach (var arrayElement in (element as IEnumerable<object>))
                    {
                        result += ParseObject(arrayElement);
                        result += ",";
                    }
                    result += "]";
                    return result;
                default:
                    throw new Exception(element.GetType().ToString());
            }
        }
    }
}
