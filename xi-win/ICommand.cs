using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xi_win
{
    public interface ICommand
    {
        String ToJSON();
        ICommand Parse(string command);

        int GetID();
        String GetCommandType();
        String GetParameterFromKey(string key);
    }

    public class CommandParser
    {
        public static ICommand Parse(string Command)
        {
            return null;
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
