<Query Kind="Statements" />

int[] numbers = { 1, 2, 3 };

numbers.Sum().Dump();

numbers.Where (n => n > 1)
       .Select (n => n * n)
       .Dump();
	   
(from n in numbers
 where n > 1
 select n * n).Dump();

// C# 3 made IEnumerable<T> fluent