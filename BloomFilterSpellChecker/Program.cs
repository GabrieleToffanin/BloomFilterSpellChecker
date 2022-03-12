using BenchmarkDotNet.Running;
using BloomFilterSpellChecker;
using System.Text;

//int capacity = 2000000;
//var filter = new Filter<string>(capacity);

//filter.add("Qualcosa");

//List<string> wordList = new List<string>()
//{
//    "Ciao", "Gabriele", "Ti", "Saluta"
//};

//List<string> wordListToCheck = new List<string>()
//{
//    "Ciao", "Albero", "Saluta"
//};

//foreach(var item in wordList) filter.add(item);

//foreach(string item in wordListToCheck)
//{
//    Console.WriteLine($"The word {item} is contained in the collection ? {filter.Contains(item)}");
//}

var summary = BenchmarkRunner.Run<BenckMarkSpeed>();

