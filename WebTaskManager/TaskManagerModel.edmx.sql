
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/08/2016 22:30:43
-- Generated from EDMX file: D:\projects\WebTaskManager\WebTaskManager\TaskManagerModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [DBTaskManager];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK__Category__Catego__267ABA7A]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Category] DROP CONSTRAINT [FK__Category__Catego__267ABA7A];
GO
IF OBJECT_ID(N'[DBTaskManagerModelStoreContainer].[FK__LoseAutho__UserI__22AA2996]', 'F') IS NOT NULL
    ALTER TABLE [DBTaskManagerModelStoreContainer].[LoseAuthorization] DROP CONSTRAINT [FK__LoseAutho__UserI__22AA2996];
GO
IF OBJECT_ID(N'[dbo].[FK__Priority__Priori__25869641]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Priority] DROP CONSTRAINT [FK__Priority__Priori__25869641];
GO
IF OBJECT_ID(N'[dbo].[FK__Task__UserId__21B6055D]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Task] DROP CONSTRAINT [FK__Task__UserId__21B6055D];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Category]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Category];
GO
IF OBJECT_ID(N'[dbo].[Priority]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Priority];
GO
IF OBJECT_ID(N'[dbo].[Task]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Task];
GO
IF OBJECT_ID(N'[dbo].[User]', 'U') IS NOT NULL
    DROP TABLE [dbo].[User];
GO
IF OBJECT_ID(N'[DBTaskManagerModelStoreContainer].[LoseAuthorization]', 'U') IS NOT NULL
    DROP TABLE [DBTaskManagerModelStoreContainer].[LoseAuthorization];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Category'
CREATE TABLE [dbo].[Category] (
    [CategoryId] int  NOT NULL,
    [CategoryName] nvarchar(20)  NOT NULL
);
GO

-- Creating table 'Priority'
CREATE TABLE [dbo].[Priority] (
    [PriorityId] int  NOT NULL,
    [PriorityName] nvarchar(20)  NOT NULL
);
GO

-- Creating table 'Task'
CREATE TABLE [dbo].[Task] (
    [TaskId] int  NOT NULL,
    [UserId] int  NOT NULL,
    [TaskName] nvarchar(30)  NOT NULL,
    [PriorityId] int  NOT NULL,
    [CategoryId] int  NULL,
    [FullDescription] nvarchar(300)  NULL,
    [SetDate] datetime  NOT NULL,
    [IsPerformance] int  NULL,
    [SpentTime] time  NULL
);
GO

-- Creating table 'User'
CREATE TABLE [dbo].[User] (
    [UserId] int  NOT NULL,
    [Login] varchar(15)  NOT NULL,
    [HashPassword] varchar(128)  NOT NULL,
    [Salt] varchar(128)  NOT NULL,
    [Name] nvarchar(30)  NOT NULL,
    [Email] varchar(30)  NOT NULL,
    [BirthDate] datetime  NULL
);
GO

-- Creating table 'LoseAuthorization'
CREATE TABLE [dbo].[LoseAuthorization] (
    [UserId] int  NOT NULL,
    [LoseTime] datetime  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [CategoryId] in table 'Category'
ALTER TABLE [dbo].[Category]
ADD CONSTRAINT [PK_Category]
    PRIMARY KEY CLUSTERED ([CategoryId] ASC);
GO

-- Creating primary key on [PriorityId] in table 'Priority'
ALTER TABLE [dbo].[Priority]
ADD CONSTRAINT [PK_Priority]
    PRIMARY KEY CLUSTERED ([PriorityId] ASC);
GO

-- Creating primary key on [TaskId] in table 'Task'
ALTER TABLE [dbo].[Task]
ADD CONSTRAINT [PK_Task]
    PRIMARY KEY CLUSTERED ([TaskId] ASC);
GO

-- Creating primary key on [UserId] in table 'User'
ALTER TABLE [dbo].[User]
ADD CONSTRAINT [PK_User]
    PRIMARY KEY CLUSTERED ([UserId] ASC);
GO

-- Creating primary key on [UserId], [LoseTime] in table 'LoseAuthorization'
ALTER TABLE [dbo].[LoseAuthorization]
ADD CONSTRAINT [PK_LoseAuthorization]
    PRIMARY KEY CLUSTERED ([UserId], [LoseTime] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [UserId] in table 'Task'
ALTER TABLE [dbo].[Task]
ADD CONSTRAINT [FK__Task__UserId__21B6055D]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[User]
        ([UserId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Task__UserId__21B6055D'
CREATE INDEX [IX_FK__Task__UserId__21B6055D]
ON [dbo].[Task]
    ([UserId]);
GO

-- Creating foreign key on [UserId] in table 'LoseAuthorization'
ALTER TABLE [dbo].[LoseAuthorization]
ADD CONSTRAINT [FK__LoseAutho__UserI__22AA2996]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[User]
        ([UserId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [CategoryId] in table 'Category'
ALTER TABLE [dbo].[Category]
ADD CONSTRAINT [FK__Category__Catego__267ABA7A]
    FOREIGN KEY ([CategoryId])
    REFERENCES [dbo].[Task]
        ([TaskId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [PriorityId] in table 'Priority'
ALTER TABLE [dbo].[Priority]
ADD CONSTRAINT [FK__Priority__Priori__25869641]
    FOREIGN KEY ([PriorityId])
    REFERENCES [dbo].[Task]
        ([TaskId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------