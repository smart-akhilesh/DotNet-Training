-- Create customer
create or alter procedure createcustomer
    @name nvarchar(100),
    @age int,
    @phone nvarchar(15),
    @email nvarchar(100),
    @password nvarchar(255),
    @customerid int output
as
begin
    insert into Customers (customername, age, phone, emailid, password)
    values (@name, @age, @phone, @email, @password)

    set @customerid = scope_identity()
end


--Login customer
create or alter procedure logincustomer
    @email nvarchar(100),
    @password nvarchar(255),
    @isvalid bit output,
    @customerid int output
as
begin
    select @customerid = customerid
    from customers
    where emailid = @email and password = @password and isdeleted = 0

    if @customerid is not null
        set @isvalid = 1
    else
        set @isvalid = 0
end


--GetTrainsByRouteAndDate
CREATE or alter PROCEDURE GetTrainsByRouteAndDate
    @Source NVARCHAR(50),
    @Destination NVARCHAR(50),
    @TravelDate DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        T.Trainno,
        T.Trainname,
        T.Source,
        T.Destination,
        T.Sleeperprice,
        T.AC3price,
        T.AC2price,
        (T.Sleeperseats - ISNULL((SELECT SUM(B.Totalpassengers) FROM Bookings B WHERE B.Trainno = T.Trainno AND B.Traveldate = @TravelDate AND B.Classtype = 'Sleeper'), 0)) AS AvailableSleeperSeats,
        (T.AC3seats - ISNULL((SELECT SUM(B.Totalpassengers) FROM Bookings B WHERE B.Trainno = T.Trainno AND B.Traveldate = @TravelDate AND B.Classtype = 'AC3'), 0)) AS AvailableAC3Seats,
        (T.AC2seats - ISNULL((SELECT SUM(B.Totalpassengers) FROM Bookings B WHERE B.Trainno = T.Trainno AND B.Traveldate = @TravelDate AND B.Classtype = 'AC2'), 0)) AS AvailableAC2Seats
    FROM Trains T
    WHERE T.Source = @Source AND T.Destination = @Destination AND T.Isdeleted = 0;
END;



--GetAvailableSeatsAndPrice

CREATE or alter PROCEDURE GetAvailableSeatsAndPrice
    @TrainNo NVARCHAR(10),
    @TravelDate DATE,
    @ClassType NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @TotalSeats INT;
    DECLARE @BookedSeats INT;
    DECLARE @Price DECIMAL(10,2);

    -- Get total seats & price based on class
    IF @ClassType = 'Sleeper'
    BEGIN
        SELECT @TotalSeats = Sleeperseats, @Price = Sleeperprice FROM Trains WHERE Trainno = @TrainNo AND Isdeleted = 0;
    END
    ELSE IF @ClassType = 'AC3'
    BEGIN
        SELECT @TotalSeats = AC3seats, @Price = AC3price FROM Trains WHERE Trainno = @TrainNo AND Isdeleted = 0;
    END
    ELSE IF @ClassType = 'AC2'
    BEGIN
        SELECT @TotalSeats = AC2seats, @Price = AC2price FROM Trains WHERE Trainno = @TrainNo AND Isdeleted = 0;
    END
    ELSE
    BEGIN
        RAISERROR('Invalid class type.', 16, 1);
        RETURN;
    END

    -- Calculate booked seats on given travel date for the class
    SELECT @BookedSeats = ISNULL(SUM(Totalpassengers), 0)
    FROM Bookings
    WHERE Trainno = @TrainNo AND Traveldate = @TravelDate AND Classtype = @ClassType;

    -- Return available seats and price
    SELECT (@TotalSeats - @BookedSeats) AS AvailableSeats, @Price AS Price;
END;

