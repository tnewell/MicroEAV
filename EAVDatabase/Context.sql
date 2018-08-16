CREATE TABLE [dbo].[Context]
(
	[Context_ID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(768) NOT NULL, 
    [Data_Name] NVARCHAR(256) NULL, 
    [Display_Text] NVARCHAR(1024) NULL, 
    CONSTRAINT [UK_Name] UNIQUE ([Name]), 
    CONSTRAINT [UK_DataName] UNIQUE ([Data_Name])
)
