﻿CREATE TABLE [dbo].[People]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FirstName] NVARCHAR(50) NOT NULL, 
    [LastName] NVARCHAR(50) NOT NULL, 
    [AddressId] INT NOT NULL, 
    [EmployerId] INT NOT NULL
)