--CreateBookingWithPassenger
ALTER PROCEDURE CreateBookingWithPassengers
    @CustomerId INT,
    @TrainNo NVARCHAR(10),
    @ClassType NVARCHAR(20),
    @TravelDate DATE,
    @TotalPassengers INT,
    @PaymentMethod NVARCHAR(20),
    @Passengers dbo.PassengerType READONLY,
    @BookingId INT OUTPUT,
    @Pnr NVARCHAR(10) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- Declare variables
    DECLARE @SeatCapacity INT;
    DECLARE @BookedSeats INT = 0;
    DECLARE @Price DECIMAL(10,2);
    DECLARE @TotalAmount DECIMAL(10,2);

    -- Step 1: Use CASE instead of IF...ELSE (works like switch)
    SELECT 
        @SeatCapacity = CASE @ClassType
                            WHEN 'Sleeper' THEN Sleeperseats
                            WHEN 'AC3' THEN AC3seats
                            WHEN 'AC2' THEN AC2seats
                            ELSE NULL
                        END,
        @Price = CASE @ClassType
                    WHEN 'Sleeper' THEN Sleeperprice
                    WHEN 'AC3' THEN AC3price
                    WHEN 'AC2' THEN AC2price
                    ELSE NULL
                END
    FROM Trains
    WHERE Trainno = @TrainNo AND Isdeleted = 0;

    -- Step 2: Check if class or train was invalid
    IF @SeatCapacity IS NULL OR @Price IS NULL
    BEGIN
        RAISERROR('Invalid class type or train not found.', 16, 1);
        RETURN;
    END

    -- Step 3: Get total already booked seats
    SELECT @BookedSeats = ISNULL(SUM(Totalpassengers), 0)
    FROM Bookings
    WHERE Trainno = @TrainNo AND Classtype = @ClassType AND Traveldate = @TravelDate AND Isdeleted = 0;

    -- Step 4: Check availability
    IF (@SeatCapacity - @BookedSeats) < @TotalPassengers
    BEGIN
        RAISERROR('Not enough seats available.', 16, 1);
        RETURN;
    END

    -- Step 5: Calculate amount and generate PNR
    SET @TotalAmount = @Price * @TotalPassengers;
    SET @Pnr = LEFT(CONVERT(NVARCHAR(40), NEWID()), 10); -- Random 10-char PNR

    -- Step 6: Insert Booking
    INSERT INTO Bookings (
        Pnr, Customerid, Trainno, Classtype, Traveldate, 
        Totalpassengers, Totalamount, Paymentmethod
    )
    VALUES (
        @Pnr, @CustomerId, @TrainNo, @ClassType, @TravelDate,
        @TotalPassengers, @TotalAmount, @PaymentMethod
    );

    SET @BookingId = SCOPE_IDENTITY();

    -- Step 7: Assign berths and seat numbers
    DECLARE @SeatNo INT = @BookedSeats + 1;
    DECLARE @RowNumber INT = 0;
    DECLARE @Berths TABLE (Berth NVARCHAR(20));

    INSERT INTO @Berths (Berth) VALUES
        ('Lower'), ('Middle'), ('Upper'), ('SideLower'), ('SideUpper');

    -- Variables for loop
    DECLARE @Name NVARCHAR(100), @Age INT, @Gender NVARCHAR(10);

    DECLARE passenger_cursor CURSOR FOR
        SELECT Passengername, Age, Gender FROM @Passengers;

    OPEN passenger_cursor;
    FETCH NEXT FROM passenger_cursor INTO @Name, @Age, @Gender;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Assign berth in round-robin
        DECLARE @BerthType NVARCHAR(20);
        SELECT TOP 1 @BerthType = Berth
        FROM (
            SELECT Berth, ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS RowNum
            FROM @Berths
        ) AS B
        WHERE B.RowNum = (@RowNumber % 5) + 1;

        -- Generate seat number
        DECLARE @SeatLabel NVARCHAR(20) = CONCAT(@ClassType, '-', @SeatNo);

        -- Insert passenger
        INSERT INTO Passengers (
            Bookingid, Passengername, Age, Gender, BerthType, SeatNo
        )
        VALUES (
            @BookingId, @Name, @Age, @Gender, @BerthType, @SeatLabel
        );

        SET @SeatNo = @SeatNo + 1;
        SET @RowNumber = @RowNumber + 1;

        FETCH NEXT FROM passenger_cursor INTO @Name, @Age, @Gender;
    END

    CLOSE passenger_cursor;
    DEALLOCATE passenger_cursor;
END

--ViewBookingDetailsByPNR

ALTER PROCEDURE ViewBookingDetailsByPNR
    @Pnr NVARCHAR(10)
