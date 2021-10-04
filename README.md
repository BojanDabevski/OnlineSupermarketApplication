# OnlineSupermarketApplication
Project assignment for the curriculum Integrated Systems to make a MVC Onion Architecture web application to sell products online as a online supermarket - Bojan Dabevski 181115

##Integrated characteristics: 

-The application is based an built on a Onion Architecture 
-User registration with the ability to choose a role(Admin and User). Also the ability to give admin priveleges to normal users and takeaway admin priveleges
-All CRUD operations 
-Viewing all available products. Able to add more products, and edit them and delete any.Each product has a name, description, image for the product, price, rating, expiration date and category.Products are also ordered by expiration date in ascending order. 
-You can also sort products by category. Here you have the ability to choose a certain category and you will then export just the products with the category you wanted into a excel document
-You can add prodcuts to shopping cart and there if you want to remove a product you can delete it 
-In the shopping cart you can order and pay for the products you had added to the shopping cart, and the payment is done using the Stripe payment processing. After payment you will receive an email with the details of your order. 
-In the order sections you can see all previously made orders, and you can see the details of every order with the press of a button.Also here you can create an invoice for every order made in the form of a pdf document. Admins can also download all the orders made as a excel document. 
-In the all tickets sections other than seeing, adding, editing and deleting tickets you can also export all the tickets into a excel document.
-In the import users page you can import users from a excel document into the application. Each user is imported with their email,password,password confirmation and role.
