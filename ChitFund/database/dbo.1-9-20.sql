CREATE TABLE [dbo].[Table]
(
	[ticketNo] INT NOT NULL, 
    [chitGroup] VARCHAR(20) NOT NULL, 
    [prizeStatus] VARCHAR(20) NULL, 
    [name] VARCHAR(20) NOT NULL, 
    [month] INT NULL, 
    [bidAmount] INT NULL, 
    [date] DATE NULL, 
    [amountPaid] INT NULL, 
    [balance] INT NULL, 
    [total] INT NULL 
)
CREATE TABLE [dbo].[Table] 
( 
    [ticketNo] INT NULL, 
    [prizeStatus] VARCHAR(20) NULL DEFAULT '',
    [auctionMonth] INT NULL DEFAULT 0,
    [name] VARCHAR(20) NULL, 
    [month] INT NULL DEFAULT 1, 
    [bidAmount] INT NULL DEFAULT '', 
    [date] VARCHAR(20) NULL DEFAULT '',
    [amountPaid] INT NULL DEFAULT '',
    [balance] INT NULL DEFAULT '', 
    [total] INT NULL DEFAULT ''
)
CREATE TABLE [dbo].[" + chitname + "] (  [ticketNo] INT NULL, " +
                                " [prizeStatus] VARCHAR(20) NULL DEFAULT '', " +
                                " [auctionMonth] INT NULL DEFAULT 0," +
                                " [name] VARCHAR(20) NULL, " +
                                " [month] INT NULL DEFAULT 1, " +
                                " [bidAmount] INT NULL DEFAULT '', " +
                                " [date] VARCHAR(20) NULL DEFAULT '', " +
                                " [amountPaid] INT NULL DEFAULT '', " +
                                " [balance] INT NULL DEFAULT '', " +
                                " [total] INT NULL DEFAULT '');