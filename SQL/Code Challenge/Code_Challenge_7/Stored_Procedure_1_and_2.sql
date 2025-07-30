use infinitedb

 --Stored procedure
 create or alter proc insertemployeedetails
    @name varchar(40),
    @salary float,
    @gender char(1),
    @Netsalary float output
as
begin
    declare @empid int;

    set @Netsalary = @salary * 0.9;

    insert into employee_details (name, salary, gender)
    values (@name, @salary, @gender);

    set @empid = @@identity;

    return @empid; 
end

--Call store procedure
declare @generatedempid int;
declare @netsalary float;

exec @generatedempid = insertemployeedetails
    @name = 'rajesh',
    @salary = 75000,
    @gender = 'm',
    @netsalary = @netsalary output;

print 'generated empid: ' + cast(@generatedempid as varchar(10));
print 'net salary: ' + cast(@netsalary as varchar(20));



--2. Store Procedure
create or alter procedure updatesalarybyempid
    @empid int,
    @updatedsalary float output
as
begin

    update employee_details
    set salary = salary + 100
    where empid = @empid;

    select @updatedsalary = salary from employee_details where empid = @empid;
end

--Call the procedure
declare @empid int = 4;
declare @updatedsalary float;

exec updatesalarybyempid
    @empid = @empid,
    @updatedsalary = @updatedsalary output;

print 'updated salary for empid ' + cast(@empid as varchar(10)) + ' is: ' + cast(@updatedsalary as varchar(20));

