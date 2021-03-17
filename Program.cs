using System;
using System.IO;
using System.Linq;

namespace LabWork_CollectionsGenerics
{
    class Program
    {
        static void Main(string[] args)
        {
            string separator = new string('_', 64);
            //Создаем коллекцию с несколькими элементами
            CustomLinkedList<Hexagon> list1 = new CustomLinkedList<Hexagon>(new Hexagon(15), new Hexagon(7));
            //Создаем элементы для коллекции
            Hexagon hex1 = new Hexagon(8);
            Hexagon hex2 = new Hexagon(4);
            Hexagon hex3 = new Hexagon(9);
            Hexagon hex4 = new Hexagon(20);
            Hexagon hex5 = new Hexagon(5);

            //Привязываем элементы в коллекцию
            list1.Add(hex1);
            list1.Add(hex2);
            list1.Add(new Hexagon(10));
            //Привязываем к коллекции и держим информацию о конкретном звене
            Node<Hexagon> node = list1.Add(hex3);
            //Привязываем hex4 к коллекции и добавляем перед звеном node
            list1.AddBefore(node, hex4);
            //Привязываем hex5 к коллекции, и делаем его первым звеном
            list1.AddFirst(hex5);
            //Сортировка коллекции
            Console.WriteLine("Коллекция до сортировки:");
            list1.DisplayToConsole();
            Console.WriteLine(separator);
            Console.WriteLine("Коллекция после сортировки внутренним методом:");
            list1.BubbleSort();
            list1.DisplayToConsole();
            Console.WriteLine(separator);
            //Теперь отсортируем вложенные элементы коллекции, используя LINQ
            Console.WriteLine("Элементы коллекции, отсортированные по площади фигур используя LINQ:");
            var hexagons = from hex in list1 orderby hex.Area descending select hex;
            int index = 0;
            foreach (var hex in hexagons)
            {
                Console.WriteLine($"Hex {index}: {hex}");
                index++;
            }
            Console.WriteLine(separator);

            //Сохраним коллекцию в файл
            Console.WriteLine("Коллекция сохранена в файл");
            list1.SaveToFile();
            Console.WriteLine(separator);
            //Выведем полученную информацию прямиком из файла
            Console.WriteLine("Вывод из полученного файла:");
            using (StreamReader sr = File.OpenText("Linked List.io"))
            {
                Console.Write(sr.ReadToEnd());
            }
            Console.WriteLine(separator);

            //Создаем несколько коллекций, добавляем их и первую в один массив
            CustomLinkedList<Hexagon> list2 = new CustomLinkedList<Hexagon>(new Hexagon(10), new Hexagon(99), new Hexagon(1));
            CustomLinkedList<Hexagon> list3 = new CustomLinkedList<Hexagon>(new Hexagon(2), new Hexagon(2), new Hexagon(2), new Hexagon(2),
                                                                            new Hexagon(2), new Hexagon(2), new Hexagon(2), new Hexagon(2),
                                                                            new Hexagon(2), new Hexagon(2), new Hexagon(2), new Hexagon(2),
                                                                            new Hexagon(2), new Hexagon(2), new Hexagon(2), new Hexagon(2));
            CustomLinkedList<Hexagon> list4 = new CustomLinkedList<Hexagon>(new Hexagon(7));
            CustomLinkedList<Hexagon>[] listArray = new[] { list1, list2, list3, list4 };

            //Сделаем LINQ запросы по массиву
            Console.WriteLine($"\n\n{' ', 32}LINQ Запросы:");
            //Запрос 1: найти количество коллекций, содержащих заданное значение (периметр > 69)
            var applicableListCount = (from list in listArray
                                       from item in list
                                       where item.Perimeter > 69
                                       select list).Distinct().Count();
            Console.WriteLine($"Количество коллекций, содержащих фигуры, периметр которых > 69: {applicableListCount}");
            Console.WriteLine(separator);

            //Запрос 2: найти минимальную коллекцию и максимальную коллекции (выберем по количеству элементов)
            var lists = from list in listArray orderby list.Count ascending select list;
            Console.WriteLine($"Минимальная коллекция по количеству элементов:\n{lists.First()}");
            Console.WriteLine(separator);
            Console.WriteLine($"Максимальная коллекция по количеству элементов:\n{lists.Last()}");
            Console.WriteLine(separator);

            Console.ReadLine();
        }
    }
}
