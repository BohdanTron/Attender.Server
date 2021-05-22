CREATE TABLE [dbo].[UsersCities] (
    [UserId] INT NOT NULL,
    [CityId] INT NOT NULL,
    CONSTRAINT [PK_UsersCities] PRIMARY KEY CLUSTERED ([UserId] ASC, [CityId] ASC),
    CONSTRAINT [FK_UsersCities_Cities] FOREIGN KEY ([CityId]) REFERENCES [dbo].[Cities] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UsersCities_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);
GO

CREATE UNIQUE NONCLUSTERED INDEX [IDX_UsersCities_UserId_CityId]
    ON [dbo].[UsersCities]([UserId], [CityId]);
GO