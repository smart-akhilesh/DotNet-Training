--Table Schema

CREATE TABLE Admins (
    AdminId INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL,
    CreatedDate DATETIME DEFAULT GETDATE()
);


--Booking Table

create table Bookings (
    Bookingid int primary key identity(1,1),
    Pnr nvarchar(10) not null unique,
    Customerid int not null,
    Trainno nvarchar(10) not null,
    Classtype nvarchar(20) not null,
    Traveldate date not null,
    Totalpassengers int not null,
    Totalamount decimal(10,2) not null,
    Bookingdate datetime default getdate(),
    Paymentmethod nvarchar(20) default 'cash',
    Status nvarchar(20) default 'active',
    Isdeleted bit default 0,
    foreign key (Customerid) references Customers(Customerid),
    foreign key (Trainno) references Trains(Trainno)
);


--Cancellation table

create table Cancellations (
    Cancellationid int primary key identity(1,1),
    Bookingid int not null,
    Passengerid int null,
    Refundamount decimal(10,2) not null,
    Cancellationdate datetime default getdate(),
    IsRefunded bit default 0,
    foreign key (Bookingid) references Bookings(Bookingid),
    foreign key (Passengerid) references Passengers(Passengerid)
);


--Customers
create table Customers (
    Customerid int primary key identity(1,1),
    Customername nvarchar(100) not null,
    Age int not null,
    Phone nvarchar(15) not null unique,
    Emailid nvarchar(100) not null unique,
    Password nvarchar(255) not null,
    Isdeleted bit default 0
);

--Passengertype
CREATE TYPE dbo.PassengerType AS TABLE
(
    Passengername NVARCHAR(100),
    Age INT,
    Gender NVARCHAR(10)
);

--Train
create table Trains (
    Trainno nvarchar(10) primary key,
    Trainname nvarchar(100) not null,
    Source nvarchar(50) not null,
    Destination nvarchar(50) not null,
    Sleeperseats int not null,
    AC3seats int not null,
    AC2seats int not null,
    Sleeperprice decimal(7,2) not null,
    AC3price decimal(7,2) not null,
    AC2price decimal(7,2) not null,
    Isdeleted bit default 0
);


--Passenger
create table Passengers (
    Passengerid int primary key identity(1,1),
    Bookingid int not null,
    Passengername nvarchar(100) not null,
    Age int not null,
    Gender nvarchar(10) not null,
    Status nvarchar(20) default 'active',
    Isdeleted bit default 0,
	SeatNo nvarchar(20),
	BerthType nvarchar(20),
    foreign key (Bookingid) references Bookings(Bookingid)
);



--Refund Rule
create table Refundrules (
    Ruleid int primary key identity(1,1),
    Classtype nvarchar(20) not null,
    Hoursbeforetravel int not null,
    Refundpercentage int not null,
    IsDeleted bit default 0,
    unique(Classtype, Hoursbeforetravel)
);


select * from Trains
select * from Refundrules
select * from Customers