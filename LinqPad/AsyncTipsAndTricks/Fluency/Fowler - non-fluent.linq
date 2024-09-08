<Query Kind="Statements" />

// NON-FLUENT (Martin Fowler)

Order o1 = new Order();
customer.addOrder(o1);
OrderLine line1 = new OrderLine(6, Product.find("TAL"));
o1.addLine(line1);
OrderLine line2 = new OrderLine(5, Product.find("HPK"));
o1.addLine(line2);
OrderLine line3 = new OrderLine(3, Product.find("LGV"));
o1.addLine(line3);
line2.setSkippable(true);
o1.setRush(true);