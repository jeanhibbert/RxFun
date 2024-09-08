<Query Kind="Expression">
  <Reference>&lt;RuntimeDirectory&gt;\System.Runtime.dll</Reference>
  <NuGetReference>Rx-Main</NuGetReference>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

// Filter sequence, keeping only even numbers

Observable
	.Interval(TimeSpan.FromSeconds(1))
	.Where(i => i % 2 == 0)