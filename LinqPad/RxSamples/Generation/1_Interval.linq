<Query Kind="Expression">
  <Reference>&lt;RuntimeDirectory&gt;\System.Runtime.dll</Reference>
  <NuGetReference>Rx-Main</NuGetReference>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

// Simple observable sequence that generates a new event every second

Observable.Interval(TimeSpan.FromSeconds(1))