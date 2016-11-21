using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xi_win
{
    public class Tab
    {
        public string tabName;
        public string fileName;
        List<Line> contents;
        int firstLine;
        public int height;

        public Tab(string name)
        {
            this.tabName = name;
            this.fileName = null;
            this.contents = new List<Line>();
            this.firstLine = 1;
        }

        public Tab(string name, string fileName)
        {
            this.tabName = name;
            this.fileName = fileName;
            this.contents = new List<Line>();
            this.firstLine = 0;
        }

        // Sets the contents of the tab
        public void SetContents(List<Line> contents)
        {
            this.contents = contents;
        }

        // Gets the contents and turn it into a string
        public string GetText()
        {
            var result = "";
            
            foreach (var line in contents)
            {
                result = result + line.text;
            }

            return result;
        }

        // Process an update command
        public void ProcessUpdate(UpdateCommand command)
        {
            this.firstLine = command.first_line;
            this.contents = command.lines;
            this.height = command.height;
        }
    }
}
