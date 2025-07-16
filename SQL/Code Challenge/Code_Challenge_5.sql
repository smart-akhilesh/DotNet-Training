create database CodingChallenge_5;

create table books
( Id int primary key,
Title varchar(30) not null,
Author varchar(30),
Isbn bigint,
Published_Date varchar(30)
);

ALTER TABLE books
ALTER COLUMN Isbn bigint;

select * from books;

insert into books values
(1, 'My First SQL book', 'Mary Parker', 981483029127, '2012-02-22 12:08:17'),
(2, 'My Second SQL book', 'John Mayer', 857300923713, '1972-07-03 09:22:45'),
(3, 'My Third SQL book', 'Cary Flint', 523120967812, '2015-10-18 14:05:44');


create table Reviews
( Id int primary key,
Book_Id int references books(id),
Reviewer_name varchar(30),
content varchar(30) not null,
rating int,
Published_Date varchar(30)
);

select * from Reviews;

insert into Reviews values
(1,1, 'John Smith', 'My first review', 4, '2017-12-10 05:50:11'),
(2,2, 'John Smith', 'My second review', 5, '2017-10-13 15:05:12'),
(3,2, 'Alice Walker', 'Another review', 1, '2017-10-22 23:47:10');


--Write a query to fetch the details of the books written by author whose name ends with er.

Select * from books where author like '%er';



--Display the Title ,Author and ReviewerName for all the books from the above table

Select books.Title, books.Author, reviews.Reviewer_name
from books, reviews
where books.Id = reviews.Book_Id;




--Display the reviewer name who reviewed more than one book
Select Reviewer_name, count(distinct Book_Id) as No_of_Books_Review
from Reviews
group by Reviewer_name
having count(distinct Book_Id) > 1;


create table customer
( Id int primary key,
Name varchar(30),
Age int,
Address varchar(30),
Salary int
);

Select * from customer;

ALTER TABLE customer
ALTER COLUMN salary varchar(30);

Insert into customer Values
(1,'Ramesh', 32, 'Ahmedabad', 2000.00),
(2,'Khilan', 25, 'Delhi', 1500.00),
(3,'Kaushik', 23, 'Kota', 2000.00),
(4,'Chaitali', 25, 'Mumbai', 6500.00),
(5,'Hardik', 27, 'Bhopal', 8500.00),
(6,'Komal', 22, 'MP', 4500.00),
(7,'Muffy', 24, 'Indore', 10000.00);




--Display the Name for the customer from above customer table who live in same address which has character o anywhere in address

Select * from customer where Address like '%o%';

create table customer_order
(OID int primary key,
Date varchar(30),
Customer_id int references customer(id),
Amount int);

Select * from customer_order;

Insert into customer_order values
(102, '2009-10-08 00:00:00', 3, 3000),
(100, '2009-10-08 00:00:00', 3, 1500),
(101, '2009-11-20 00:00:00', 2, 1560),
(103, '2008-05-20 00:00:00', 4,2060);




--Write a query to display the Date,Total no of customer placed order on same Date

SELECT Date, COUNT(Customer_id) AS Total_Customers
FROM customer_order
GROUP BY Date;

update customer
set Salary = NULL
where name in ('Komal', 'Muffy');

select * from customer;





--Display the Names of the Employee in lower case, whose salary is null
Select lower(Name), salary as Customer_Name
from customer
where Salary IS NULL;


create table student_details
(RegisterNo int primary key,
Name varchar(30),
Age int,
Qualification varchar(30),
MobileNo bigint,
Mail_Id varchar(30),
Location varchar(30),
Gender varchar(30));

select * from student_details;

insert into student_details values
(2, 'Sai', 22, 'B.E', '9952836777', 'Sai@gmail.com', 'chennai', 'M'),
(3, 'Kumar', 20, 'BSC', '7890125648', 'Sai@gmail.com', 'chennai', 'M'),
(4, 'Selvi', 22, 'B.Tech', '8904567342', 'Sai@gmail.com', 'chennai', 'F'),
(5, 'Nisha', 25, 'M.E', '9952836777', 'Sai@gmail.com', 'chennai', 'F'),
(6, 'SaiSaran', 21, 'B.A', '9952836777', 'Sai@gmail.com', 'chennai', 'F'),
(7, 'Tom', 23, 'BCA', '9952836777', 'Sai@gmail.com', 'chennai', 'M');


insert into student_details values
(8, 'Sai', 22, 'B.E', '9952836777', 'Sai@gmail.com', 'chennai', 'F');





--Display the Names of the Employee in lower case, whose salary is null
Select Gender, COUNT(*) as 'Gender count'
from student_details
group by Gender
having Gender IN ('M', 'F');


