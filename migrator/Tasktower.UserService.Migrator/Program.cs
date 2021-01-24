using CommandLine;
using DbUp;
using System;
using System.Linq;
using System.Reflection;

/*
This is a CLI application for running database migrations, please pass the --help option in
the program arguments for further details
*/

namespace Tasktower.UserService.Migrator
{
    class Program
    {
        public class Options
        {
            [Option('c', "connectionstring", Required = true,
                HelpText = "SQL Server connection string, must be in the format:" +
                    "\"Host = hostname; User Id = username; Password = userpassword; Database = sbname; Port = 5432\"")]
            public string ConnectionString { get; set; }

            [Option('d', "data", Required = false, Default = false, 
                HelpText = "Choose whether to include data in migrations")]
            public bool AddData { get; set; }
        }

        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunOptions);
        }

        static void RunOptions(Options opts)
        {
            var addData = opts.AddData;
            var connectionString = opts.ConnectionString;


            var upgraderBuilder = DeployChanges.To
                .SqlDatabase(connectionString)
                .WithScriptsFromFileSystem("./Scripts/Migrations");
            if (addData)
            {
                upgraderBuilder.WithScriptsFromFileSystem("./Scripts/Data");
            }

            var upgrader = upgraderBuilder.LogToConsole().Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
#if DEBUG
                Console.ReadLine();
#endif
                System.Environment.Exit(-1);
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();
        }
    }
}
