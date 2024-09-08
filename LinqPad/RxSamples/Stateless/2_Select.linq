<Query Kind="Expression">
  <Reference>&lt;RuntimeDirectory&gt;\System.Runtime.dll</Reference>
  <NuGetReference>Rx-Main</NuGetReference>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

// Create a new observable sequence by mapping each element to its value multiplied by 10

Observable
	.Interval(TimeSpan.FromSeconds(1))
	.Select(i => i * 10)