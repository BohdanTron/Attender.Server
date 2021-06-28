CREATE TABLE [dbo].[Descriptions]
(
	[Id] INT IDENTITY(1, 1) NOT NULL,
    [Text] NVARCHAR(50) NOT NULL,
    [LanguageId] INT NOT NULL,
    [DescriptionTypeId] INT NOT NULL,

    CONSTRAINT [PK_Descriptions] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Descriptions_Languages] FOREIGN KEY ([LanguageId]) REFERENCES [dbo].[Languages] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Descriptions_DescriptionTypes] FOREIGN KEY ([DescriptionTypeId]) REFERENCES [dbo].[DescriptionTypes] ([Id]) ON DELETE CASCADE,
)
