/*

This application applies OOP concepts such as inhertence and polymorphism

--------------------------------------------------------------------------------
inheretance

General Manager inherts from Warehouse Leader
Warehouse Leader inherts from Associate
Associate Inhert from Employee class.

When General Manager Inhert from Warehouse leader, it inherts all Warehouse leader functions and add more to them.

General Manager can do everything that Warehouse leader, Associate,and Employee can do
Warehouse leader can do everything that Associate,and Employee can do
Associate can do everything that Employee can do.


-----------------------------------------------------------------------------

Polymorphism
Employee LoggedInEmployee = new General Manager();

in our application it doesn't happen this way.
First employee object is created by retreiving employee from database that matched username and password
Employee object data will be used to create General Manager object and LoggedInEmployee is pointing to it.

LoggedInEmployee can be either General Manager, WarehouseLeader or Associate object depending on Employee.RoleID


if you try to call a function that is exclusive to General Manager using Associate object, application will throw exception.

This will be helpful in development, but the final product will not be throwing exception, because if associate, for example, 
is on the wrong page, associate will be redirected to main page before trying to use functions that he is not suppose to.
And if LoggedInEmployee is null It will Redirect user to login page.

---------------------------------------------------------------------------------------

Sales record are created as one record per product.
If you have 10 items in cart, after processing tranaction, it will create 10 sales record, 1 per product.

The same for placing orders. Employee will order products one by one. Each time order placed for a product it will create
one order record


 */