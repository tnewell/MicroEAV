CREATE TABLE [dbo].[Instance]
(
	[Instance_ID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Parent_Instance_ID] INT NULL, 
    [Container_ID] INT NOT NULL, 
    [Subject_ID] INT NOT NULL
)
