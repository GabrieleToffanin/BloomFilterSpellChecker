using BloomFilterSpellChecker;
using System.Text;

int capacity = 2000000;
var filter = new Filter<string>(capacity);

filter.add("dioCamaja");

Console.WriteLine(filter.Contains("content"));