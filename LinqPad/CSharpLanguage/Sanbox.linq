<Query Kind="Program" />

class BaseClass
{
	public virtual void PrintMessage()
	{
		Console.WriteLine("Message from the base class");
	}
}

class DerivedClass : BaseClass
{
	public new void PrintMessage()
	{
		Console.WriteLine("Message from the derived class");
	}
}

class Program
{
	static void Main()
	{
		BaseClass baseObject = new BaseClass();
		BaseClass derivedObject = new DerivedClass();

		baseObject.PrintMessage();      // "Message from the base class"
		derivedObject.PrintMessage();   // "Message from the base class" (hides the method in the derived class)
	}
}
