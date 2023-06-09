﻿CREATE TABLE [dbo].[Categories] (
    [Id]   INT          IDENTITY (1, 1) NOT NULL,
    [Name] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UC_Categories_Name] UNIQUE NONCLUSTERED ([Name] ASC)
);

