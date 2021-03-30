CREATE TABLE [dbo].[SearchHistory] (
    [UserId] INT           NOT NULL,
    [Phrase] VARCHAR (MAX) NOT NULL,
    CONSTRAINT [FK_SearchHistory_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);

