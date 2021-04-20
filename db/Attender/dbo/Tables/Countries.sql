CREATE TABLE [dbo].[Countries] (
    [Id]   INT          IDENTITY (1, 1) NOT NULL,
    [Name] VARCHAR (50) NOT NULL,
    [Code] VARCHAR(2) NOT NULL, 
    [Supported] BIT NOT NULL DEFAULT 0, 
    [Longitude] DECIMAL(18, 6) NULL, 
    [Latitude] DECIMAL(18, 6) NULL, 
    CONSTRAINT [PK_Countries] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UC_Countries_Name] UNIQUE NONCLUSTERED ([Name] ASC)
);

