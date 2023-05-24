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

            int numberOfProperties = 1;
            Dictionary<string, string> propertieDictionary = new Dictionary<string, string>();

            Console.WriteLine("How many properties this class will have?");
            numberOfProperties = Convert.ToInt32(Console.ReadLine());

            int i = 0;
            for (i = 0; i < numberOfProperties; i++)
            {
                string name;
                string type;
                Console.WriteLine("What is the name of the property?");
                name = Console.ReadLine();
                Console.WriteLine("What is the type of the property?");
                type = Console.ReadLine();

                propertieDictionary.Add(name, type);
            }

            string classContent = $@"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace {className}s.Entities
{{
    public class {className}
    {{
        {GenerateProperties(propertieDictionary)}
    }}
}}";

            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, classContent);
            }

        }
        catch (Exception Ex)
        {
            Console.WriteLine(Ex.ToString());
        }
    }
    public string GenerateProperties(Dictionary<string, string> dict)
    {
        IList<string> property = new List<string>();

        foreach (var dic in dict)
        {
            string prop = @$"public virtual {dic.Value} {dic.Key} {{get; set;}}";
            property.Add(prop);
        }

        return string.Join(Environment.NewLine, property);
    }
}
