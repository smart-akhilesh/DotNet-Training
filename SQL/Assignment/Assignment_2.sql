use infinitedb

--Table create for department
create table dept(
deptno int primary key,
dname varchar(30),
loc varchar(30)
)
 
--Insert values in department table
insert into dept values
(10,'ACCOUNTING','NEW YORK'),
(20,'RESEARCH','DALLAS'),
(30,'SALES','CHICAGO' ),
(40,'OPERATIONS','BOSTON')


--Table create for employee
create table emp(
empno int primary key,
ename varchar(30) not null,
job varchar(30) not null,
mgr_id int,
hire_date varchar(30),
salary int,
comm int,
deptno int references dept(deptno)
)
 

 --Insert values in employees table
insert into emp values 
(7369, 'SMITH', 'CLERK', 7902, '17-DEC-80', 800, NULL, 20),
(7499, 'ALLEN', 'SALESMAN', 7698, '20-FEB-81', 1600, 300, 30),
(7521, 'WARD', 'SALESMAN', 7698, '22-FEB-81', 1250, 500, 30),
(7566, 'JONES', 'MANAGER', 7839, '02-APR-81', 2975, NULL, 20),
(7654, 'MARTIN', 'SALESMAN', 7698, '28-SEP-81', 1250, 1400, 30),
(7698, 'BLAKE', 'MANAGER', 7839, '01-MAY-81', 2850, NULL, 30),
(7782, 'CLARK', 'MANAGER', 7839, '09-JUN-81', 2450, NULL, 10),
(7788, 'SCOTT', 'ANALYST', 7566, '19-APR-87', 3000, NULL, 20),
(7839, 'KING', 'PRESIDENT', NULL, '17-NOV-81', 5000, NULL, 10),
(7844, 'TURNER', 'SALESMAN', 7698, '08-SEP-81', 1500, 0, 30),
(7876, 'ADAMS', 'CLERK', 7788, '23-MAY-87', 1100, NULL, 20),
(7900, 'JAMES', 'CLERK', 7698, '03-DEC-81', 950, NULL, 30),
(7902, 'FORD', 'ANALYST', 7566, '03-DEC-81', 3000, NULL, 20),
(7934, 'MILLER', 'CLERK', 7782, '23-JAN-82', 1300, NULL, 10)

 

select * from emp
select * from dept

-- List all employees whose name begins with 'A'. 
select * from emp where ename like 'a%'

--2. Select all those employees who don't have a manager. 
select * from emp where mgr_id is null

--3. List employee name, number and salary for those employees who earn in the range 1200 to 4000.
select ename, empno, salary from emp where salary between 1200 and 4000

--4. Give all the employees in the RESEARCH department a 10% pay rise. Verify that this has been done by listing all their details before and after the rise. 
select * from emp where deptno = (Select deptno from dept where dname = 'research')

update emp set salary = (salary + (0.1 * salary)) where deptno = (Select deptno from dept where dname = 'research')

--5. Find the number of CLERKS employed. Give it a descriptive heading. 
select count(job) as [No of Clerks employed] from emp where job = 'clerk' 

--6. Find the average salary for each job type and the number of people employed in each job. 
select job, avg(salary)as [Avg salary of each job], count(job) as [Total no. of people employ] from emp group by job

--7. List the employees with the lowest and highest salary. 
select * from emp order by salary   -- list employee salary in ascending order
select * from emp order by salary desc  -- list emoloyee salary in descending order

--8. List full details of departments that don't have any employees. 
select * from dept where deptno not in (select deptno from emp)

--9. Get the names and salaries of all the analysts earning more than 1200 who are based in department 20. Sort the answer by ascending order of name. 
select ename, salary, job, deptno from emp where job = 'analyst' and salary > 1200 and deptno = 20 order by ename 

--10. For each department, list its name and number together with the total salary paid to employees in that department. 
select d.deptno, d.dname, sum(e.salary) as [Total salary paid] from emp e full join dept d on d.deptno = e.deptno group by d.deptno, d.dname

--11. Find out salary of both MILLER and SMITH.
select ename, salary from emp where ename in ('miller', 'smith')

--12. Find out the names of the employees whose name begin with ‘A’ or ‘M’. 
select ename from emp where ename like ('a%') or ename like ('m%')

--13. Compute yearly salary of SMITH. 
select ename, (salary *12) as [Annual Salary] from emp where ename = 'Smith'

--14. List the name and salary for all employees whose salary is not in the range of 1500 and 2850. 
select ename, salary from emp where salary not between 1500 and 2850

--15. Find all managers who have more than 2 employees reporting to them
select n.mgr_id, e.ename, n.total_emp_count as [Total employee under]  from emp e join (select mgr_id, count(*) as 'total_emp_count' from emp group by mgr_id having count(*) > 2) n on e.empno=  n.mgr_id

