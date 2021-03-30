CREATE TABLE [dbo].[SubCategories] (
    [Id]         INT          IDENTITY (1, 1) NOT NULL,
    [Name]       VARCHAR (50) NOT NULL,
    [CategoryId] INT          NOT NULL,
    CONSTRAINT [PK_SubCategories] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SubCategories_Categories] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Categories] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [UC_SubCategories_Name] UNIQUE NONCLUSTERED ([Name] ASC)
);

