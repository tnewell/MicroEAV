CREATE TABLE [dbo].[Value] (
    [Attribute_ID] INT            NOT NULL,
    [Instance_ID]  INT            NOT NULL,
    [Raw_Value]    NVARCHAR (MAX) NOT NULL,
    [Unit_ID]      INT            NULL,
    CONSTRAINT [PK__Value__FA7B50EAA3568448] PRIMARY KEY CLUSTERED ([Attribute_ID] ASC, [Instance_ID] ASC),
    CONSTRAINT [FK_Value_Attribute] FOREIGN KEY ([Attribute_ID]) REFERENCES [dbo].[Attribute] ([Attribute_ID]),
    CONSTRAINT [FK_Value_Instance] FOREIGN KEY ([Instance_ID]) REFERENCES [dbo].[Instance] ([Instance_ID]),
    CONSTRAINT [FK_Value_Unit] FOREIGN KEY ([Unit_ID]) REFERENCES [dbo].[Unit] ([Unit_ID])
);


