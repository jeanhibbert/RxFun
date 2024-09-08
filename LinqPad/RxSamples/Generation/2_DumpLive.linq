<Query Kind="Statements">
  <Reference>&lt;RuntimeDirectory&gt;\System.Runtime.dll</Reference>
  <NuGetReference>Rx-Main</NuGetReference>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

// Simple observable sequence that generates a new event every second, output using the Live Observables tab

var interval = Observable.Interval(TimeSpan.FromSeconds(1));

interval.DumpLive();