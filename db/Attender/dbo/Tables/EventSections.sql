﻿CREATE TABLE [dbo].[EventSections]
(
	[Id]	INT				IDENTITY(1, 1) NOT NULL,
	[Name]	VARCHAR(250)	NOT NULL,

	CONSTRAINT [PK_EventSections] PRIMARY KEY CLUSTERED ([Id] ASC)
)