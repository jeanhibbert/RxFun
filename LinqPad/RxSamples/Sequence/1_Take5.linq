<Query Kind="Expression">
  <Reference>&lt;RuntimeDirectory&gt;\System.Runtime.dll</Reference>
  <NuGetReference>Rx-Main</NuGetReference>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

// Truncate observable sequence after receiving 5 events 

Observable
	.Interval(TimeSpan.FromSeconds(1))
	.Take(5)