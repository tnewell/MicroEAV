CREATE TABLE [dbo].[Attribute_Unit] (
    [Attribute_ID] INT NOT NULL,
    [Unit_ID]      INT NOT NULL,
    CONSTRAINT [FK_Attribute_Unit_Attribute] FOREIGN KEY ([Attribute_ID]) REFERENCES [dbo].[Attribute] ([Attribute_ID]),
    CONSTRAINT [FK_Attribute_Unit_Unit] FOREIGN KEY ([Unit_ID]) REFERENCES [dbo].[Unit] ([Unit_ID]), 
    CONSTRAINT [PK_Attribute_Unit] PRIMARY KEY ([Attribute_ID], [Unit_ID])
);

