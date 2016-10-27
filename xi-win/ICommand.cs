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
        Tuple<string, object> GetParameters();
        String GetEntryFromKey(string key);
    }

    public class CommandParser
    {
        public static ICommand Parse(string Command)
        {
            return null;
        }
    }
}
