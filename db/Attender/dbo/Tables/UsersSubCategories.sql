CREATE TABLE [dbo].[UsersSubCategories] (
    [UserId]        INT NOT NULL,
    [SubCategoryId] INT NOT NULL,
    CONSTRAINT [PK_UsersSubCategories] PRIMARY KEY CLUSTERED ([UserId] ASC, [SubCategoryId] ASC),
    CONSTRAINT [FK_UsersSubCategories_SubCategories] FOREIGN KEY ([SubCategoryId]) REFERENCES [dbo].[SubCategories] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UsersSubCategories_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);

GO
CREATE UNIQUE NONCLUSTERED INDEX [IDX_UsersSubCategories_UserId_SubCategoryId]
    ON [dbo].[UsersSubCategories]([UserId], [SubCategoryId]);