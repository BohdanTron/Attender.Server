CREATE TABLE [dbo].[Users] (
    [Id]            INT IDENTITY (1, 1) NOT NULL,
    [Email]         VARCHAR (50)        NULL,
    [UserName]      VARCHAR (25)        NOT NULL,
    [PhoneNumber]   VARCHAR (25)        NOT NULL,
    [AvatarId]      UNIQUEIDENTIFIER    NULL,
    [RoleId]        TINYINT             NOT NULL,
    [LanguageId]    INT                 NOT NULL,

    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Users_Roles] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Users_Languages] FOREIGN KEY ([LanguageId]) REFERENCES [dbo].[Languages] ON DELETE CASCADE,
    CONSTRAINT [UC_Users_PhoneNumber] UNIQUE NONCLUSTERED ([PhoneNumber] ASC),
    CONSTRAINT [UC_Users_UserName] UNIQUE NONCLUSTERED ([UserName] ASC)
);
GO

CREATE UNIQUE NONCLUSTERED INDEX [IDX_Email]
    ON [dbo].[Users]([Email] ASC) WHERE ([Email] IS NOT NULL);
