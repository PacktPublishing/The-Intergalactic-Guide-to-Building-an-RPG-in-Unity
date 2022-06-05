using UnityEngine;

using System;
using System.Collections.Generic;
//using System.Text;

public class ConsoleController {
    
    const int retainedLogLinesCount = 25;  

    Queue<string> retainedLogLines = new Queue<string>(retainedLogLinesCount);
    List<string> commandHistory = new List<string>();

    Dictionary<string, ConsoleCommand> commands = new Dictionary<string, ConsoleCommand>();


    // retainedLogLines as an array for easier use by ConsoleView class...
    public string[] log { get; private set; }

    // The name of the repeat command
    const string repeatCommandName = "!!";

    public ConsoleController() {
       // Debug.Log("ConsoleController()");
        ConsoleCommand.Register(repeatCommandName, RepeatCommand, "Repeat last command.");  // !!!
        ConsoleCommand.Register("help", Help, "Print this help.");
        ConsoleCommand.Register("invade", Invade, "Send in the fleet!");
    }




    string[] ParseArguments(string commandString) {
        LinkedList<char> commandChars = new LinkedList<char>(commandString.ToCharArray());
        bool inQuote = false;
        var node = commandChars.First;
        while (node != null) {
            var next = node.Next;
            if (node.Value == '"') {
                inQuote = !inQuote;
                commandChars.Remove(node);
            }
            if (!inQuote && node.Value == ' ') {
                node.Value = '\n';
            }
            node = next;
        }
        char[] parsedCharsArray = new char[commandChars.Count];
        commandChars.CopyTo(parsedCharsArray, 0);
        var str = new string(parsedCharsArray);
        var delimiter = new char[] { '\n' };
        return str.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
    }

    public delegate void LogChangedHandler(string[] log);
    public event LogChangedHandler logChanged;

    public delegate void VisibilityChangedHandler(bool visible);
    public event VisibilityChangedHandler visibilityChanged;

    public void AppendLogLine(string line) {
        Debug.Log(line);

        if (retainedLogLines.Count >= ConsoleController.retainedLogLinesCount) {
            retainedLogLines.Dequeue();
        }
        retainedLogLines.Enqueue(line);

        log = retainedLogLines.ToArray();
        if (logChanged != null) {
            logChanged(log);
        }
    }

    

    
    public void RunCommandString(string commandString) {
        AppendLogLine("$ " + commandString);

        string[] commandSplit = ParseArguments(commandString);
        string[] args = new string[0];
        if (commandSplit.Length < 1) {
            AppendLogLine(string.Format("Unable to process command '{0}'", commandString));
            return;
        } else if (commandSplit.Length >= 2) {
            int numArgs = commandSplit.Length - 1;
            args = new string[numArgs];
            Array.Copy(commandSplit, 1, args, 0, numArgs);
        }
        RunCommand(commandSplit[0].ToLower(), args);
        commandHistory.Add(commandString);
    }

    public void RunCommand(string commandName, string[] args)
    { // #1
        ConsoleCommand cmd = null; // #2                                     !!!
        var hasCommand = ConsoleCommand.commands.TryGetValue(commandName, out cmd); //#3     !!!
        if (!hasCommand)
        { // #4
            AppendLogLine($"Unknown command '{commandName}', type 'help' for list.");  // #5
        }
        else
        {
            if (cmd.handler == null)
            { // #6
                AppendLogLine($"Unable to process command '{commandName}', handler was null."); // #7
            }
            else
            {
                cmd.handler(args); // #8
            }
        }
    }

    void RepeatCommand(string[] args) {
        for (int cmdIdx = commandHistory.Count - 1; cmdIdx >= 0; --cmdIdx) {
            string cmd = commandHistory[cmdIdx];
            if (String.Equals(repeatCommandName, cmd)) {
                continue;
            }
            RunCommandString(cmd);
            break;
        }
    }

    void Help(string[] args) {
        foreach (ConsoleCommand cmd in ConsoleCommand.commands.Values) {  // !!        
            AppendLogLine($"  {cmd.name}: {cmd.help}");
        }
    }

    void Invade(string[] args)
    {
        AppendLogLine("All your base are belong to us!");
    }


} // end ConsoleController

/*
RegisterCommand("resetprefs", resetPrefs, "Reset & saves PlayerPrefs.");

RegisterCommand("babble", babble, "Example command that demonstrates how to parse arguments. babble [word] [# of times to repeat]");
RegisterCommand("echo", echo, "echoes arguments back as array (for testing argument parser)");

RegisterCommand("hide", hide, "Hide the console.");
*/