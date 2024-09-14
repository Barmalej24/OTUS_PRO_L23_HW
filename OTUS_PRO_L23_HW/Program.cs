using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace OTUS_PRO_L23_HW
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var path = "SecretPath";
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var task1 = GetCountSpacesTextAsync($"{path}\\file1.txt");
            var task2 = GetCountSpacesTextAsync($"{path}\\file2.txt");
            var task3 = GetCountSpacesTextAsync($"{path}\\file3.txt");

            await Task.WhenAll(task1, task2, task3);

            stopwatch.Stop();

            Console.WriteLine($"Количество пробелов в file1.txt: {task1.Result}");
            Console.WriteLine($"Количество пробелов в file2.txt: {task2.Result}");
            Console.WriteLine($"Количество пробелов в file3.txt: {task3.Result}");
            Console.WriteLine($"Время выполнения: {stopwatch.ElapsedMilliseconds} мс");


            stopwatch.Restart();

            var task4 = CountSpacesInFolderAsync("SecretPath");
            await task4;

            stopwatch.Stop();

            Console.WriteLine($"Количество пробелов во всех файлах в папке {path}: {task4.Result}");
            Console.WriteLine($"Время выполнения: {stopwatch.ElapsedMilliseconds} мс");
        }

        /// <summary>
        /// Подсчет количества пробелов в файле
        /// </summary>
        /// <param name="filePath">Путь к папке</param>
        /// <returns></returns>
        static async Task<int> GetCountSpacesTextAsync(string filePath)
        {
            var content = await File.ReadAllTextAsync(filePath);

            return content.Split(' ').Length - 1;
        }

        /// <summary>
        /// Функция, которая принимает путь к папке и возвращает общее количество пробелов во всех файлах в ней
        /// </summary>
        /// <param name="folderPath">Путь к папке</param>
        /// <returns></returns>
        static async Task<int> CountSpacesInFolderAsync(string folderPath)
        {
            var files = Directory.GetFiles(folderPath);
            var tasks = files.Select(file => GetCountSpacesTextAsync(file)).ToArray();

            await Task.WhenAll(tasks);

            return tasks.Sum(t => t.Result);
        }
    }
}
