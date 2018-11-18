using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KnapsackSdk.Dtos;

namespace KnapsackProblem
{
    public interface IFileManager
    {
        IEnumerable<DefinitionDto> GetDefinitions(string path);
        IEnumerable<ResultDto> GetResults(string path);
        void AppendResult(string path, params string[] parameters);
    }

    public class FileManager : IFileManager
    {

        public IEnumerable<ResultDto> GetResults(string path)
        {
            using (var file = new StreamReader(path))
            {
                string line;

                while ((line = file.ReadLine()) != null)
                {
                    var result = GetResult(line);
                    yield return result;
                }

                file.Close();
            }
        }

        public void AppendResult(string path, params string[] parameters)
        {
            var value = string.Join("\t", parameters);
            File.AppendAllText(path, $"{value}{Environment.NewLine}");
        }

        private static ResultDto GetResult(string text)
        {
            var parsedValues = text.Split(' ').Where(value => !string.IsNullOrEmpty(value)).Select(Int32.Parse).ToList();
            var items = new List<bool>();
            for (int index = 0; index < parsedValues[1]; index++)
            {
                items.Add(parsedValues[3 + index] == 1);
            }
            return new ResultDto(parsedValues[0], parsedValues[2], items);
        }

        public IEnumerable<DefinitionDto> GetDefinitions(string path)
        {
            using (var file = new StreamReader(path))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    var definition = GetDefinition(line);
                    yield return definition;
                }
                file.Close();
            }
        }

        private static DefinitionDto GetDefinition(string text)
        {
            var parsedValues = text.Split(' ').Select(Int32.Parse).ToList();
            var items = new List<ItemDto>();
            for (int index = 0; index < parsedValues[1]; index++)
            {
                var item = new ItemDto(parsedValues[3 + index * 2], parsedValues[4 + index * 2]);
                items.Add(item);
            }
            return new DefinitionDto(parsedValues[0], parsedValues[2], items);
        }
    }
}