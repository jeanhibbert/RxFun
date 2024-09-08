<Query Kind="Statements">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

Task<int> i = Task.FromResult (1), j = Task.FromResult (2), k = Task.FromResult (3);

( await i + await j + await k ).Dump();

// C# 5 made Task<T> fluent