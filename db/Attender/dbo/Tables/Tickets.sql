CREATE TABLE [dbo].[Tickets] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Price]       MONEY         NOT NULL,
    [OrderedDate] DATETIME2 (7) NULL,
    [UserId]      INT           NULL,
    [EventId]     INT           NOT NULL,
    CONSTRAINT [PK_Tickets] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Tickets_Events] FOREIGN KEY ([EventId]) REFERENCES [dbo].[Events] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Tickets_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);