AS
BEGIN
    -- Booking Info
    SELECT 
        B.Bookingid,
        B.Pnr,
        B.Customerid,
        C.CustomerName,
        B.Trainno,
        T.Trainname,
        B.Classtype,
        B.Traveldate,
        B.Totalpassengers,
        B.Totalamount,
        B.Paymentmethod,
        B.Bookingdate,
        B.Status
    FROM Bookings B
    INNER JOIN Customers C ON B.Customerid = C.Customerid
    INNER JOIN Trains T ON B.Trainno = T.Trainno
    WHERE B.Pnr = @Pnr AND B.Isdeleted = 0;

    -- Passenger Info
    SELECT 
        P.Passengerid,
        P.Passengername,
        P.Age,
        P.Gender,
        P.Status,
        P.BerthType,
        P.SeatNo
    FROM Passengers P
    INNER JOIN Bookings B ON P.Bookingid = B.Bookingid
    WHERE B.Pnr = @Pnr AND P.Isdeleted = 0;
END;


--CancelPassengerTicket
ALTER PROCEDURE CancelPassengerTicket
    @Pnr NVARCHAR(10),
    @PassengerId INT,
    @RefundAmount DECIMAL(10,2) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @BookingId INT,
            @TravelDate DATE,
            @ClassType NVARCHAR(20),
            @TotalAmount DECIMAL(10,2),
            @RefundPercentage INT = 0,
            @OriginalPassengerCount INT;

    -- Initialize output
    SET @RefundAmount = 0;

    -- Get booking info
    SELECT 
        @BookingId = Bookingid,
        @TravelDate = Traveldate,
        @ClassType = Classtype,
        @TotalAmount = Totalamount
    FROM Bookings
    WHERE Pnr = @Pnr AND Isdeleted = 0;

    IF @BookingId IS NULL
    BEGIN
        RAISERROR(' Invalid PNR.', 16, 1);
        RETURN;
    END

    -- Check passenger existence and status
    IF NOT EXISTS (
        SELECT 1
        FROM Passengers
        WHERE Passengerid = @PassengerId
          AND Bookingid = @BookingId
          AND Isdeleted = 0
          AND Status = 'active'
    )
    BEGIN
        RAISERROR(' Passenger not found or already cancelled.', 16, 1);
        RETURN;
    END

    -- Cancel passenger
    UPDATE Passengers
    SET Status = 'Cancelled'
    WHERE Passengerid = @PassengerId;

    -- Count remaining active passengers
    DECLARE @ActivePassengers INT;

    SELECT @ActivePassengers = COUNT(*)
    FROM Passengers
    WHERE Bookingid = @BookingId AND Status = 'active' AND Isdeleted = 0;

    IF @ActivePassengers = 0
    BEGIN
        UPDATE Bookings
        SET Status = 'Cancelled',
            Totalpassengers = 0
        WHERE Bookingid = @BookingId;
    END
    ELSE
    BEGIN
        UPDATE Bookings
        SET Totalpassengers = @ActivePassengers
        WHERE Bookingid = @BookingId;
    END

    -- Calculate refund amount
    DECLARE @HoursBefore INT = DATEDIFF(HOUR, GETDATE(), DATEADD(DAY, 1, @TravelDate));

    SELECT TOP 1 @RefundPercentage = Refundpercentage
    FROM Refundrules
    WHERE Classtype = @ClassType AND Hoursbeforetravel <= @HoursBefore
    ORDER BY Hoursbeforetravel DESC;

    IF @RefundPercentage IS NULL
        SET @RefundPercentage = 0;

    SELECT @OriginalPassengerCount = COUNT(*) 
    FROM Passengers
    WHERE Bookingid = @BookingId AND Isdeleted = 0;

    IF @OriginalPassengerCount > 0
    BEGIN
        SET @RefundAmount = ROUND((@TotalAmount / @OriginalPassengerCount) * (@RefundPercentage / 100.0), 2);
    END

    -- Insert cancellation record
    INSERT INTO Cancellations (Bookingid, Passengerid, Refundamount)
    VALUES (@BookingId, @PassengerId, @RefundAmount);
END


--VerifyAdminLogin

