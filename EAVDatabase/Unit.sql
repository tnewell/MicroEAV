CREATE TABLE [dbo].[Unit] (
    [Unit_ID]               INT            IDENTITY (1, 1) NOT NULL,
    [Singular_Name]         NVARCHAR (128) NULL,
    [Singular_Abbreviation] NVARCHAR (8)   NULL,
    [Plural_Name]           NVARCHAR (128) NULL,
    [Plural_Abbreviation]   NVARCHAR (8)   NULL,
    [Symbol]                NVARCHAR (4)   NULL,
    [Display_Text]          NVARCHAR (128) NULL,
    [Curated]               BIT            NOT NULL,
    CONSTRAINT [PK_Unit] PRIMARY KEY CLUSTERED ([Unit_ID] ASC)
);

