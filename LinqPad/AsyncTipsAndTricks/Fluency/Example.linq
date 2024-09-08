<Query Kind="Statements" />

try
{
	await ctx.Model.RegisterCustomer (model);
	return Result (ctx.User.Id);
}
catch (ServerValidationException ex)
{
	return JsonValidationFailure (ex.Errors);
}