CREATE PROCEDURE VerifyAdminLogin
    @Username NVARCHAR(50),
    @Password NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(*) AS MatchCount
    FROM Admins
    WHERE Username = @Username AND Password = @Password; -- Plain text
END;


--InsertTrain

CREATE PROCEDURE InsertTrain
    @Trainno NVARCHAR(10),
    @Trainname NVARCHAR(100),
    @Source NVARCHAR(50),
    @Destination NVARCHAR(50),
    @Sleeperseats INT,
    @AC3seats INT,
    @AC2seats INT,
    @Sleeperprice DECIMAL(7,2),
    @AC3price DECIMAL(7,2),
    @AC2price DECIMAL(7,2)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Trains WHERE Trainno = @Trainno)
    BEGIN
        RAISERROR('Train with this number already exists.', 16, 1);
        RETURN;
    END

    INSERT INTO Trains (Trainno, Trainname, Source, Destination, Sleeperseats, AC3seats, AC2seats, Sleeperprice, AC3price, AC2price)
    VALUES (@Trainno, @Trainname, @Source, @Destination, @Sleeperseats, @AC3seats, @AC2seats, @Sleeperprice, @AC3price, @AC2price);
END


--UpdateTrain
!-- Procedure to update the train
CREATE PROCEDURE UpdateTrain
    @Trainno NVARCHAR(10),
    @Trainname NVARCHAR(100),
    @Source NVARCHAR(50),
    @Destination NVARCHAR(50),
    @Sleeperseats INT,
    @AC3seats INT,
    @AC2seats INT,
    @Sleeperprice DECIMAL(7,2),
    @AC3price DECIMAL(7,2),
    @AC2price DECIMAL(7,2)
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Trains WHERE Trainno = @Trainno AND Isdeleted = 0)
    BEGIN
        RAISERROR('Train not found or deleted.', 16, 1);
        RETURN;
    END

    UPDATE Trains
    SET Trainname = @Trainname,
        Source = @Source,
        Destination = @Destination,
        Sleeperseats = @Sleeperseats,
        AC3seats = @AC3seats,
        AC2seats = @AC2seats,
        Sleeperprice = @Sleeperprice,
        AC3price = @AC3price,
        AC2price = @AC2price
    WHERE Trainno = @Trainno;
END





--delete train
alter PROCEDURE DeleteTrain
    @Trainno NVARCHAR(10),
    @TotalRefundAmount DECIMAL(10,2) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if train exists and is not already deleted
    IF NOT EXISTS (SELECT 1 FROM Trains WHERE Trainno = @Trainno AND IsDeleted = 0)
    BEGIN
        RAISERROR('Train not found or already deleted.', 16, 1);
        RETURN;
    END

    -- Soft delete the train (this will trigger cancellations and refunds)
    UPDATE Trains
    SET IsDeleted = 1
    WHERE Trainno = @Trainno;

    -- Wait for trigger to complete and then calculate total refund
    SELECT @TotalRefundAmount = ISNULL(SUM(c.RefundAmount), 0)
    FROM Cancellations c
    JOIN Bookings b ON c.BookingID = b.BookingID
    WHERE b.TrainNo = @Trainno
      AND b.Status = 'Cancelled'
      AND c.CancellationDate >= CAST(GETDATE() AS DATE);
END


--GetTotalBookingsAndRevenueByDate
CREATE PROCEDURE GetTotalBookingsAndRevenueByDate
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        COUNT(*) AS TotalBookings,
        ISNULL(SUM(Totalamount), 0) AS TotalRevenue
    FROM Bookings
    WHERE Isdeleted = 0 
      AND Status = 'active'
      AND Bookingdate BETWEEN @StartDate AND DATEADD(DAY, 1, @EndDate);  -- includes full end date
END;



--GetBookingsPerTrainByDate
CREATE PROCEDURE GetBookingsPerTrainByDate
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        b.Trainno,
        t.Trainname,
        COUNT(*) AS TotalBookings
    FROM Bookings b
    JOIN Trains t ON b.Trainno = t.Trainno
    WHERE b.Isdeleted = 0 
      AND b.Status = 'active'
      AND b.Bookingdate BETWEEN @StartDate AND DATEADD(DAY, 1, @EndDate)
    GROUP BY b.Trainno, t.Trainname
    ORDER BY TotalBookings DESC;
