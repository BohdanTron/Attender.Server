CREATE TABLE [dbo].[Countries] (
    [Id]   INT          IDENTITY (1, 1) NOT NULL,
    [Name] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Countries] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UC_Countries_Name] UNIQUE NONCLUSTERED ([Name] ASC)
);

