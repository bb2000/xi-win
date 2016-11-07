using System;
using System.Collections;
using System.Collections.Generic;

namespace xi_win {
    public class NewTabCommand : ICommand
    {
        int ID;

        public NewTabCommand(int Id)
        {
            this.ID = Id;
        }

        public string GetCommandType()
        {
            return "new_tab";
        }

        public string GetParameterFromKey(string key)
        {
            throw new NotImplementedException();
        }

        public int GetID()
        {
            return ID;
        }

        public ICommand Parse(string command)
        {
            throw new NotImplementedException("Not a response");
        }

        public string ToJSON()
        {
            return "{\"id\":" + ID.ToString() + ",\"method\":\"new_tab\",\"params\":[]}";
        }
    }

    public class NewTabResponse : ICommand
    {
        public string GetCommandType()
        {
            throw new NotImplementedException();
        }

        public int GetID()
        {
            throw new NotImplementedException();
        }

        public string GetParameterFromKey(string key)
        {
            throw new NotImplementedException();
        }

        public ICommand Parse(string command)
        {
            throw new NotImplementedException();
        }

        public string ToJSON()
        {
            throw new NotImplementedException();
        }
    }

    public class DeleteTabCommand : ICommand
    {
        int ID;
        string tab_name;
        
        public DeleteTabCommand(int ID, string tab)
        {
            this.ID = ID;
            this.tab_name = tab;
        }

        public string GetCommandType()
        {
            return "delete_tab";
        }

        public string GetParameterFromKey(string key)
        {
            if (key == "tab")
            {
                return tab_name;
            }
            else
            {
                return null;
            }
        }

        public int GetID()
        {
            return ID;
        }

        public ICommand Parse(string command)
        {
            throw new NotImplementedException("Not a response");
        }

        public string ToJSON()
        {
            return "{\"id\":" + ID.ToString() + ",\"method\":\"delete_tab\",\"params\":{\"tab\":\"" + tab_name + "\"}}";
        }
    }

    public class OpenCommand : ICommand
    {
        int ID;
        string filename;

        public OpenCommand(int ID, string filename)
        {
            this.ID = ID;
            this.filename = filename;
        }

        public string GetCommandType()
        {
            return "open";
        }

        public int GetID()
        {
            throw new NotImplementedException();
        }

        public string GetParameterFromKey(string key)
        {
            if (key == "params")
            {
                return "{\"filename\":\"" + filename + "\"}"; 
            }

            throw new NotImplementedException();
        }

        public ICommand Parse(string command)
        {
            throw new NotImplementedException();
        }

        public string ToJSON()
        {
            return "{\"id\":" + ID.ToString() + ",\"method\":\"open\",\"params\":{\"filename\":\"" + filename + "\"}}";
        }
    }

    public class SaveCommand : ICommand
    {
        string filename;

        public SaveCommand(int ID, string filename)
        {
            this.filename = filename;
        }

        public string GetCommandType()
        {
            return "save";
        }

        public int GetID()
        {
            throw new NotImplementedException();
        }

        public string GetParameterFromKey(string key)
        {
            if (key == "params")
            {
                return "{\"filename\":\"" + filename + "\"}";
            }

            throw new NotImplementedException();
        }

        public ICommand Parse(string command)
        {
            throw new NotImplementedException();
        }

        public string ToJSON()
        {
            return "{\"method\":\"save\",\"params\":{\"filename\":\"" + filename + "\"}}";
        }
    }

    public class RenderLinesCommand : ICommand
    {
        int ID;
        int first_line;
        int last_line;

        public RenderLinesCommand(int Id, int first_line, int last_line)
        {
            this.ID = Id;
            this.first_line = first_line;
            this.last_line = last_line;
        }

        public string GetCommandType()
        {
            return "render_lines";
        }

        public int GetID()
        {
            throw new NotImplementedException();
        }

        public string GetParameterFromKey(string key)
        {
            if (key == "params")
            {
                return "{\"first_line\":" + first_line.ToString() + ",\"last_line\": " + last_line.ToString() + "}";
            }
            throw new NotImplementedException();
        }

        public ICommand Parse(string command)
        {
            throw new NotImplementedException();
        }

        public string ToJSON()
        {
            return "{\"id\":" + ID.ToString() + ",\"method\":\"render_lines\",\"params\":{first_line:" + first_line.ToString() + ",last_line:" + last_line.ToString() + "}}";
        }
    }

    public class InsertCommand : ICommand
    {
        string chars;

        public InsertCommand(string chars)
        {
            this.chars = chars;
        }

        public string GetCommandType()
        {
            return "insert";
        }

        public int GetID()
        {
            throw new NotImplementedException();
        }

        public string GetParameterFromKey(string key)
        {
            if (key == "params") {
                return "{\"chars\":\"" + chars + "\"}";
            }
            throw new NotImplementedException();
        }

        public ICommand Parse(string command)
        {
            throw new NotImplementedException();
        }

        public string ToJSON()
        {
            throw new NotImplementedException();
        }
    }

    public class ScrollCommand : ICommand
    {
        int firstline;
        int lastline; // This is non-inclusive

        public ScrollCommand(int firstline, int lastline)
        {
            this.firstline = firstline;
            this.lastline = lastline;
        }

        public string GetCommandType()
        {
            return "scroll";
        }

        public int GetID()
        {
            throw new NotImplementedException();
        }

        public string GetParameterFromKey(string key)
        {
            if (key == "params")
            {
                return "[" + firstline.ToString() + "," + lastline.ToString() + "]";
            }
            throw new NotImplementedException();
        }

        public ICommand Parse(string command)
        {
            throw new NotImplementedException();
        }

        public string ToJSON()
        {
            throw new NotImplementedException();
        }
    }

    public class ClickCommand : ICommand
    {
        int line;
        int col;
        int modifiers; // 2 is shift
        int count;

        public ClickCommand(int line, int col, int modifiers, int count)
        {
            this.line = line;
            this.col = col;
            this.modifiers = modifiers;
            this.count = count;
        }

        public string GetCommandType()
        {
            return "click";
        }

        public int GetID()
        {
            throw new NotImplementedException();
        }

        public string GetParameterFromKey(string key)
        {
            if (key == "params")
            {
                return "[" + line.ToString() + "," + col.ToString() + "," + modifiers.ToString() + "," + count.ToString() + "]";
            }
            throw new NotImplementedException();
        }

        public ICommand Parse(string command)
        {
            throw new NotImplementedException();
        }

        public string ToJSON()
        {
            throw new NotImplementedException();
        }
    }

    public class EditCommand : ICommand
    {
        int ID;
        string tab;
        ICommand interior_command;

        public EditCommand(int ID, string tab, ICommand interior_command)
        {
            this.ID = ID;
            this.tab = tab;
            this.interior_command = interior_command;
        }

        public string GetCommandType()
        {
            throw new NotImplementedException();
        }

        public int GetID()
        {
            return ID;
        }

        public string GetParameterFromKey(string key)
        {
            throw new NotImplementedException();
        }

        public ICommand Parse(string command)
        {
            throw new NotImplementedException();
        }

        public string ToJSON()
        {
            return "{\"id\":" + ID.ToString() + ",\"method\":\"edit\",\"params\":{\"method\":\"" + interior_command.GetCommandType() + "\",\"params\":" + interior_command.GetParameterFromKey("params") + ",\"tab\":\"" + tab + "\"}}";
        }
    }
}