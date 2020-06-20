/*
Run this script on a database with the schema represented by:

        SupraLogDB_full    -  This database will be modified. The scripts folder will not be modified.

to synchronize it with a database with the schema represented by:

        SupraLogDB

First, execute the create database command on the master database: create database SupraLogDB
Then, execute this script
*/
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Dropping [dbo].[WriteLog]'
GO
IF OBJECT_ID(N'[dbo].[WriteLog]', 'P') IS NOT NULL
DROP PROCEDURE [dbo].[WriteLog]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Dropping [dbo].[USP_GetLog]'
GO
IF OBJECT_ID(N'[dbo].[USP_GetLog]', 'P') IS NOT NULL
DROP PROCEDURE [dbo].[USP_GetLog]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Dropping [dbo].[USP_DBBuild_GetStaticDataTable]'
GO
IF OBJECT_ID(N'[dbo].[USP_DBBuild_GetStaticDataTable]', 'P') IS NOT NULL
DROP PROCEDURE [dbo].[USP_DBBuild_GetStaticDataTable]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Dropping [dbo].[USP_DBBuild_GetExistingStaticDataTable]'
GO
IF OBJECT_ID(N'[dbo].[USP_DBBuild_GetExistingStaticDataTable]', 'P') IS NOT NULL
DROP PROCEDURE [dbo].[USP_DBBuild_GetExistingStaticDataTable]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Dropping [dbo].[InsertCategoryLog]'
GO
IF OBJECT_ID(N'[dbo].[InsertCategoryLog]', 'P') IS NOT NULL
DROP PROCEDURE [dbo].[InsertCategoryLog]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Dropping [dbo].[ClearLogs]'
GO
IF OBJECT_ID(N'[dbo].[ClearLogs]', 'P') IS NOT NULL
DROP PROCEDURE [dbo].[ClearLogs]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Dropping [dbo].[AgeLogs]'
GO
IF OBJECT_ID(N'[dbo].[AgeLogs]', 'P') IS NOT NULL
DROP PROCEDURE [dbo].[AgeLogs]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Dropping [dbo].[AddCategory]'
GO
IF OBJECT_ID(N'[dbo].[AddCategory]', 'P') IS NOT NULL
DROP PROCEDURE [dbo].[AddCategory]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[sql_static_table]'
GO
IF OBJECT_ID(N'[dbo].[sql_static_table]', 'U') IS NULL
CREATE TABLE [dbo].[sql_static_table]
(
[TableName] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[EntryDate] [smalldatetime] NOT NULL,
[IsUpdated] [bit] NOT NULL CONSTRAINT [DF_sql_static_table_IsUpdated] DEFAULT ((1))
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating primary key [PK_sql_static_table] on [dbo].[sql_static_table]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_sql_static_table' AND object_id = OBJECT_ID(N'[dbo].[sql_static_table]'))
ALTER TABLE [dbo].[sql_static_table] ADD CONSTRAINT [PK_sql_static_table] PRIMARY KEY CLUSTERED  ([TableName])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[Category]'
GO
IF OBJECT_ID(N'[dbo].[Category]', 'U') IS NULL
CREATE TABLE [dbo].[Category]
(
[CategoryID] [int] NOT NULL IDENTITY(1, 1),
[CategoryName] [nvarchar] (64) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating primary key [PK_Categories] on [dbo].[Category]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_Categories' AND object_id = OBJECT_ID(N'[dbo].[Category]'))
ALTER TABLE [dbo].[Category] ADD CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED  ([CategoryID])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating index [IX_Category] on [dbo].[Category]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Category' AND object_id = OBJECT_ID(N'[dbo].[Category]'))
CREATE NONCLUSTERED INDEX [IX_Category] ON [dbo].[Category] ([CategoryName])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[AddCategory]'
GO
IF OBJECT_ID(N'[dbo].[AddCategory]', 'P') IS NULL
EXEC sp_executesql N'


CREATE PROCEDURE [dbo].[AddCategory]
	-- Add the parameters for the function here
	@CategoryName nvarchar(64),
	@LogID int
AS
BEGIN
	SET NOCOUNT ON;
    DECLARE @CatID INT
	SELECT @CatID = CategoryID FROM Category WHERE CategoryName = @CategoryName
	IF @CatID IS NULL
	BEGIN
		INSERT INTO Category (CategoryName) VALUES(@CategoryName)
		SELECT @CatID = @@IDENTITY
	END

	--EXEC InsertCategoryLog @CatID, @LogID 

	RETURN @CatID
END





'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[Log]'
GO
IF OBJECT_ID(N'[dbo].[Log]', 'U') IS NULL
CREATE TABLE [dbo].[Log]
(
[LogID] [int] NOT NULL IDENTITY(1, 1),
[EventID] [int] NULL,
[Priority] [int] NOT NULL,
[Severity] [nvarchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Title] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Timestamp] [datetime] NOT NULL,
[MachineName] [nvarchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AppDomainName] [nvarchar] (512) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ProcessID] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ProcessName] [nvarchar] (512) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ThreadName] [nvarchar] (512) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Win32ThreadId] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Message] [nvarchar] (1500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FormattedMessage] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[SessionId] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating primary key [PK_Log] on [dbo].[Log]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_Log' AND object_id = OBJECT_ID(N'[dbo].[Log]'))
ALTER TABLE [dbo].[Log] ADD CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED  ([LogID])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[AgeLogs]'
GO
IF OBJECT_ID(N'[dbo].[AgeLogs]', 'P') IS NULL
EXEC sp_executesql N'

