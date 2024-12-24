using System;
using System.Collections.Generic;
using System.IO;

namespace DictionaryApp
{
    class Program
    {
        static void Main()
        {
            DictionaryManager manager = new DictionaryManager();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("1. Создать словарь\n2. Добавить слово\n3. Заменить слово/перевод\n4. Удалить слово/перевод\n5. Найти перевод слова\n6. Экспортировать слово\n7. Выйти");
                Console.Write("Выберите пункт: ");
                switch (Console.ReadLine())
                {
                    case "1":
                        manager.CreateDictionary();
                        break;
                    case "2":
                        manager.AddWord();
                        break;
                    case "3":
                        manager.ReplaceWordOrTranslation();
                        break;
                    case "4":
                        manager.DeleteWordOrTranslation();
                        break;
                    case "5":
                        manager.FindTranslation();
                        break;
                    case "6":
                        manager.ExportWord();
                        break;
                    case "7":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Нажмите любую клавишу для продолжения.");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }

    public class DictionaryManager
    {
        private readonly Dictionary<string, Dictionary<string, List<string>>> dictionaries = new();

        public void CreateDictionary()
        {
            Console.Write("Введите тип словаря (например, англо-русский): ");
            string type = Console.ReadLine();
            if (dictionaries.ContainsKey(type))
            {
                Console.WriteLine("Словарь уже существует.");
            }
            else
            {
                dictionaries[type] = new Dictionary<string, List<string>>();
                Console.WriteLine("Словарь создан.");
            }
            Pause();
        }

        public void AddWord()
        {
            string type = SelectDictionary();
            if (type == null) return;

            Console.Write("Введите слово: ");
            string word = Console.ReadLine();
            Console.Write("Введите перевод: ");
            string translation = Console.ReadLine();

            if (!dictionaries[type].ContainsKey(word))
            {
                dictionaries[type][word] = new List<string>();
            }

            dictionaries[type][word].Add(translation);
            Console.WriteLine("Перевод добавлен.");
            Pause();
        }

        public void ReplaceWordOrTranslation()
        {
            string type = SelectDictionary();
            if (type == null) return;

            Console.Write("Введите слово: ");
            string word = Console.ReadLine();

            if (!dictionaries[type].ContainsKey(word))
            {
                Console.WriteLine("Слово не найдено.");
            }
            else
            {
                Console.WriteLine("1. Заменить слово\n2. Заменить перевод");
                Console.Write("Выберите пункт: ");
                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    Console.Write("Введите новое слово: ");
                    string newWord = Console.ReadLine();
                    dictionaries[type][newWord] = dictionaries[type][word];
                    dictionaries[type].Remove(word);
                    Console.WriteLine("Слово заменено.");
                }
                else if (choice == "2")
                {
                    Console.Write("Введите перевод для замены: ");
                    string oldTranslation = Console.ReadLine();

                    if (dictionaries[type][word].Remove(oldTranslation))
                    {
                        Console.Write("Введите новый перевод: ");
                        string newTranslation = Console.ReadLine();
                        dictionaries[type][word].Add(newTranslation);
                        Console.WriteLine("Перевод заменен.");
                    }
                    else
                    {
                        Console.WriteLine("Перевод не найден.");
                    }
                }
                else
                {
                    Console.WriteLine("Неверный выбор.");
                }
            }
            Pause();
        }

        public void DeleteWordOrTranslation()
        {
            string type = SelectDictionary();
            if (type == null) return;

            Console.Write("Введите слово: ");
            string word = Console.ReadLine();

            if (!dictionaries[type].ContainsKey(word))
            {
                Console.WriteLine("Слово не найдено.");
            }
            else
            {
                Console.WriteLine("1. Удалить слово\n2. Удалить перевод");
                Console.Write("Выберите пункт: ");
                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    dictionaries[type].Remove(word);
                    Console.WriteLine("Слово удалено.");
                }
                else if (choice == "2")
                {
                    Console.Write("Введите перевод для удаления: ");
                    string translation = Console.ReadLine();

                    if (dictionaries[type][word].Count > 1 && dictionaries[type][word].Remove(translation))
                    {
                        Console.WriteLine("Перевод удален.");
                    }
                    else
                    {
                        Console.WriteLine("Нельзя удалить последний перевод или перевод не найден.");
                    }
                }
                else
                {
                    Console.WriteLine("Неверный выбор.");
                }
            }
            Pause();
        }

        public void FindTranslation()
        {
            string type = SelectDictionary();
            if (type == null) return;

            Console.Write("Введите слово: ");
            string word = Console.ReadLine();

            if (dictionaries[type].TryGetValue(word, out var translations))
            {
                Console.WriteLine("Переводы: " + string.Join(", ", translations));
            }
            else
            {
                Console.WriteLine("Слово не найдено.");
            }
            Pause();
        }

        public void ExportWord()
        {
            string type = SelectDictionary();
            if (type == null) return;

            Console.Write("Введите слово для экспорта: ");
            string word = Console.ReadLine();

            if (dictionaries[type].TryGetValue(word, out var translations))
            {
                string fileName = word + ".txt";
                File.WriteAllText(fileName, word + " - " + string.Join(", ", translations));
                Console.WriteLine($"Слово экспортировано в файл {fileName}.");
            }
            else
            {
                Console.WriteLine("Слово не найдено.");
            }
            Pause();
        }

        private string SelectDictionary()
        {
            if (dictionaries.Count == 0)
            {
                Console.WriteLine("Нет доступных словарей.");
                Pause();
                return null;
            }

            Console.WriteLine("Доступные словари:");
            foreach (var ttype in dictionaries.Keys)
            {
                Console.WriteLine("- " + ttype);
            }
            Console.Write("Выберите словарь: ");
            string type = Console.ReadLine();

            if (!dictionaries.ContainsKey(type))
            {
                Console.WriteLine("Словарь не найден.");
                Pause();
                return null;
            }

            return type;
        }

        private void Pause()
        {
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }
}
