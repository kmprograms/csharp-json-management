using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace CSharpJsonManagement
{
    internal interface IJsonConverter<T>
    {
        private static JsonSerializerOptions options = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        
        public static string ToJson(T data)
        {
            if (data == null)
            {
                throw new ArgumentException("to json - data object is null");
            }
            
            return JsonSerializer.Serialize(data, options);
        }

        public static void ToJsonFile(T data, string filename)
        {
            if (data == null)
            {
                throw new ArgumentException("to json file - data object is null");
            }
            
            if (filename == null)
            {
                throw new ArgumentException("to json file - filename string is null");
            }

            var json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(filename, json);
        }

        public static T FromJson(string json)
        {
            if (json == null)
            {
                throw new ArgumentException("from json - json string is null");
            }
            return JsonSerializer.Deserialize<T>(json, options);
        }

        public static T FromJsonFile(string filename)
        {
            if (filename == null)
            {
                throw new ArgumentException("from json file - filename string is null");
            }

            var jsonFromFile = File.ReadAllText(filename);
            return JsonSerializer.Deserialize<T>(jsonFromFile, options);
        }
    }

    internal class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public double Weight { get; set; }

        public override string ToString()
        {
            return $"{Name} {Age} {Weight}";
        }
    }

    internal static class Program
    {
        private static void Main(string[] args)
        {
            var people = new List<Person>()
            {
                new Person()
                {
                    Name = "ADAM",
                    Age = 10,
                    Weight = 55.6
                },
                new Person()
                {
                    Name = "ELA",
                    Age = 20,
                    Weight = 57.6
                }
            };

            var jsonPeople = IJsonConverter<List<Person>>.ToJson(people);
            Console.WriteLine(jsonPeople);

            var peopleFromJson = IJsonConverter<List<Person>>.FromJson(jsonPeople);
            peopleFromJson.ForEach(Console.WriteLine);

            var jsonFilename = "people.json";
            IJsonConverter<List<Person>>.ToJsonFile(people, jsonFilename);

            var peopleFromJsonFile = IJsonConverter<List<Person>>.FromJsonFile(jsonFilename);
            peopleFromJsonFile.ForEach(Console.WriteLine);
        }
    }
}