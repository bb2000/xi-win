using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        int id;
        string result;

        public NewTabResponse(int id, string result)
        {
            this.id = id;
            this.result = result;
        }

        public string GetCommandType()
        {
            return "new_tab_response";
        }

        public int GetID()
        {
            return id;
        }

        public string GetParameterFromKey(string key)
        {
            if (key == "result")
            {
                return result;
            }
            throw new NotImplementedException();
        }

        public static ICommand ParseStatic(string command)
        {
            if (!command.Contains("result"))
            {
                return null;
            }
            else
            {
                var parsedJSON = JsonConvert.DeserializeObject<NewTabResponse>(command);
                return parsedJSON;
            }
        }

        public ICommand Parse(string command)
        {
            if (!command.Contains("result"))
            {
                return null;
            }
            else
            {
                var parsedJSON = JsonConvert.DeserializeObject<NewTabResponse>(command);
                return parsedJSON;
            }
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

    public class DragCommand : ICommand
    {
        int line;
        int col;
        int modifiers;

        public DragCommand(int line, int col, int modifiers)
        {
            this.line = line;
            this.col = col;
            this.modifiers = modifiers;
        }

        public string GetCommandType()
        {
            return "drag";
        }

        public int GetID()
        {
            throw new NotImplementedException();
        }

        public string GetParameterFromKey(string key)
        {
            if (key == "params")
            {
                return "[" + line.ToString() + "," + col.ToString() + "," + modifiers.ToString() + "]";
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

    public class DeleteBackwardCommand : ICommand
    {
        public DeleteBackwardCommand()
        {

        }

        public string GetCommandType()
        {
            return "delete_backward";
        }

        public int GetID()
        {
            throw new NotImplementedException();
        }

        public string GetParameterFromKey(string key)
        {
            if (key == "params")
            {
                return "{}";
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

    public class InsertNewlineCommand : ICommand
    {
        public InsertNewlineCommand()
        {

        }

        public string GetCommandType()
        {
            return "insert_newline";
        }

        public int GetID()
        {
            throw new NotImplementedException();
        }

        public string GetParameterFromKey(string key)
        {
            if (key == "params") {
                return "{}";
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

    public class MoveUpCommand : ICommand
    {
        public MoveUpCommand()
        {

        }

        public string GetCommandType()
        {
            return "move_up";
        }

        public int GetID()
        {
            throw new NotImplementedException();
        }

        public string GetParameterFromKey(string key)
        {
            if (key == "params")
            {
                return "{}";
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

    public class MoveUpAndModifySelectionCommand : ICommand
    {
        public MoveUpAndModifySelectionCommand()
        {

        }

        public string GetCommandType()
        {
            return "move_up_and_modify_selection";
        }

        public int GetID()
        {
            throw new NotImplementedException();
        }

        public string GetParameterFromKey(string key)
        {
            if (key == "params")
            {
                return "{}";
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

    public class MoveDownCommand : ICommand
    {
        public MoveDownCommand()
        {

        }

        public string GetCommandType()
        {
            return "move_down";
        }

        public int GetID()
        {
            throw new NotImplementedException();
        }

        public string GetParameterFromKey(string key)
        {
            if (key == "params")
            {
                return "{}";
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

    public class MoveDownAndModifySelectionCommand : ICommand
    {
        public MoveDownAndModifySelectionCommand()
        {

        }

        public string GetCommandType()
        {
            return "move_down_and_modify_selection";
        }

        public int GetID()
        {
            throw new NotImplementedException();
        }

        public string GetParameterFromKey(string key)
        {
            if (key == "params")
            {
                return "{}";
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

    public class MoveLeftCommand : ICommand
    {
        public MoveLeftCommand()
        {

        }

        public string GetCommandType()
        {
            return "move_left";
        }

        public int GetID()
        {
            throw new NotImplementedException();
        }

        public string GetParameterFromKey(string key)
        {
            if (key == "params")
            {
                return "{}";
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

    public class MoveLeftAndModifySelectionCommand : ICommand
    {
        public MoveLeftAndModifySelectionCommand()
        {

        }

        public string GetCommandType()
        {
            return "move_left_and_modify_selection";
        }

        public int GetID()
        {
            throw new NotImplementedException();
        }

        public string GetParameterFromKey(string key)
        {
            if (key == "params")
            {
                return "{}";
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

    public class MoveRightCommand : ICommand
    {
        public MoveRightCommand()
        {

        }

        public string GetCommandType()
        {
            return "move_right";
        }

        public int GetID()
        {
            throw new NotImplementedException();
        }

        public string GetParameterFromKey(string key)
        {
            if (key == "params")
            {
                return "{}";
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

    public class MoveRightAndModifySelectionCommand : ICommand
    {
        public MoveRightAndModifySelectionCommand()
        {

        }

        public string GetCommandType()
        {
            return "move_right_and_modify_selection";
        }

        public int GetID()
        {
            throw new NotImplementedException();
        }

        public string GetParameterFromKey(string key)
        {
            if (key == "params")
            {
                return "{}";
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

    public class ScrollPageUpCommand : ICommand
    {
        public ScrollPageUpCommand()
        {

        }

        public string GetCommandType()
        {
            return "scroll_page_up";
        }

        public int GetID()
        {
            throw new NotImplementedException();
        }

        public string GetParameterFromKey(string key)
        {
            if (key == "params")
            {
                return "{}";
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

    public class PageUpCommand : ICommand
    {
        public PageUpCommand()
        {

        }

        public string GetCommandType()
        {
            return "page_up";
        }

        public int GetID()
        {
            throw new NotImplementedException();
        }

        public string GetParameterFromKey(string key)
        {
            if (key == "params")
            {
                return "{}";
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

    public class PageUpAndModifySelectionCommand : ICommand
    {
        public PageUpAndModifySelectionCommand()
        {

        }

        public string GetCommandType()
        {
            return "page_up_and_modify_selection";
        }

        public int GetID()
        {
            throw new NotImplementedException();
        }

        public string GetParameterFromKey(string key)
        {
            if (key == "params")
            {
                return "{}";
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

    public class ScrollPageDownCommand : ICommand
    {
        public ScrollPageDownCommand()
        {

        }

        public string GetCommandType()
        {
            return "scroll_page_down";
        }

        public int GetID()
        {
            throw new NotImplementedException();
        }

        public string GetParameterFromKey(string key)
        {
            if (key == "params")
            {
                return "{}";
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

    public class PageDownCommand : ICommand
    {
        public PageDownCommand()
        {

        }

        public string GetCommandType()
        {
            return "page_down";
        }

        public int GetID()
        {
            throw new NotImplementedException();
        }

        public string GetParameterFromKey(string key)
        {
            if (key == "params")
            {
                return "{}";
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

    public class PageDownAndModifySelectionCommand : ICommand
    {
        public PageDownAndModifySelectionCommand()
        {

        }

        public string GetCommandType()
        {
            return "page_down_and_modify_selection";
        }

        public int GetID()
        {
            throw new NotImplementedException();
        }

        public string GetParameterFromKey(string key)
        {
            if (key == "params")
            {
                return "{}";
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

    public class ErrorCommand : ICommand
    {
        int id;
        string error;

        public ErrorCommand(int ID, string errorString)
        {
            this.id = ID;
            this.error = errorString;
        }

        public string GetCommandType()
        {
            return "error";
        }

        public int GetID()
        {
            return id;
        }

        public string GetParameterFromKey(string key)
        {
            if (key == "error")
            {
                return error;
            }
            throw new NotImplementedException();
        }

        public static ICommand ParseStatic(string command)
        {
            if (!command.Contains("\"error\":"))
            {
                return null;
            }
            else
            {
                var parsedJSON = JsonConvert.DeserializeObject<ErrorCommand>(command);
                return parsedJSON;
            }
        }

        public ICommand Parse(string command)
        {
            if (!command.Contains("\"error\":"))
            {
                return null;
            }
            else
            {
                var parsedJSON = JsonConvert.DeserializeObject<ErrorCommand>(command);
                return parsedJSON;
            }
        }

        public string ToJSON()
        {
            throw new NotImplementedException();
        }
    }

    public class Line
    {
        string text;
        int cursor;
        Tuple<int, int> sel;
        Tuple<int, int, int> fg;

        public Line(JToken line)
        {
            this.text = line.First.Value<string>();

            int index = 0;
            foreach (var val in line)
            {
                if (index != 0)
                {
                    var argArray = val as JArray;
                    var argType = argArray[0].Value<string>();

                    switch (argType)
                    {
                        case "cursor":
                            this.cursor = argArray[1].Value<int>();
                            break;
                        default:
                            break;
                    }
                }
                index++;
            }
        }
    }

    public class UpdateCommand : ICommand
    {
        string tab;
        int first_line;
        int height;
        List<Line> lines;
        Tuple<int, int> scrollto;

        public UpdateCommand()
        {

        }

        public string ToJSON()
        {
            throw new NotImplementedException();
        }

        public ICommand Parse(string command)
        {
            throw new NotImplementedException();
        }

        public static ICommand ParseJSONNET(Newtonsoft.Json.Linq.JObject Jsobject)
        {
            UpdateCommand command = new UpdateCommand();
            command.lines = new List<Line>();

            Newtonsoft.Json.Linq.JToken parameters = Jsobject.GetValue("params");
            command.tab = parameters.Value<string>("tab").ToString();

            var updateParameters = parameters.Value<JToken>("update");

            command.first_line = updateParameters.Value<int>("first_line");
            command.height = updateParameters.Value<int>("height");

            var lineArray = updateParameters.Value<JArray>("lines");

            foreach (var line in lineArray)
            {
                command.lines.Add(new Line(line));
            }

            var scrollto = updateParameters.Value<JToken>("srcollto");
            if (scrollto != null)
            {
                throw new NotImplementedException("What does a scrollto look like?");
            }

            return command;
        }

        public static ICommand ParseStatic(string command)
        {
            if (!command.Contains("update"))
            {
                return null;
            }
            else
            {
                return ParseJSONNET(JsonConvert.DeserializeObject(command) as Newtonsoft.Json.Linq.JObject);
            }
        }

        public int GetID()
        {
            throw new NotImplementedException();
        }

        public string GetCommandType()
        {
            throw new NotImplementedException();
        }

        public string GetParameterFromKey(string key)
        {
            throw new NotImplementedException();
        }
    }
}