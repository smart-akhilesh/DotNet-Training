CREATE DATABASE ElectricityBillDB

USE ElectricityBillDB;

CREATE TABLE ElectricityBill (
    consumer_number VARCHAR(20) PRIMARY KEY,
    consumer_name VARCHAR(50) NOT NULL,
    units_consumed INT NOT NULL,
    bill_amount FLOAT NOT NULL
);


select * from ElectricityBill


