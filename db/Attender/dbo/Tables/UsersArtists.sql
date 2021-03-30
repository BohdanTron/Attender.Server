CREATE TABLE [dbo].[UsersArtists] (
    [UserId]   INT NOT NULL,
    [ArtistId] INT NOT NULL,
    CONSTRAINT [PK_UsersArtists] PRIMARY KEY CLUSTERED ([UserId] ASC, [ArtistId] ASC),
    CONSTRAINT [FK_UsersArtists_Artists] FOREIGN KEY ([ArtistId]) REFERENCES [dbo].[Artists] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UsersArtists_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);

