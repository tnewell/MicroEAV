CREATE TABLE [dbo].[Value]
(
	[Attribute_ID] INT NOT NULL , 
    [Instance_ID] INT NOT NULL, 
    [Raw_Value] NVARCHAR(MAX) NOT NULL, 
    [Units] NVARCHAR(32) NULL, 
    PRIMARY KEY ([Attribute_ID], [Instance_ID])
)
