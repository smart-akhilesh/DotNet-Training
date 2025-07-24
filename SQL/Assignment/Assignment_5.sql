use infinitedb

--1. Write a T-Sql based procedure to generate complete payslip of a given employee with respect to the following condition
   --a) HRA as 10% of Salary
   --b) DA as 20% of Salary
   --c) PF as 8% of Salary
   --d) IT as 5% of Salary
   --e) Deductions as sum of PF and IT
   --f) Gross Salary as sum of Salary, HRA, DA
   --g) Net Salary as Gross Salary - Deductions


create procedure sp_generatepayslip
    @empid int
as
begin

    declare 
        @empname varchar(100),
        @salary decimal(18,2),
        @hra decimal(18,2),
        @da decimal(18,2),
        @pf decimal(18,2),
        @it decimal(18,2),
        @deductions decimal(18,2),
        @grosssalary decimal(18,2),
        @netsalary decimal(18,2);

    select @empname = ename, @salary = salary
    from employee
    where empno= @empid;

    if @empname is null
    begin
        print 'employee not found.';
        return;
    end

    set @hra = 0.10 * @salary;
    set @da = 0.20 * @salary;
    set @pf = 0.08 * @salary;
    set @it = 0.05 * @salary;
    set @deductions = @pf + @it;
    set @grosssalary = @salary + @hra + @da;
    set @netsalary = @grosssalary - @deductions;
 
    print  '----------- employee payslip---------------' 
    print 'employee id   : ' + cast(@empid as varchar);
    print 'employee name : ' + @empname;
    print 'basic salary  : ' + cast(@salary as varchar);
    print 'hra (10%)     : ' + cast(@hra as varchar);
    print 'da (20%)      : ' + cast(@da as varchar);
    print 'gross salary  : ' + cast(@grosssalary as varchar);
    print 'pf (8%)       : ' + cast(@pf as varchar);
    print 'it (5%)       : ' + cast(@it as varchar);
    print 'deductions    : ' + cast(@deductions as varchar);
    print 'net salary    : ' + cast(@netsalary as varchar);
end;


exec sp_GeneratePayslip @EmpID = 7001;

select * from Employee


--2.  Create a trigger to restrict data manipulation on EMP table during General holidays. Display the error message like “Due to Independence day you cannot manipulate data” or "Due To Diwali", you cannot manipulate" etc
--Note: Create holiday table with (holiday_date,Holiday_name). Store at least 4 holiday details. try to match and stop manipulation 

create table holiday (
    holiday_date date ,
    holiday_name varchar(30)
);

insert into holidaymaster (holiday_date, holiday_name)
values
('2025-08-15', 'independence day'),
('2025-10-29', 'diwali'),
('2025-01-26', 'republic day'),
('2025-12-25', 'christmas');

-- create trigger to restrict data manipulation during holidays
create trigger trg_restrictemployee
on employee
after insert, update, delete
as
begin
    declare @today date = cast(getdate() as date);
    declare @holidayname varchar(100);

    select @holidayname = holiday_name
    from holiday
    where holiday_date = @today;

    if @holidayname is not null
    begin
        print 'due to holiday data manipulation not permit'
        rollback transaction;
    end
end


insert into Employee(Empno, EName, Salary)
VALUES (7300, 'Akhilesh', 60000);


