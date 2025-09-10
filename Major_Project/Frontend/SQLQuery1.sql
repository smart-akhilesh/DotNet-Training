use majorproject

CREATE TABLE Users (

    UserId INT PRIMARY KEY IDENTITY,

    Username NVARCHAR(50),

    Password NVARCHAR(50),

    Email NVARCHAR(100),

    Address NVARCHAR(200),

    PhoneNumber NVARCHAR(20)

)
 
CREATE TABLE Retailers (

    RetailerId INT PRIMARY KEY IDENTITY,

    Username NVARCHAR(50),

    Password NVARCHAR(50),

    Email NVARCHAR(100),

    Address NVARCHAR(200),

    PhoneNumber NVARCHAR(20)

)
 
CREATE TABLE Admins (

    AdminId INT PRIMARY KEY IDENTITY,

    Username NVARCHAR(50),

    Password NVARCHAR(50),

    Email NVARCHAR(100),

    Address NVARCHAR(200),

    PhoneNumber NVARCHAR(20)

)

 select * from users
 select * from Admins
 select * from Retailers


 --Categories table
CREATE TABLE Categories (
    CategoryID INT PRIMARY KEY IDENTITY(1,1),
    CategoryName VARCHAR(100) UNIQUE
);
 
 
--Products table
CREATE TABLE Products (
    ProductID INT PRIMARY KEY IDENTITY(1,1),
    RetailerID INT,
    CategoryID INT,
    Name VARCHAR(100),
    Description VARCHAR(MAX),
    Price DECIMAL(10,2),
    QuantityAvailable INT,
    CreatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (RetailerID) REFERENCES Retailers(RetailerID),
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
);
 
 
CREATE TABLE ProductImage (
    ImageID INT PRIMARY KEY IDENTITY(1,1),
    ProductID INT NOT NULL,
    ImageData VARBINARY(MAX) NULL,
    AltText VARCHAR(255) NULL,
    ImageType VARCHAR(50) NULL,
    UploadedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);
 
 
--Cart table
CREATE TABLE Cart (
    CartID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT,
    ProductID INT,
    Quantity INT,
    AddedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);
 
 
--Wishlist table
CREATE TABLE Wishlist (
    WishlistID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT,
    ProductID INT,
    AddedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);
 
 
--Compare table
CREATE TABLE Compare (
    CompareID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT,
    ProductID INT,
    AddedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);
 
 
--Orders table
CREATE TABLE Orders (
    OrderID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT,
    TotalAmount DECIMAL(10,2),
    OrderDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);
 
 
--OrderDetails Table
CREATE TABLE OrderDetails (
    OrderDetailID INT PRIMARY KEY IDENTITY(1,1),
    OrderID INT,
    ProductID INT,
    Quantity INT,
    PriceAtPurchase DECIMAL(10,2),
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);
 
 
--ShippingDetails Table
CREATE TABLE ShippingDetails (
    ShippingID INT PRIMARY KEY IDENTITY(1,1),
    OrderID INT,
    Status VARCHAR(50),
    TrackingNumber VARCHAR(100),
    ShippingMethod VARCHAR(50),
    EstimatedDeliveryDate DATE,
    LastUpdated DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID)
);
 
 
--Reviews Table
CREATE TABLE Reviews (
    ReviewID INT PRIMARY KEY IDENTITY(1,1),
    ProductID INT,
    UserID INT,
    Rating INT CHECK (Rating BETWEEN 1 AND 5),
    Comment VARCHAR(MAX),
    CreatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);
 
 
-- Sample Categories
INSERT INTO Categories (CategoryName) VALUES 
('Electronics'), 
('Fashion'), 
('Home & Kitchen');
 
-- Sample Products
INSERT INTO Products (RetailerID, CategoryID, Name, Description, Price, QuantityAvailable) VALUES 
(1, 1, 'Wireless Headphones', 'Noise-cancelling over-ear headphones', 2999.00, 50),
(2, 2, 'Cotton T-Shirt', 'Comfortable round-neck t-shirt', 499.00, 200);
 
 
-- Sample Cart Items
INSERT INTO Cart (UserID, ProductID, Quantity) VALUES 
(1, 1, 1),
(2, 2, 2);
 
-- Sample Wishlist Items
INSERT INTO Wishlist (UserID, ProductID) VALUES 
(1, 2),
(2, 1);
 
-- Sample Compare Items
INSERT INTO Compare (UserID, ProductID) VALUES 
(1, 1),
(1, 2);
 
-- Sample Orders
INSERT INTO Orders (UserID, TotalAmount) VALUES 
(1, 3498.00);
 
-- Sample Order Details
INSERT INTO OrderDetails (OrderID, ProductID, Quantity, PriceAtPurchase) VALUES 
(1, 1, 1, 2999.00),
(1, 2, 1, 499.00);
 
-- Sample Shipping Details
INSERT INTO ShippingDetails (OrderID, Status, TrackingNumber, ShippingMethod, EstimatedDeliveryDate) VALUES 
(1, 'Shipped', 'TRK123456789', 'CourierX', '2025-09-05');
 
-- Sample Reviews
INSERT INTO Reviews (ProductID, UserID, Rating, Comment) VALUES 
(1, 1, 5, 'Amazing sound quality!'),
(2, 2, 4, 'Good fabric, fits well.');
 
 
select * from Orders
select * from Products


ALTER TABLE Users ADD IsActive BIT NOT NULL DEFAULT 1;




select * from Users
select * from Retailers
select * from Admins
update users set IsActive =1 where UserId=8



ALTER TABLE Retailers
DROP CONSTRAINT DF__Retailers__IsAct__151B244E

ALTER TABLE Retailers
DROP COLUMN IsActive;


ALTER TABLE Admins
DROP CONSTRAINT DF__Admins__IsActive__160F4887;

ALTER TABLE Admins
DROP COLUMN IsActive;

ALTER TABLE Retailers
ADD IsApproved BIT NOT NULL DEFAULT 0;
select * from Retailers