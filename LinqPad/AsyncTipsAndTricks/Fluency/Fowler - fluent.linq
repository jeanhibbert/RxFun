<Query Kind="Statements" />

// FLUENT (Martin Fowler)

customer.newOrder()
        .with(6, "TAL")
        .with(5, "HPK").skippable()
        .with(3, "LGV")
        .priorityRush();