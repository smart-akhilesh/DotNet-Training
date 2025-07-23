use infinitedb

--1.Write a query to display your birthday( day of week)
select  'Akhilesh Singh' as [My Name], datename(weekday, '2001-05-17') as [My Birthday Day];


--2. Write a query to display your age in days
select 'Akhilesh Singh' as [My Name],datediff(day, '2001-05-17', getdate()) as [My Age In Days];

--3.	Write a query to display all employees information those who joined before 5 years in the current month
select *, datediff(year, cast(hire_date as date), getdate()) as experience_years from emp2
where  datediff(year, cast(hire_date as date), getdate()) >= 5 and month(cast(hire_date as date)) = month(getdate());


--4.	Create table Employee with empno, ename, sal, doj columns or use your emp table and perform the following operations in a single transact First insert 3 rows Update the second row sal with 15% increment  c. Delete first row.
--After completing above all actions, recall the deleted row without losing increment of second row.

begin transaction;

insert into emp2 (empno, ename, job, mgr_id, hire_date, salary, comm, deptno) values
(1, 'Aman', 'Manager', 1, '2020-01-01', 1000, NULL, 10),
(2, 'Mayank', 'Developer', 1, '2019-05-15', 1500, NULL, 10),
(3, 'Anshul', 'Analyst', 1, '2021-03-20', 1200, NULL, 20);

create table #DeletedRows (
    empno int primary key,
    ename varchar(30),
    job varchar(30),
    mgr_id int,
    hire_date varchar(30),
    salary int,
    comm int,
    deptno int
);

update emp2
set salary = salary * 1.15
where empno = 2;

delete from emp2
output deleted.empno, deleted.ename, deleted.job, deleted.mgr_id, deleted.hire_date, deleted.salary, deleted.comm, deleted.deptno
into #DeletedRows
where empno = 1;

insert into emp2 (empno, ename, job, mgr_id, hire_date, salary, comm, deptno)
select empno, ename, job, mgr_id, hire_date, salary, comm, deptno from #DeletedRows;

commit transaction;

select * from emp2


--5 --------------------------------------------------------Function

create or alter function dbo.bonuscal
(
    @deptno int,
    @salary int
)
returns float
as
begin
    declare @bonus float;

    if @deptno = 10
        set @bonus = @salary * 0.15;
    else if @deptno = 20
        set @bonus = @salary * 0.20;
    else
        set @bonus = @salary * 0.05;

    return @bonus;
end;



select empno, ename, deptno, salary, dbo.bonusCal(deptno, salary) as [Employee Bonus] from emp;


--6. Create a procedure to update the salary of employee by 500 whose dept name is Sales and current salary is below 1500 (use emp table)

create procedure dbo.salaryupdateforsalesemp
as
begin
    update e
    set e.salary = e.salary + 500
    from emp e
    join dept d on e.deptno = d.deptno
    where d.dname = 'sales'
      and e.salary < 1500;
end;

exec dbo.salaryupdateforsalesemp




