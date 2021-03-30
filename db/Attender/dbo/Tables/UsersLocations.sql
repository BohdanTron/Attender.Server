CREATE TABLE [dbo].[UsersLocations] (
    [UserId]     INT NOT NULL,
    [LocationId] INT NOT NULL,
    CONSTRAINT [PK_UsersLocations] PRIMARY KEY CLUSTERED ([UserId] ASC, [LocationId] ASC),
    CONSTRAINT [FK_UsersLocations_Locations] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Locations] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UsersLocations_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);

