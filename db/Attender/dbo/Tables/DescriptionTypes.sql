CREATE TABLE [dbo].[DescriptionTypes]
(
	[Id] INT NOT NULL,
    [Type] VARCHAR(50) NOT NULL,
	
	CONSTRAINT [PK_DescriptionTypes] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [UC_DescriptionTypes_Type] UNIQUE NONCLUSTERED ([Type] ASC)
)
