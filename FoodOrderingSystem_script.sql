begin
create database FoodOrderingSystem
end
go

use FoodOrderingSystem
go

create table Role (
	RoleId int identity(1,1) primary key,
	Name varchar(50) not null
)

create table MenuStatus (
	MenuStatusId int identity(1,1) primary key,
	Name varchar(50) not null
)

create table OrderStatus (
	OrderStatusId int identity(1,1) primary key,
	Name varchar(50) not null
)

create table [User] (
	Username varchar(50) primary key,
	Name varchar(100) not null,
	PasswordHash varbinary(512) not null,
	PasswordSalt varbinary(512) not null,
	RoleId int foreign key references Role(RoleId) not null
)

create table Menu (
	MenuId int identity(1,1) primary key,
	Name varchar(100) not null,
	MenuStatusId int foreign key references MenuStatus(MenuStatusId) not null
)

create table [Order] (
	OrderId int identity(1,1) primary key,
	OrderNo char(15) not null,
	TableNo int not null,
	Date datetime not null,
	Username varchar(50) foreign key references [User](Username) not null,
	OrderStatusId int foreign key references OrderStatus(OrderStatusId) not null
)

create table MenuOrder (
	MenuOrderId int identity(1,1) primary key,
	Qty int not null,
	MenuId int foreign key references Menu(MenuId) not null,
	OrderId int foreign key references [Order](OrderId) not null
)


insert into MenuStatus (Name) values
('Ready'),
('Empty')


insert into [Role] (Name) values
('Pelayan'),
('Kasir')

insert into OrderStatus (Name) values
('Active'),
('Finished')
