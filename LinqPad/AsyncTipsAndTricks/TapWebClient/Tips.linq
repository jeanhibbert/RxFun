<Query Kind="Expression" />

/* TIPS:

* Scope cancellation tokens and progress reporting to methods, not types
* Assume all TCP methods in the CLR are semi-asynchronous
* Task.Run will efficiently convert a semi-asynchronous method into a fully asynchronous method
* Use .ConfigureAwait(false) on all library methods
* But don't assume .ConfigureAwait(false) will get you off the sync context

*/