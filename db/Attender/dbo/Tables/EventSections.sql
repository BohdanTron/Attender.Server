CREATE TABLE [dbo].[EventSections]
(
	[RowId]			INT	IDENTITY(1, 1)	NOT NULL,
	[Id]			INT					NOT NULL,
	[Name]			NVARCHAR(250)		NOT NULL,
	[LanguageId]	INT					NOT NULL,

	CONSTRAINT [PK_EventSections] PRIMARY KEY CLUSTERED ([RowId] ASC),
	CONSTRAINT [FK_EventSections_Languages] FOREIGN KEY ([LanguageId]) REFERENCES [dbo].[Languages] ([Id]) ON DELETE CASCADE
)
