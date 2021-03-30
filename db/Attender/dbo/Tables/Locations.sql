CREATE TABLE [dbo].[Locations] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (250) NOT NULL,
    [Description] VARCHAR (MAX) NULL,
    [CityId]      INT           NOT NULL,
    CONSTRAINT [PK_Locations] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Locations_Cities] FOREIGN KEY ([CityId]) REFERENCES [dbo].[Cities] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [UC_Locations_Name] UNIQUE NONCLUSTERED ([Name] ASC)
);

