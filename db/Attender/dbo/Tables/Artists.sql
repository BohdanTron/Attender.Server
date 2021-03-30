CREATE TABLE [dbo].[Artists] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (50)  NOT NULL,
    [Description] VARCHAR (MAX) NULL,
    CONSTRAINT [PK_Artists] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UC_Artists_Name] UNIQUE NONCLUSTERED ([Name] ASC)
);

