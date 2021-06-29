CREATE TABLE [dbo].[CategoryDescriptions]
(
	[Id] INT IDENTITY(1, 1) NOT NULL,
    [Text] NVARCHAR(500) NOT NULL,
    [LanguageId] INT NOT NULL,
    [CategoryId] INT NOT NULL,

    CONSTRAINT [PK_CategoryDescriptions] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CategoryDescriptions_Languages] FOREIGN KEY ([LanguageId]) REFERENCES [dbo].[Languages] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_CategoryDescriptions_Categories] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Categories] ([Id]) ON DELETE CASCADE
)
