using System.Collections.Generic;


public delegate void CommandHandler(string[] args);

public class ConsoleCommand
{
    public static Dictionary<string, ConsoleCommand> commands = new Dictionary<string, ConsoleCommand>();

    public string name { get; private set; }
    public CommandHandler handler { get; private set; }
    public string help { get; private set; }

    public ConsoleCommand(string name, CommandHandler handler, string help) {
        this.name = name;
        this.handler = handler;
        this.help = help;
    }

    //When adding commands, you must add a call below to Register() with its name, implementation method, and help text.
    public static void Register(string commandName, CommandHandler handler, string help) {
        var cmd = new ConsoleCommand(commandName, handler, help); // #2
        commands.Add(commandName, cmd); // #3
    }

}  // end ConsoleCommand

