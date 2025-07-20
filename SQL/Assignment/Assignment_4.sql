use infinitedb

--1. Write a T-SQL Program to find the factorial of a given number.
declare @num int = 4, @mul int = 1
begin transaction
while(@num > 1)
begin 
set @mul = @mul * @num
set @num =  @num - 1
end
print 'The Factorial is' + ' = ' + cast(@mul as varchar)
save transaction t1
commit 



--2. Create a stored procedure to generate multiplication table that accepts a number and generates up to a given number. 
create or alter proc print_multiplication_table @num int, @limit int
as
begin
 if(@num < 1)
 begin
  raiserror('Please enter positive no.',15,1)
  return
 end
 declare @i int = 1
 while(@i <= @limit)
 begin
 print cast(@num as varchar) + ' * ' + cast(@i as varchar) + ' = ' + cast(@num * @i as varchar)
 set @i += 1
 end
end 

--call the procedure by passing the num and limit
print_multiplication_table 0, 10


--3. Create a function to calculate the status of the student. If student score >=50 then pass, else fail. Display the data neatly
create table students
(Sid int primary key,
Sname varchar(20)
)

insert into students values
(1, 'Jack'),
(2, 'Rithvik'),
(3, 'Jaspreeth'),
(4, 'Praveen'),
(5, 'Bisa'),
(6, 'Suraj')

select * from students

create table marks 
(mid int primary key,
sid int references students(Sid),
score int
)


insert into marks values 
(1, 1, 23),
(2, 6, 95),
(3, 4, 98),
(4, 2, 17),
(5, 3, 53),
(6, 5, 13);

select * from marks
select * from students


-- Creating a function for result status
create or alter function status_of_student(@score int)
returns varchar(10)
as
begin
declare @result varchar(10)
if(@score >= 50)
set @result = 'Pass'
else
set @result = 'Fail'
return @result
end

select s.sid, s.sname, m.score, dbo.status_of_student(m.score) as [Result Status] from students s join marks m on s.sid = m.sid 