---------------------------------------------------------------------
--cleanup log entries such that it leaves entries (Latest)
--than a number of records
--Input: @entriesOfMil - number of million of records to keep
--Change History:
--Engineer		Date		Changes
--Sean D		11/16/2010	Created
----------------------------------------------------------------------
CREATE PROCEDURE [dbo].[AgeLogs](@entriesOfMil int)
AS
BEGIN
	set transaction isolation level read uncommitted
	
	SET NOCOUNT ON;
	
	if @entriesOfMil <= 0 set @entriesOfMil = 1
	DECLARE @Date  datetime
	set @date = DateAdd(hh, -8, GETUTCDATE())
	--Do not allow cleanup during 8:00 PM through 8:00AM PST	
	if (DatePart(hh, @date)>=20 OR DatePart(hh, @date)<8)
	BEGIN
		RAISERROR(''Please do not run this procedure between 8:00 PM and 8:00AM'',16,1)
		return
	END
	set transaction isolation level read uncommitted
	declare @id int
	select @id = max(logid) from log
	set @id = @id - @entriesOfMil*1000000
	DELETE FROM [Log] where logid <  @id
END





'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[CategoryLog]'
GO
IF OBJECT_ID(N'[dbo].[CategoryLog]', 'U') IS NULL
CREATE TABLE [dbo].[CategoryLog]
(
[CategoryLogID] [int] NOT NULL IDENTITY(1, 1),
[CategoryID] [int] NOT NULL,
[LogID] [int] NOT NULL
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating primary key [PK_CategoryLog] on [dbo].[CategoryLog]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_CategoryLog' AND object_id = OBJECT_ID(N'[dbo].[CategoryLog]'))
ALTER TABLE [dbo].[CategoryLog] ADD CONSTRAINT [PK_CategoryLog] PRIMARY KEY CLUSTERED  ([CategoryLogID])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[ClearLogs]'
GO
IF OBJECT_ID(N'[dbo].[ClearLogs]', 'P') IS NULL
EXEC sp_executesql N'
CREATE PROCEDURE [dbo].[ClearLogs]
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM CategoryLog
	DELETE FROM [Log]
    DELETE FROM Category
END





'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[USP_DBBuild_GetExistingStaticDataTable]'
GO
IF OBJECT_ID(N'[dbo].[USP_DBBuild_GetExistingStaticDataTable]', 'P') IS NULL
EXEC sp_executesql N'
/************************************************************************  
  
     [dbo].[USP_DBBuild_GetExistingStaticDataTable] is used by build server to get   
     tables that have static data and needed to deployed.   
        
        
     Created By Y.Qi  06/17/2013         
  
*************************************************************************/  
  
Create Procedure [dbo].[USP_DBBuild_GetExistingStaticDataTable]
    
AS   
SET NOCOUNT ON  
  
declare @tables varchar(max)= ''''  
declare @tablename varchar(100)  
  
declare table_cursor cursor for  
select  tablename from sql_static_table where isupdated = 1  
  
open table_cursor  
fetch next from table_cursor into @tablename   
while @@FETCH_STATUS = 0  
begin  
    if exists ( select 1 from sysobjects where name = @tablename and type = ''U'')  
    begin  
  select @tables = @tables + @tablename + ''^|''   
 end   
 fetch next from table_cursor into @tablename   
end   
close table_cursor  
deallocate table_cursor  
if (LEN(@tables)> 2) 
begin
	select @tables = substring(@tables, 1, LEN(@tables)-2)  
	select @tables  
end






'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[USP_DBBuild_GetStaticDataTable]'
GO
IF OBJECT_ID(N'[dbo].[USP_DBBuild_GetStaticDataTable]', 'P') IS NULL
EXEC sp_executesql N'




/************************************************************************

     [dbo].[USP_DBBuild_GetStaticDataTable] is used by build server to get 
     tables that have static data and needed to deployed. 
      
     	
     Created By Y.Qi  01/11/2013       

*************************************************************************/

CREATE Procedure [dbo].[USP_DBBuild_GetStaticDataTable]
AS 

SET NOCOUNT ON

declare @tables varchar(max)
if exists (select 1 from sql_static_table) 
begin
    select @tables = ''''
	select @tables = @tables + tablename + ''^|'' from sql_static_table where isupdated = 1
	select @tables = substring(@tables, 1, LEN(@tables)-2)
	select @tables
end 



'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[USP_GetLog]'
GO
IF OBJECT_ID(N'[dbo].[USP_GetLog]', 'P') IS NULL
EXEC sp_executesql N'

-- =============================================
-- Author:		Y.Qi
-- Create date: 10-29-2012
-- Description:	gets error messages
-- Called By: Core API Test page
--Modified Data		Modified By		Comments
--28/2/2013			Vishnu			Fixed defect 6544
-- =============================================
CREATE PROCEDURE [dbo].[USP_GetLog]
	@SessionID NVarchar(50)
	,@RecordNumber int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
 
    SET ROWCOUNT   @RecordNumber
	SELECT     LogID, Timestamp as LogDate, MachineName, LEFT(Message, 300) AS LogMessage
	FROM       [Log] 
	WHERE      SessionId like ''%'' + @SessionID + ''%''
	AND severity = ''Error''
	ORDER BY LogID DESC
 

END


'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[WriteLog]'
GO
IF OBJECT_ID(N'[dbo].[WriteLog]', 'P') IS NULL
EXEC sp_executesql N'




/****** Object:  Stored Procedure dbo.WriteLog    Script Date: 10/1/2004 3:16:36 PM ******/

CREATE PROCEDURE [dbo].[WriteLog]
(
	@EventID int, 
	@Priority int, 
	@Severity nvarchar(32), 
	@Title nvarchar(256), 
	@Timestamp datetime,
	@MachineName nvarchar(32), 
	@AppDomainName nvarchar(512),
	@ProcessID nvarchar(256),
	@ProcessName nvarchar(512),
	@ThreadName nvarchar(512),
	@Win32ThreadId nvarchar(128),
	@Message nvarchar(1500),
	@FormattedMessage ntext,
	@sessionId nvarchar(32) = null,
	@LogId int OUTPUT
)
AS 

	INSERT INTO [Log] (
		EventID,
		Priority,
		Severity,
		Title,
		[Timestamp],
		MachineName,
		AppDomainName,
		ProcessID,
		ProcessName,
		ThreadName,
		Win32ThreadId,
		Message,
		--FormattedMessage,
		SessionId
	)
	VALUES (
		@EventID, 
		@Priority, 
		@Severity, 
		@Title, 
		@Timestamp,
		@MachineName, 
		@AppDomainName,
		@ProcessID,
		@ProcessName,
		@ThreadName,
		@Win32ThreadId,
		@Message,
		--@FormattedMessage,
		@sessionId
		)

	SET @LogID = @@IDENTITY
	RETURN @LogID












'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[ContextLogPriority]'
GO
IF OBJECT_ID(N'[dbo].[ContextLogPriority]', 'U') IS NULL
CREATE TABLE [dbo].[ContextLogPriority]
(
[LogPriorityID] [int] NOT NULL,
[LogPriorityName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating primary key [PK_ContextLogPriority] on [dbo].[ContextLogPriority]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_ContextLogPriority' AND object_id = OBJECT_ID(N'[dbo].[ContextLogPriority]'))
ALTER TABLE [dbo].[ContextLogPriority] ADD CONSTRAINT [PK_ContextLogPriority] PRIMARY KEY CLUSTERED  ([LogPriorityID])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[sql_version]'
GO
IF OBJECT_ID(N'[dbo].[sql_version]', 'U') IS NULL
CREATE TABLE [dbo].[sql_version]
(
[VersionID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[VersionDate] [datetime] NULL,
[VersionCreatedBy] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Project] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Developer] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Scripts] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Description] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating primary key [PK_sql_version] on [dbo].[sql_version]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'PK_sql_version' AND object_id = OBJECT_ID(N'[dbo].[sql_version]'))
ALTER TABLE [dbo].[sql_version] ADD CONSTRAINT [PK_sql_version] PRIMARY KEY CLUSTERED  ([VersionID])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[InsertCategoryLog]'
GO
IF OBJECT_ID(N'[dbo].[InsertCategoryLog]', 'P') IS NULL
EXEC sp_executesql N'
CREATE PROCEDURE [dbo].[InsertCategoryLog]
	@CategoryID INT,
	@LogID INT
AS
BEGIN
	SET NOCOUNT ON;
	return 0
	/*DECLARE @CatLogID INT
	SELECT @CatLogID FROM CategoryLog WHERE CategoryID=@CategoryID and LogID = @LogID
	IF @CatLogID IS NULL
	BEGIN
		INSERT INTO CategoryLog (CategoryID, LogID) VALUES(@CategoryID, @LogID)
		RETURN @@IDENTITY
	END
	ELSE RETURN @CatLogID*/
END



'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
DECLARE @Success AS BIT
SET @Success = 1
SET NOEXEC OFF
IF (@Success = 1) PRINT 'The database update succeeded'
ELSE BEGIN
	IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
	PRINT 'The database update failed'
END
GO
INSERT INTO [dbo].[sql_version] ([VersionID], [VersionDate], [VersionCreatedBy], [Project]) VALUES ('1.0.1.54757_Full', GetDate(), suser_name(), '-1.0.1.54757_159009')
GO
