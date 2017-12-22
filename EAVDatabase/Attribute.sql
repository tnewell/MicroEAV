CREATE TABLE [dbo].[Attribute]
(
	[Attribute_ID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Container_ID] INT NOT NULL, 
    [Data_Type_ID] INT NOT NULL, 
    [Name] NVARCHAR(1024) NOT NULL, 
    [Data_Name] NVARCHAR(256) NULL, 
    [Display_Text] NVARCHAR(1024) NULL, 
	[Sequence] INT NOT NULL, 
    [Is_Key] BIT NOT NULL, 
    CONSTRAINT [FK_Attribute_Container] FOREIGN KEY ([Container_ID]) REFERENCES [Container]([Container_ID]), 
    CONSTRAINT [FK_Attribute_DataType] FOREIGN KEY ([Data_Type_ID]) REFERENCES [Data_Type]([Data_Type_ID]), 
    CONSTRAINT [UK_Container_Name] UNIQUE ([Container_ID],[Name]), 
    CONSTRAINT [UK_Container_DataName] UNIQUE ([Container_ID],[Data_Name])
)
