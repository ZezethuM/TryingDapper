using Microsoft.Data.Sqlite;
using Dapper;
using TryingDapper;

using (var connection = new SqliteConnection("Data Source=./trying_dapper.db;")) {
    connection.Open();

string CREATE_PIZZA_TABLE = @"create table if not exists pizza (
	id integer primary key AUTOINCREMENT,
	type text,
	size text,
	price real
);" ;

connection.Execute(CREATE_PIZZA_TABLE);
connection.Execute(@"
	insert into 
		pizza (type, size, price)
	values 
		(@Type, @Size, @Price);",
	new Object[] {
		new Pizza() {
		Type = "Regina",
		Size = "small",
		Price = 31.75
	}, new Pizza {
		Type = "Regina",
		Size = "medium",
		Price = 51.75
	}, new Pizza {
		Type = "Regina",
		Size = "large",
		Price = 89.75
	}
});
var pizzas = connection.Query<Pizza>(@"select * from pizza");
Console.WriteLine(pizzas.Count());
var GROUP_BY_SIZE = @"select size as grouping, sum(price) as total from pizza group by size";

var pizzaTotalPerSize = connection.Query<PizzaGrouped>(GROUP_BY_SIZE);

foreach (var pizza in pizzaTotalPerSize)
{
	Console.WriteLine($"size : {pizza.Grouping} - total @ {pizza.Total} ");
}
}