<Query Kind="Expression">
  <Reference>&lt;RuntimeDirectory&gt;\System.Runtime.dll</Reference>
  <NuGetReference>Rx-Main</NuGetReference>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

// Skip the first 3 events, then stop sequence after 5 events 

Observable
	.Interval(TimeSpan.FromSeconds(1))
	.Skip(3)
	.Take(5)