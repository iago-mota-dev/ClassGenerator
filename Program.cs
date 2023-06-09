﻿using CommandLine;
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

            //Console.WriteLine("How many properties this class will have?");
            //numberOfProperties = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("What you want to do with the class?");

            Console.WriteLine(@"
1 - Add property |
3 - Exit         |");

            string name;
            string type;
            string bsRl;
            string ex;
            string awnsersRules = "";
            string awnsers = Console.ReadLine();
            switch (awnsers)
            {
                case "1":
                    Console.WriteLine("What is the name of the property?");
                    name = Console.ReadLine();

                    Console.WriteLine("What is the type of the property?");
                    type = Console.ReadLine();

                    Console.WriteLine("This property has bs? S/N");
                    bsRl = Console.ReadLine();

                    propertieDictionary.Add(name, type);

                    if (bsRl == "S")
                    {
                        
                        Console.WriteLine("What you rule?");

                        Console.WriteLine(@"
1 - Length           |
3 - Required  |");
                         awnsersRules = Console.ReadLine();

                        switch (awnsersRules)
                        {
                            case "1":
                                Console.WriteLine("Type the maximum length number: ");
                                string maximumLength = Console.ReadLine();
                                awnsersRules = @$"  if(property.Length > {maximumLength}){{
                    throw new Exception({name});
                }}";
                                break;
                            default:
                                awnsersRules = @$"  if(property is null){{
                    throw new ArgumentException({name});
                }}";
                                break;
                        }
                    }

                    break;
                default:
                    return;
                  
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
        public virtual int Id {{get; protected set;}}
        {GenerateProperties(propertieDictionary)}

        public {className}({GenerateConstructorParams(propertieDictionary)})
        {{
            {CallSetsInConstructor(propertieDictionary)}
        }}

        {GenerateSets(propertieDictionary, bsRl, awnsersRules)}
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
            string prop = @$"
            public virtual {dic.Value} {dic.Key} {{get; set;}}";
            property.Add(prop);
        }

        return string.Join(Environment.NewLine, property);
    }
    public string GenerateSets(Dictionary<string, string> dict, string bsRl,string awnsersRules)
    {
        IList<string> property = new List<string>();

        foreach (var dic in dict)
        {
            string prop;
            if (bsRl == "S")
            {
                 prop = @$"
            public virtual void Set{dic.Key}({dic.Value} property)
            {{
                {awnsersRules}

                this.{dic.Key} = property;
            }}";
            }
            else
            {
                 prop = @$"
            public virtual void Set{dic.Key}({dic.Value} property)
            {{
                this.{dic.Key} = property;
            }}";
            }
           
            property.Add(prop);
        }
        return string.Join(Environment.NewLine, property);
    }
    public string GenerateConstructorParams(Dictionary<string, string> dict)
    {
        IList<string> property = new List<string>();

        foreach (var dic in dict)
        {
            if(dict.Last().Key == dic.Key)
            {
                string prop = @$" {dic.Value} {dic.Key}";
                property.Add(prop);
            }
            else
            {
                string prop = @$" {dic.Value} {dic.Key},";
                property.Add(prop);
            }
           
        }
        return string.Join("", property);
    }
    public string CallSetsInConstructor(Dictionary<string, string> dict)
    {
        IList<string> property = new List<string>();

        foreach (var dic in dict)
        {
                string prop = @$"   Set{dic.Key}({dic.Key});";
                property.Add(prop);
        }
        return string.Join(Environment.NewLine, property);
    }
}
