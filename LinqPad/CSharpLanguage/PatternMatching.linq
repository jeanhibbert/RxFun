<Query Kind="Program">
  <RuntimeVersion>7.0</RuntimeVersion>
</Query>

void Main()
{
	bool isVerified = true;
	
	string result;
	if (isVerified)
	{
		result = "Verified";
	}
	else
	{
		result = "Not Verified";
	}
	
	result.Dump();


	result = isVerified switch
	{
		true => "Verified",
		false => "Not Verified",
	};
	result.Dump();

}

// You can define other methods, fields, classes and namespaces here