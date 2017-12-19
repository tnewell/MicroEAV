CREATE TABLE [dbo].[Subject]
(
	[Subject_ID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Entity_ID] INT NOT NULL, 
    [Context_ID] INT NOT NULL, 
    [Identifier] NVARCHAR(256) NOT NULL, 
    CONSTRAINT [FK_Subject_Entity] FOREIGN KEY ([Entity_ID]) REFERENCES [Entity]([Entity_ID]), 
    CONSTRAINT [FK_Subject_Context] FOREIGN KEY ([Context_ID]) REFERENCES [Context]([Context_ID]), 
    CONSTRAINT [UK_Context_Identifier] UNIQUE ([Context_ID],[Identifier])
)
