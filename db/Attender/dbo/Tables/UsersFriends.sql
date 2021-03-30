CREATE TABLE [dbo].[UsersFriends] (
    [UserId]   INT NOT NULL,
    [FriendId] INT NOT NULL,
    CONSTRAINT [PK_UsersFriends] PRIMARY KEY CLUSTERED ([UserId] ASC, [FriendId] ASC),
    CONSTRAINT [FK_UsersFriends_Users_FriendId] FOREIGN KEY ([FriendId]) REFERENCES [dbo].[Users] ([Id]),
    CONSTRAINT [FK_UsersFriends_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);

