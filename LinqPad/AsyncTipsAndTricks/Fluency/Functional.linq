<Query Kind="Statements" />

// FUNCTIONAL

new Order (customer,
	new OrderLine (6, "TAL"),
	new OrderLine (6, "HPK") { Skippable = true },
	new OrderLine (6, "LGV"))
	{
		Rush = true
	};

// Not Fowler-style fluency, but achieves similar goals.