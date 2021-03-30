CREATE TABLE [dbo].[RefreshTokens] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [UserId]        INT           NOT NULL,
    [Value]         VARCHAR (MAX) NOT NULL,
    [AccessTokenId] VARCHAR (MAX) NOT NULL,
    [Used]          BIT           NOT NULL,
    [AddedDate]     DATETIME      NOT NULL,
    [ExpiryDate]    DATETIME      NOT NULL,
    CONSTRAINT [PK_RefreshTokens] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_RefreshTokens_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);

