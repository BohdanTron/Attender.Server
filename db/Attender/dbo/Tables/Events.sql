CREATE TABLE [dbo].[Events] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [Name]          VARCHAR (100) NOT NULL,
    [Description]   VARCHAR (MAX) NULL,
    [Date]          DATETIME      NOT NULL,
    [LocationId]    INT           NOT NULL,
    [ArtistId]      INT           NOT NULL,
    [SubCategoryId] INT           NOT NULL,
    [Promoted]      BIT           NOT NULL DEFAULT(0),
    [ImageURL] VARCHAR(250) NULL, 
    CONSTRAINT [PK_Events] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Events_Artists] FOREIGN KEY ([ArtistId]) REFERENCES [dbo].[Artists] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Events_Locations] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Locations] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Events_SubCategories] FOREIGN KEY ([SubCategoryId]) REFERENCES [dbo].[SubCategories] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [UC_Events_Name] UNIQUE NONCLUSTERED ([Name] ASC)
);

