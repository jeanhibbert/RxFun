<Query Kind="Expression">
  <Reference>&lt;RuntimeDirectory&gt;\System.Runtime.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Threading.Tasks.dll</Reference>
  <NuGetReference>Rx-Main</NuGetReference>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

// Example using LINQ query syntax:
// Group events into a window of 10 events each
// Resulting sequence generates an event every second

from w in Observable.Interval(TimeSpan.FromMilliseconds(100))
	.Window(10)
from c in w.Count()
select c

