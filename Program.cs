using LabWork_Classes;
using System;
using System.IO;
using System.Collections.Generic;

namespace LabWork_CollectionsGenerics
{
    class Program
    {
        static void Main(string[] args)
        {
            CustomLinkedList<Hexagon> list1 = new CustomLinkedList<Hexagon>();
            CustomLinkedList<Hexagon> list2 = new CustomLinkedList<Hexagon>();
            Node<Hexagon> node1_list1 = new Node<Hexagon>(new Hexagon(1));
            Node<Hexagon> node2_list1 = new Node<Hexagon>(new Hexagon(2));
            Node<Hexagon> node3_list1 = new Node<Hexagon>(new Hexagon(3));
            Node<Hexagon> node4_list1 = new Node<Hexagon>(new Hexagon(4));
            Node<Hexagon> node5_list1 = new Node<Hexagon>(new Hexagon(5));
            list1.Add(node1_list1);
            list1.Add(node2_list1);
            list1.Add(node3_list1);
            list1.Add(new Hexagon(22));
            list1.AddBefore(node3_list1, node4_list1);
            list1.AddFirst(node5_list1);

            list1.SaveToFile();
            using (StreamReader sr = File.OpenText("Linked List.io"))
            {
                Console.Write(sr.ReadToEnd());
            }
            Console.WriteLine("\n");
            list1.BubbleSort();
            list1.SaveToFile();
            using (StreamReader sr = File.OpenText("Linked List.io"))
            {
                Console.Write(sr.ReadToEnd());
            }
            Console.ReadLine();
        }
    }
}
