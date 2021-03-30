CREATE TABLE [dbo].[UsersEvents] (
    [UserId]  INT NOT NULL,
    [EventId] INT NOT NULL,
    CONSTRAINT [PK_UsersEvents] PRIMARY KEY CLUSTERED ([UserId] ASC, [EventId] ASC),
    CONSTRAINT [FK_UsersEvents_Events] FOREIGN KEY ([EventId]) REFERENCES [dbo].[Events] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UsersEvents_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);

