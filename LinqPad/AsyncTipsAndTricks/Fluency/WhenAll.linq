<Query Kind="Statements">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

Task<int> i = Task.FromResult (1), j = Task.FromResult (2), k = Task.FromResult (3);

Task.WhenAll (i, j, k).Dump();

// i.AndWhen (j).AndWhen (k).Dump();