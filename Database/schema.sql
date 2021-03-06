USE [coreaccesscontrol]
GO
/****** Object:  StoredProcedure [dbo].[AdminSearch]    Script Date: 6/15/2020 5:13:05 PM ******/
DROP PROCEDURE [dbo].[AdminSearch]
GO
ALTER TABLE [dbo].[UserStatus] DROP CONSTRAINT [FK_locationId1]
GO
ALTER TABLE [dbo].[UserPermission] DROP CONSTRAINT [fk_userlocation_id_permission_id]
GO
ALTER TABLE [dbo].[UserLocation] DROP CONSTRAINT [fk_UserStatus]
GO
ALTER TABLE [dbo].[UserLocation] DROP CONSTRAINT [FK_UserLocationMap_Location]
GO
ALTER TABLE [dbo].[UserLocation] DROP CONSTRAINT [FK_UserLocation_User]
GO
ALTER TABLE [dbo].[UserKeyMapping] DROP CONSTRAINT [FK_UserToolkit_Location]
GO
ALTER TABLE [dbo].[UserKeyMapping] DROP CONSTRAINT [fk_UserKeyMapping]
GO
ALTER TABLE [dbo].[UserKeyMapping] DROP CONSTRAINT [fk_user]
GO
ALTER TABLE [dbo].[UserActivity] DROP CONSTRAINT [fk_UserLocation_Location]
GO
ALTER TABLE [dbo].[SpaceStatus] DROP CONSTRAINT [FK_locationId0]
GO
ALTER TABLE [dbo].[Space] DROP CONSTRAINT [fk_SpaceStatus]
GO
ALTER TABLE [dbo].[Space] DROP CONSTRAINT [fk_Space_location]
GO
ALTER TABLE [dbo].[KeyholderStatus] DROP CONSTRAINT [FK_locationId00]
GO
ALTER TABLE [dbo].[KeyholderSpace] DROP CONSTRAINT [FK_KeyholderSpace_Space0]
GO
ALTER TABLE [dbo].[KeyholderSpace] DROP CONSTRAINT [fk_KeyholderSpace_Keyholder]
GO
ALTER TABLE [dbo].[KeyholderDevice] DROP CONSTRAINT [fk_KeyholderSpace_Keyholder0]
GO
ALTER TABLE [dbo].[KeyholderDevice] DROP CONSTRAINT [FK_KeyholderDevice_Device]
GO
ALTER TABLE [dbo].[KeyHolder] DROP CONSTRAINT [fk_status_KeyholderStatus]
GO
ALTER TABLE [dbo].[KeyHolder] DROP CONSTRAINT [fk_Location]
GO
ALTER TABLE [dbo].[Devicestatus] DROP CONSTRAINT [FK_locationId]
GO
ALTER TABLE [dbo].[DeviceSpace] DROP CONSTRAINT [FK_DeviceSpace_Space]
GO
ALTER TABLE [dbo].[DeviceSpace] DROP CONSTRAINT [fk_DeviceSpace_Device]
GO
ALTER TABLE [dbo].[Device] DROP CONSTRAINT [fk_device_status]
GO
ALTER TABLE [dbo].[Device] DROP CONSTRAINT [fk_device_location]
GO
ALTER TABLE [dbo].[ChangeEmailRequest] DROP CONSTRAINT [FK_ChangeEmailRequest_User]
GO
/****** Object:  Table [dbo].[UserStatus]    Script Date: 6/15/2020 5:13:05 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserStatus]') AND type in (N'U'))
DROP TABLE [dbo].[UserStatus]
GO
/****** Object:  Table [dbo].[UserPermission]    Script Date: 6/15/2020 5:13:05 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserPermission]') AND type in (N'U'))
DROP TABLE [dbo].[UserPermission]
GO
/****** Object:  Table [dbo].[UserLocation]    Script Date: 6/15/2020 5:13:05 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserLocation]') AND type in (N'U'))
DROP TABLE [dbo].[UserLocation]
GO
/****** Object:  Table [dbo].[UserKeyMapping]    Script Date: 6/15/2020 5:13:05 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserKeyMapping]') AND type in (N'U'))
DROP TABLE [dbo].[UserKeyMapping]
GO
/****** Object:  Table [dbo].[UserActivity]    Script Date: 6/15/2020 5:13:05 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserActivity]') AND type in (N'U'))
DROP TABLE [dbo].[UserActivity]
GO
/****** Object:  Table [dbo].[User]    Script Date: 6/15/2020 5:13:05 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[User]') AND type in (N'U'))
DROP TABLE [dbo].[User]
GO
/****** Object:  Table [dbo].[SpaceStatus]    Script Date: 6/15/2020 5:13:05 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SpaceStatus]') AND type in (N'U'))
DROP TABLE [dbo].[SpaceStatus]
GO
/****** Object:  Table [dbo].[Space]    Script Date: 6/15/2020 5:13:05 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Space]') AND type in (N'U'))
DROP TABLE [dbo].[Space]
GO
/****** Object:  Table [dbo].[Location]    Script Date: 6/15/2020 5:13:05 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Location]') AND type in (N'U'))
DROP TABLE [dbo].[Location]
GO
/****** Object:  Table [dbo].[KeyholderStatus]    Script Date: 6/15/2020 5:13:05 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[KeyholderStatus]') AND type in (N'U'))
DROP TABLE [dbo].[KeyholderStatus]
GO
/****** Object:  Table [dbo].[KeyholderSpace]    Script Date: 6/15/2020 5:13:05 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[KeyholderSpace]') AND type in (N'U'))
DROP TABLE [dbo].[KeyholderSpace]
GO
/****** Object:  Table [dbo].[KeyholderDevice]    Script Date: 6/15/2020 5:13:05 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[KeyholderDevice]') AND type in (N'U'))
DROP TABLE [dbo].[KeyholderDevice]
GO
/****** Object:  Table [dbo].[KeyHolder]    Script Date: 6/15/2020 5:13:05 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[KeyHolder]') AND type in (N'U'))
DROP TABLE [dbo].[KeyHolder]
GO
/****** Object:  Table [dbo].[Devicestatus]    Script Date: 6/15/2020 5:13:05 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Devicestatus]') AND type in (N'U'))
DROP TABLE [dbo].[Devicestatus]
GO
/****** Object:  Table [dbo].[DeviceSpace]    Script Date: 6/15/2020 5:13:05 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeviceSpace]') AND type in (N'U'))
DROP TABLE [dbo].[DeviceSpace]
GO
/****** Object:  Table [dbo].[Device]    Script Date: 6/15/2020 5:13:05 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Device]') AND type in (N'U'))
DROP TABLE [dbo].[Device]
GO
/****** Object:  Table [dbo].[ChangeEmailRequest]    Script Date: 6/15/2020 5:13:05 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ChangeEmailRequest]') AND type in (N'U'))
DROP TABLE [dbo].[ChangeEmailRequest]
GO
/****** Object:  Table [dbo].[ChangeEmailRequest]    Script Date: 6/15/2020 5:13:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChangeEmailRequest](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[Email] [varchar](256) NOT NULL,
	[RequestedOn] [datetime2](0) NOT NULL,
	[VerificationToken] [varchar](45) NULL,
	[VerificationTokenExpiry] [datetime2](0) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Device]    Script Date: 6/15/2020 5:13:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Device](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[SerialNumber] [varchar](10) NULL,
	[LocationId] [bigint] NOT NULL,
	[StatusId] [bigint] NULL,
	[DeviceName_ID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DeviceSpace]    Script Date: 6/15/2020 5:13:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DeviceSpace](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[SpaceId] [bigint] NOT NULL,
	[DeviceId] [bigint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Devicestatus]    Script Date: 6/15/2020 5:13:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Devicestatus](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](500) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IsDefault] [bit] NOT NULL,
	[LastUpdatedBy] [bigint] NULL,
	[LastUpdatedOn] [datetime2](0) NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[LocationId] [bigint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KeyHolder]    Script Date: 6/15/2020 5:13:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KeyHolder](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[StatusId] [bigint] NULL,
	[LocationId] [bigint] NOT NULL,
	[KeySerialNumber] [varchar](10) NOT NULL,
	[Name] [varchar](200) NULL,
	[State] [int] NULL,
	[Pin] [char](4) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [KeySerialNumber_UNIQUE] UNIQUE NONCLUSTERED 
(
	[KeySerialNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KeyholderDevice]    Script Date: 6/15/2020 5:13:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KeyholderDevice](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[KeyHolderId] [bigint] NOT NULL,
	[DeviceId] [bigint] NOT NULL,
	[KeyDevicePermissionId] [bigint] NOT NULL,
	[Type] [int] NULL,
	[SpaceId] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KeyholderSpace]    Script Date: 6/15/2020 5:13:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KeyholderSpace](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[KeyHolderId] [bigint] NOT NULL,
	[SpaceId] [bigint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KeyholderStatus]    Script Date: 6/15/2020 5:13:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KeyholderStatus](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](500) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IsDefault] [bit] NOT NULL,
	[LastUpdatedBy] [bigint] NULL,
	[LastUpdatedOn] [datetime2](0) NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[LocationId] [bigint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Location]    Script Date: 6/15/2020 5:13:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Location](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK__Location__3214EC071ED998B2] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Space]    Script Date: 6/15/2020 5:13:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Space](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[LocationId] [bigint] NOT NULL,
	[State] [int] NULL,
	[StatusId] [bigint] NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[LastUpdatedBy] [bigint] NULL,
	[LastUpdatedOn] [datetime2](0) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SpaceStatus]    Script Date: 6/15/2020 5:13:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SpaceStatus](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](500) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IsDefault] [bit] NOT NULL,
	[LastUpdatedBy] [bigint] NULL,
	[LastUpdatedOn] [datetime2](0) NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[LocationId] [bigint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 6/15/2020 5:13:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](250) NULL,
	[Email] [varchar](250) NOT NULL,
	[PasswordHash] [nvarchar](256) NOT NULL,
	[SecurityQuestion] [varchar](200) NULL,
	[SecurityQuestionAnswer] [varchar](200) NULL,
	[VerificationToken] [varchar](45) NULL,
	[VerificationTokenExpiry] [datetime2](0) NULL,
	[IsEmailVerified] [bit] NULL,
	[IsTemporaryPassword] [bit] NULL,
	[LastUpdatedBy] [bigint] NULL,
	[LastUpdatedOn] [datetime2](0) NULL,
 CONSTRAINT [PK__User__3214EC07060DEAE8] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [Email_UNIQUE] UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserActivity]    Script Date: 6/15/2020 5:13:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserActivity](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[ActivityText] [varchar](250) NOT NULL,
	[LocationId] [bigint] NOT NULL,
	[ActivityTime] [datetime2](0) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserKeyMapping]    Script Date: 6/15/2020 5:13:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserKeyMapping](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[LocationId] [bigint] NOT NULL,
	[KeySerialNumber] [varchar](10) NOT NULL,
	[AppliedOn] [datetime2](0) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserLocation]    Script Date: 6/15/2020 5:13:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserLocation](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[LocationId] [bigint] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[IsToolKitEnabled] [bit] NOT NULL,
	[StatusId] [bigint] NULL,
	[LastUpdatedBy] [bigint] NULL,
	[LastUpdatedOn] [datetime2](0) NULL,
	[State] [int] NOT NULL,
	[DisabledReason] [varchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserPermission]    Script Date: 6/15/2020 5:13:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserPermission](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserLocationId] [bigint] NOT NULL,
	[HasAdminRead] [bit] NULL,
	[HasAdminEdit] [bit] NULL,
	[HasKeyholderRead] [bit] NULL,
	[HasKeyholderEdit] [bit] NULL,
	[HasDeviceRead] [bit] NULL,
	[HasDeviceEdit] [bit] NULL,
	[HasSpaceRead] [bit] NULL,
	[HasSpaceEdit] [bit] NULL,
	[HasConfigRead] [bit] NULL,
	[HasConfigEdit] [bit] NULL,
	[LastUpdatedBy] [bigint] NOT NULL,
	[LastUpdatedOn] [datetime2](0) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserStatus]    Script Date: 6/15/2020 5:13:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserStatus](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsDefault] [bit] NOT NULL,
	[LastUpdatedBy] [bigint] NULL,
	[LastUpdatedOn] [datetime2](0) NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedOn] [datetime2](0) NOT NULL,
	[LocationId] [bigint] NOT NULL,
 CONSTRAINT [PK__UserStat__3214EC073C69FB99] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ChangeEmailRequest]  WITH CHECK ADD  CONSTRAINT [FK_ChangeEmailRequest_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[ChangeEmailRequest] CHECK CONSTRAINT [FK_ChangeEmailRequest_User]
GO
ALTER TABLE [dbo].[Device]  WITH CHECK ADD  CONSTRAINT [fk_device_location] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Device] CHECK CONSTRAINT [fk_device_location]
GO
ALTER TABLE [dbo].[Device]  WITH CHECK ADD  CONSTRAINT [fk_device_status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Devicestatus] ([Id])
GO
ALTER TABLE [dbo].[Device] CHECK CONSTRAINT [fk_device_status]
GO
ALTER TABLE [dbo].[DeviceSpace]  WITH CHECK ADD  CONSTRAINT [fk_DeviceSpace_Device] FOREIGN KEY([DeviceId])
REFERENCES [dbo].[Device] ([Id])
GO
ALTER TABLE [dbo].[DeviceSpace] CHECK CONSTRAINT [fk_DeviceSpace_Device]
GO
ALTER TABLE [dbo].[DeviceSpace]  WITH CHECK ADD  CONSTRAINT [FK_DeviceSpace_Space] FOREIGN KEY([SpaceId])
REFERENCES [dbo].[Space] ([Id])
GO
ALTER TABLE [dbo].[DeviceSpace] CHECK CONSTRAINT [FK_DeviceSpace_Space]
GO
ALTER TABLE [dbo].[Devicestatus]  WITH CHECK ADD  CONSTRAINT [FK_locationId] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Devicestatus] CHECK CONSTRAINT [FK_locationId]
GO
ALTER TABLE [dbo].[KeyHolder]  WITH CHECK ADD  CONSTRAINT [fk_Location] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[KeyHolder] CHECK CONSTRAINT [fk_Location]
GO
ALTER TABLE [dbo].[KeyHolder]  WITH CHECK ADD  CONSTRAINT [fk_status_KeyholderStatus] FOREIGN KEY([StatusId])
REFERENCES [dbo].[KeyholderStatus] ([Id])
GO
ALTER TABLE [dbo].[KeyHolder] CHECK CONSTRAINT [fk_status_KeyholderStatus]
GO
ALTER TABLE [dbo].[KeyholderDevice]  WITH CHECK ADD  CONSTRAINT [FK_KeyholderDevice_Device] FOREIGN KEY([DeviceId])
REFERENCES [dbo].[Device] ([Id])
GO
ALTER TABLE [dbo].[KeyholderDevice] CHECK CONSTRAINT [FK_KeyholderDevice_Device]
GO
ALTER TABLE [dbo].[KeyholderDevice]  WITH CHECK ADD  CONSTRAINT [fk_KeyholderSpace_Keyholder0] FOREIGN KEY([KeyHolderId])
REFERENCES [dbo].[KeyHolder] ([Id])
GO
ALTER TABLE [dbo].[KeyholderDevice] CHECK CONSTRAINT [fk_KeyholderSpace_Keyholder0]
GO
ALTER TABLE [dbo].[KeyholderSpace]  WITH CHECK ADD  CONSTRAINT [fk_KeyholderSpace_Keyholder] FOREIGN KEY([KeyHolderId])
REFERENCES [dbo].[KeyHolder] ([Id])
GO
ALTER TABLE [dbo].[KeyholderSpace] CHECK CONSTRAINT [fk_KeyholderSpace_Keyholder]
GO
ALTER TABLE [dbo].[KeyholderSpace]  WITH CHECK ADD  CONSTRAINT [FK_KeyholderSpace_Space0] FOREIGN KEY([SpaceId])
REFERENCES [dbo].[Space] ([Id])
GO
ALTER TABLE [dbo].[KeyholderSpace] CHECK CONSTRAINT [FK_KeyholderSpace_Space0]
GO
ALTER TABLE [dbo].[KeyholderStatus]  WITH CHECK ADD  CONSTRAINT [FK_locationId00] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[KeyholderStatus] CHECK CONSTRAINT [FK_locationId00]
GO
ALTER TABLE [dbo].[Space]  WITH CHECK ADD  CONSTRAINT [fk_Space_location] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Space] CHECK CONSTRAINT [fk_Space_location]
GO
ALTER TABLE [dbo].[Space]  WITH CHECK ADD  CONSTRAINT [fk_SpaceStatus] FOREIGN KEY([StatusId])
REFERENCES [dbo].[SpaceStatus] ([Id])
GO
ALTER TABLE [dbo].[Space] CHECK CONSTRAINT [fk_SpaceStatus]
GO
ALTER TABLE [dbo].[SpaceStatus]  WITH CHECK ADD  CONSTRAINT [FK_locationId0] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[SpaceStatus] CHECK CONSTRAINT [FK_locationId0]
GO
ALTER TABLE [dbo].[UserActivity]  WITH CHECK ADD  CONSTRAINT [fk_UserLocation_Location] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[UserActivity] CHECK CONSTRAINT [fk_UserLocation_Location]
GO
ALTER TABLE [dbo].[UserKeyMapping]  WITH CHECK ADD  CONSTRAINT [fk_user] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[UserKeyMapping] CHECK CONSTRAINT [fk_user]
GO
ALTER TABLE [dbo].[UserKeyMapping]  WITH CHECK ADD  CONSTRAINT [fk_UserKeyMapping] FOREIGN KEY([KeySerialNumber])
REFERENCES [dbo].[KeyHolder] ([KeySerialNumber])
GO
ALTER TABLE [dbo].[UserKeyMapping] CHECK CONSTRAINT [fk_UserKeyMapping]
GO
ALTER TABLE [dbo].[UserKeyMapping]  WITH CHECK ADD  CONSTRAINT [FK_UserToolkit_Location] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[UserKeyMapping] CHECK CONSTRAINT [FK_UserToolkit_Location]
GO
ALTER TABLE [dbo].[UserLocation]  WITH CHECK ADD  CONSTRAINT [FK_UserLocation_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[UserLocation] CHECK CONSTRAINT [FK_UserLocation_User]
GO
ALTER TABLE [dbo].[UserLocation]  WITH CHECK ADD  CONSTRAINT [FK_UserLocationMap_Location] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[UserLocation] CHECK CONSTRAINT [FK_UserLocationMap_Location]
GO
ALTER TABLE [dbo].[UserLocation]  WITH CHECK ADD  CONSTRAINT [fk_UserStatus] FOREIGN KEY([StatusId])
REFERENCES [dbo].[UserStatus] ([Id])
GO
ALTER TABLE [dbo].[UserLocation] CHECK CONSTRAINT [fk_UserStatus]
GO
ALTER TABLE [dbo].[UserPermission]  WITH CHECK ADD  CONSTRAINT [fk_userlocation_id_permission_id] FOREIGN KEY([UserLocationId])
REFERENCES [dbo].[UserLocation] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserPermission] CHECK CONSTRAINT [fk_userlocation_id_permission_id]
GO
ALTER TABLE [dbo].[UserStatus]  WITH CHECK ADD  CONSTRAINT [FK_locationId1] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[UserStatus] CHECK CONSTRAINT [FK_locationId1]
GO
/****** Object:  StoredProcedure [dbo].[AdminSearch]    Script Date: 6/15/2020 5:13:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AdminSearch]
	-- Add the parameters for the stored procedure here
	@id varchar(max) = NULL,
	@onlyToolKitUser bit = NULL,
	@name varchar(max) = NULL,
	@email varchar(max) = NULL,
	@state bigint = NULL,
	@statusid bigint = NULL,
	@skip int = 0,
	@take int = 5,
	@orderby varchar(max) = NULL,
	@orderdir varchar(max) = NULL

AS
BEGIN
	DECLARE @where varchar(max) = ''
	DECLARE @order varchar(max) = ''
	DECLARE @order1 varchar(max) = ''
	DECLARE @select varchar(max) = ''
	DECLARE @sql nvarchar(max) = ''
	DECLARE @from varchar(max) = ''

	IF (@orderdir IS NOT NULL)
	BEGIN
	SET @orderdir =
	CASE WHEN @orderdir = 'asc' THEN @orderdir
	WHEN @orderdir = 'desc' THEN @orderdir
	ELSE 'asc'
	END
	END

	IF (@orderdir IS NULL)
	BEGIN
	SET @orderdir = 'asc'
	END



BEGIN
SET @order+=
CASE 
WHEN @orderby = 'id' THEN  ' ORDER BY U.Id ' + @orderdir
WHEN @orderby = 'name' THEN  ' ORDER BY U.NAME '+ @orderdir
WHEN @orderby = 'state' THEN  ' ORDER BY UL.State '+ @orderdir
WHEN @orderby = 'statusid' THEN  ' ORDER BY US.Id '+ @orderdir
WHEN @orderby = 'disabledreason' THEN  ' ORDER BY UL.DisabledReason '+ @orderdir
WHEN @orderby = 'statusname' THEN  ' ORDER BY US.Name '+ @orderdir
WHEN @orderby = 'keyholderid' THEN  ' ORDER BY KH.Id '+ @orderdir
WHEN @orderby = 'keyserialnumber' THEN  ' ORDER BY KH.KeySerialNumber '+ @orderdir
WHEN @orderby = 'pin' THEN  ' ORDER BY KH.Pin '+ @orderdir
WHEN @orderby = 'activitytext' THEN  ' ORDER BY UA.ActivityText '+ @orderdir
WHEN @orderby = 'activitytime' THEN  ' ORDER BY UA.ActivityTime '+ @orderdir
ELSE ' ORDER BY U.ID '
END
END

BEGIN
SET @order1+=
CASE 
WHEN @orderby = 'id' THEN  ' ORDER BY UserId ' + @orderdir
WHEN @orderby = 'name' THEN  ' ORDER BY NAME '+ @orderdir
WHEN @orderby = 'state' THEN  ' ORDER BY State '+ @orderdir
WHEN @orderby = 'statusid' THEN  ' ORDER BY StatusId '+ @orderdir
WHEN @orderby = 'disabledreason' THEN  ' ORDER BY UL.DisabledReason '+ @orderdir
WHEN @orderby = 'statusname' THEN  ' ORDER BY StatusName '+ @orderdir
WHEN @orderby = 'keyholderid' THEN  ' ORDER BY KeyHolderId '+ @orderdir
WHEN @orderby = 'keyserialnumber' THEN  ' ORDER BY KeySerialNumber '+ @orderdir
WHEN @orderby = 'pin' THEN  ' ORDER BY Pin '+ @orderdir
WHEN @orderby = 'activitytext' THEN  ' ORDER BY UA.ActivityText '+ @orderdir
WHEN @orderby = 'activitytime' THEN  ' ORDER BY UA.ActivityTime '+ @orderdir
ELSE ' ORDER BY UserId '
END
END

	SET @select +=  
     N'SELECT ROW_NUMBER() OVER ('+@order+') row,
	 U.Id UserId, U.Name, U.Email, UL.State, UL.LocationId, UL.DisabledReason, US.Id as StatusId, US.Name AS StatusName, 
	 KH.Id as KeyHolderId, KH.KeySerialNumber, KH.Pin, UP.*, UA.Id UserActivityId, UA.ActivityText, UA.ActivityTime'

	IF (NOT @id IS NULL)
	BEGIN
	SET @where += N' U.Id = ' + @id + ' and'
	END
	IF (NOT @name IS NULL)
	BEGIN
	SET @where += N' U.NAME like ''%' + @name + '%'' and'
	SET @where += N' US.NAME like ''%' + @name + '%'' and'
	END
	IF (NOT @email IS NULL)
	BEGIN
	SET @where += N' U.EMAIL like ''%' + @email + '%'' and'
	END
	IF (NOT @state IS NULL)
	BEGIN
	SET @where += N' UL.STATE = ' + @state+ ' and'
	END
	IF (NOT @statusid IS NULL)
	BEGIN
	SET @where += N' US.Id =' + @statusid + ' and'
	END

SET @from = N'
	FROM 
		[User] U		
INNER JOIN UserLocation UL on UL.UserId = U.Id'
IF (NOT @onlyToolKitUser IS NULL)
BEGIN
SET @from+= ' INNER JOIN UserKeyMapping UKM on U.Id = UKM.UserId and UKM.LocationId = UL.LocationId 
INNER JOIN KeyHolder KH on KH.KeySerialNumber = UKM.KeySerialNumber'
END
IF (@onlyToolKitUser IS NULL)
BEGIN
SET @from+= ' LEFT OUTER JOIN UserKeyMapping UKM on U.Id = UKM.UserId and UKM.LocationId = UL.LocationId 
LEFT OUTER JOIN KeyHolder KH on KH.KeySerialNumber = UKM.KeySerialNumber
'
END

SET @from+= 'LEFT OUTER JOIN UserStatus US on UL.StatusId = US.Id
LEFT OUTER JOIN UserPermission UP on UP.UserLocationId = UL.Id
LEFT OUTER JOIN 
(
	SELECT TOP 1.* FROM UserActivity ORDER BY ActivityTime desc
) UA on UA.UserId = U.Id and UA.LocationId = UL.LocationId '

IF (@where != '')
BEGIN 
SET @where = ' where ' + @where + ' 1 = 1'
END

SET @sql += @select + @from + @where
DECLARE @sql1 nvarchar(max) = ''
PRINT(@sql)

SET @sql1 += N'SELECT TOP ' +CAST(@take AS nvarchar)+' T.* FROM ( (' + @sql + ')T
INNER JOIN [User] ON [User].Id = T.Userid
) WHERE row > '+ CAST(@skip AS nvarchar) + @order1

PRINT(@sql1)

EXEC sp_executesql @sql1
--BEGIN
--sp_executesql(@sql)
--END

--PRINT(@sql1)

END
GO
