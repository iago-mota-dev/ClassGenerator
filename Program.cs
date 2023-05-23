using CommandLine;
using System.Text;

Parser.Default.ParseArguments<Options>(args)
                   .WithParsed<Options>(o =>
                   {
                       var generator = new ClassGenerator();
                       generator.GenerateClass(o.OutputClass);
                   });



public class Options
{
    [Option('c', "class", Required = false, HelpText = "Specify the class name.")]
    public string OutputClass { get; set; }
}

public class ClassGenerator
{
    public void GenerateClass(string className)
    {
        string path = Directory.GetCurrentDirectory();
        string entityPath = @$"{path}\{className}\Entities\";
        string filePath = @$"{entityPath}{className}.cs";
        string pathToDelete = @$"{path}\{className}\";
        try
        {
            if (Directory.Exists(entityPath))
            {
                Console.WriteLine("There is a folder and a file with that same path, you want to delete it?");
                Console.WriteLine("1 - Yes, 2 - No");
                var awnser = Console.ReadLine();
                if (awnser == "1")
                {
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                    Directory.Delete(pathToDelete + "Entities");
                    Directory.Delete(pathToDelete);
                    Console.WriteLine("The directory was deleted successfully.");
                }
                else
                {
                    Console.WriteLine("Exiting the program...");
                    Console.WriteLine("Goodbye");
                    return;
                }

            }

            DirectoryInfo di = Directory.CreateDirectory(entityPath);
            Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(path));


            if (!File.Exists(filePath))
            { 
                File.WriteAllText(filePath, String.Empty);
            }
    
        }
        catch (Exception Ex)
        {
            Console.WriteLine(Ex.ToString());
        }
    }
}
