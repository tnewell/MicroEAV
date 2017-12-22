CREATE TABLE [dbo].[Container]
(
	[Container_ID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Parent_Container_ID] INT NULL, 
    [Context_ID] INT NOT NULL, 
    [Name] NVARCHAR(1024) NOT NULL, 
    [Data_Name] NVARCHAR(256) NULL, 
    [Display_Text] NVARCHAR(1024) NULL, 
	[Sequence] INT NOT NULL, 
    [Is_Repeating] BIT NOT NULL, 
    CONSTRAINT [FK_Container_Context] FOREIGN KEY ([Context_ID]) REFERENCES [Context]([Context_ID]), 
    CONSTRAINT [FK_ChildContainer_ParentContainer] FOREIGN KEY ([Parent_Container_ID]) REFERENCES [Container]([Container_ID]), 
    CONSTRAINT [UK_Context_Name] UNIQUE ([Context_ID],[Name]), 
    CONSTRAINT [UK_Context_DataName] UNIQUE ([Context_ID],[Data_Name])
)
