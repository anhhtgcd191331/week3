create database mystore;
use mystore;

create table users
(id int identity primary key,
firstname nvarchar(20));

create table orders
(order_id int identity primary key,
u_id int foreign key references users(id),
order_date date);

create table products
(prod_id int identity primary key,
prod_name nvarchar(20));

create table order_details
(order_id int foreign key references orders(order_id),
product_id int foreign key references products(prod_id),
quantity int,
primary key(order_id,product_id));
