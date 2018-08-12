CREATE TABLE [dbo].[Instance] (
    [Instance_ID]        INT IDENTITY (1, 1) NOT NULL,
    [Parent_Instance_ID] INT NULL,
    [Container_ID]       INT NOT NULL,
    [Subject_ID]         INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Instance_ID] ASC),
    CONSTRAINT [FK_Instance_Instance] FOREIGN KEY ([Parent_Instance_ID]) REFERENCES [dbo].[Instance] ([Instance_ID]),
    CONSTRAINT [FK_Instance_Subject] FOREIGN KEY ([Subject_ID]) REFERENCES [dbo].[Subject] ([Subject_ID])
);