END;


--GetCancellationRefundsReport
CREATE OR ALTER PROCEDURE GetCancellationRefundsReport
    @StartDate DATE = NULL,
    @EndDate DATE = NULL,
    @TrainNo NVARCHAR(10) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.Cancellationid,
        c.Bookingid,
        b.Trainno,
        c.Passengerid,
        c.Refundamount,
        c.Cancellationdate,
        c.IsRefunded,
        CASE WHEN c.IsRefunded = 0 THEN 'Not Refunded' ELSE 'Refunded' END AS RefundStatus
    FROM Cancellations c
    INNER JOIN Bookings b ON c.Bookingid = b.Bookingid
    WHERE ( (@StartDate IS NULL OR c.Cancellationdate >= @StartDate)
        AND (@EndDate IS NULL OR c.Cancellationdate <= @EndDate)
        AND (@TrainNo IS NULL OR b.Trainno = @TrainNo) )
      AND b.Isdeleted = 0
    ORDER BY c.Cancellationdate DESC;
END;



CREATE TRIGGER trg_AfterTrainDeleted
ON Trains
AFTER UPDATE
AS
BEGIN
SET NOCOUNT ON;

-- Only trigger if IsDeleted changed to 1
IF EXISTS (
    SELECT 1 FROM inserted i
    JOIN deleted d ON i.TrainNo = d.TrainNo
    WHERE i.IsDeleted = 1 AND d.IsDeleted = 0
)
BEGIN
    DECLARE @Today DATE = CAST(GETDATE() AS DATE);

    -- Cancel active bookings for this train
    UPDATE b
    SET b.Status = 'Cancelled',
        b.IsDeleted = 1
    FROM Bookings b
    INNER JOIN inserted i ON b.TrainNo = i.TrainNo
    WHERE b.TravelDate >= @Today AND b.Status = 'Active';

    -- Insert into Cancellations table
    INSERT INTO Cancellations (BookingID, RefundAmount, CancellationDate)
    SELECT
        b.BookingID,
        b.TotalAmount * 0.5, -- Assume 50% refund on system cancel
        GETDATE()
    FROM Bookings b
    INNER JOIN inserted i ON b.TrainNo = i.TrainNo
    WHERE b.TravelDate >= @Today AND b.Status = 'Cancelled';
END
END;

ALTER TRIGGER trg_AfterTrainDeleted
ON Trains
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Today DATE = CAST(GETDATE() AS DATE);

    -- Handle soft delete only (IsDeleted changed from 0 to 1)
    IF EXISTS (
        SELECT 1
        FROM inserted i
        JOIN deleted d ON i.TrainNo = d.TrainNo
        WHERE i.IsDeleted = 1 AND d.IsDeleted = 0
    )
    BEGIN
        -- Cancel all ACTIVE bookings for the affected train(s) from today onward
        UPDATE b
        SET b.Status = 'Cancelled',
            b.IsDeleted = 1
        FROM Bookings b
        JOIN inserted i ON b.TrainNo = i.TrainNo
        WHERE b.TravelDate >= @Today
          AND b.Status = 'Active';

        -- Cancel passengers related to those bookings
        UPDATE p
        SET p.Status = 'Cancelled',
            p.IsDeleted = 1
        FROM Passengers p
        JOIN Bookings b ON p.BookingID = b.BookingID
        JOIN inserted i ON b.TrainNo = i.TrainNo
        WHERE b.TravelDate >= @Today
          AND b.Status = 'Cancelled'
          AND p.Status = 'Active';

        -- Insert refund records for cancelled bookings (assuming 50% refund)
        INSERT INTO Cancellations (BookingID, RefundAmount, CancellationDate)
        SELECT
            b.BookingID,
            b.TotalAmount,  -- 50% refund
            GETDATE()
        FROM Bookings b
        JOIN inserted i ON b.TrainNo = i.TrainNo
        WHERE b.TravelDate >= @Today
          AND b.Status = 'Cancelled';
    END
END;


select * from Trains
select * from bookings where Trainno = 1278
select * from Cancellations
select * from Passengers

select * from Admins

Drop table Cancellations