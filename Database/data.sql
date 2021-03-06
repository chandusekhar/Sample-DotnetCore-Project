USE [coreaccesscontrol]
GO
DELETE FROM [dbo].[UserActivity]
GO
DELETE FROM [dbo].[ChangeEmailRequest]
GO
DELETE FROM [dbo].[UserPermission]
GO
DELETE FROM [dbo].[UserLocation]
GO
DELETE FROM [dbo].[UserStatus]
GO
DELETE FROM [dbo].[UserKeyMapping]
GO
DELETE FROM [dbo].[User]
GO
DELETE FROM [dbo].[KeyholderSpace]
GO
DELETE FROM [dbo].[KeyholderDevice]
GO
DELETE FROM [dbo].[KeyHolder]
GO
DELETE FROM [dbo].[KeyholderStatus]
GO
DELETE FROM [dbo].[DeviceSpace]
GO
DELETE FROM [dbo].[Space]
GO
DELETE FROM [dbo].[SpaceStatus]
GO
DELETE FROM [dbo].[Device]
GO
DELETE FROM [dbo].[Devicestatus]
GO
DELETE FROM [dbo].[Location]
GO
SET IDENTITY_INSERT [dbo].[Location] ON 

INSERT [dbo].[Location] ([Id], [Name]) VALUES (1, N'Location1')
INSERT [dbo].[Location] ([Id], [Name]) VALUES (2, N'Topcoder Owner1')
SET IDENTITY_INSERT [dbo].[Location] OFF
GO
SET IDENTITY_INSERT [dbo].[Devicestatus] ON 

INSERT [dbo].[Devicestatus] ([Id], [Name], [Description], [IsActive], [IsDefault], [LastUpdatedBy], [LastUpdatedOn], [CreatedBy], [CreatedOn], [LocationId]) VALUES (1, N'Active', N'Active', 1, 1, 9, CAST(N'2020-06-13T20:03:28.0000000' AS DateTime2), 9, CAST(N'2020-06-13T19:29:58.0000000' AS DateTime2), 1)
SET IDENTITY_INSERT [dbo].[Devicestatus] OFF
GO
SET IDENTITY_INSERT [dbo].[Device] ON 

INSERT [dbo].[Device] ([Id], [Name], [SerialNumber], [LocationId], [StatusId], [DeviceName_ID]) VALUES (1, N'Back Door', N'44491288', 2, 1, 5796)
INSERT [dbo].[Device] ([Id], [Name], [SerialNumber], [LocationId], [StatusId], [DeviceName_ID]) VALUES (2, N'East Side Door', N'44491288', 2, 1, 5794)
INSERT [dbo].[Device] ([Id], [Name], [SerialNumber], [LocationId], [StatusId], [DeviceName_ID]) VALUES (3, N'Front Door', N'44491288', 2, 1, 5797)
INSERT [dbo].[Device] ([Id], [Name], [SerialNumber], [LocationId], [StatusId], [DeviceName_ID]) VALUES (4, N'Outbuilding Padlock', N'44491288', 2, 1, 5793)
INSERT [dbo].[Device] ([Id], [Name], [SerialNumber], [LocationId], [StatusId], [DeviceName_ID]) VALUES (5, N'Test Arc', N'44491288', 2, 1, 5811)
INSERT [dbo].[Device] ([Id], [Name], [SerialNumber], [LocationId], [StatusId], [DeviceName_ID]) VALUES (6, N'West Side Door', N'44491288', 2, 1, 5795)
SET IDENTITY_INSERT [dbo].[Device] OFF
GO
SET IDENTITY_INSERT [dbo].[SpaceStatus] ON 

INSERT [dbo].[SpaceStatus] ([Id], [Name], [Description], [IsActive], [IsDefault], [LastUpdatedBy], [LastUpdatedOn], [CreatedBy], [CreatedOn], [LocationId]) VALUES (3, N'Active', N'Active', 1, 1, NULL, CAST(N'2020-06-13T13:37:32.0000000' AS DateTime2), 1, CAST(N'2020-06-13T13:37:32.0000000' AS DateTime2), 2)
SET IDENTITY_INSERT [dbo].[SpaceStatus] OFF
GO
SET IDENTITY_INSERT [dbo].[Space] ON 

INSERT [dbo].[Space] ([Id], [Name], [LocationId], [State], [StatusId], [CreatedBy], [CreatedOn], [LastUpdatedBy], [LastUpdatedOn]) VALUES (2, N'Apartment1', 2, 1, 3, 1, CAST(N'2020-06-13T13:37:32.0000000' AS DateTime2), 1, CAST(N'2020-06-13T13:37:32.0000000' AS DateTime2))
SET IDENTITY_INSERT [dbo].[Space] OFF
GO
SET IDENTITY_INSERT [dbo].[DeviceSpace] ON 

INSERT [dbo].[DeviceSpace] ([Id], [SpaceId], [DeviceId]) VALUES (1, 2, 1)
INSERT [dbo].[DeviceSpace] ([Id], [SpaceId], [DeviceId]) VALUES (2, 2, 2)
INSERT [dbo].[DeviceSpace] ([Id], [SpaceId], [DeviceId]) VALUES (3, 2, 3)
INSERT [dbo].[DeviceSpace] ([Id], [SpaceId], [DeviceId]) VALUES (4, 2, 4)
INSERT [dbo].[DeviceSpace] ([Id], [SpaceId], [DeviceId]) VALUES (5, 2, 5)
INSERT [dbo].[DeviceSpace] ([Id], [SpaceId], [DeviceId]) VALUES (6, 2, 6)
SET IDENTITY_INSERT [dbo].[DeviceSpace] OFF
GO
SET IDENTITY_INSERT [dbo].[KeyholderStatus] ON 

INSERT [dbo].[KeyholderStatus] ([Id], [Name], [Description], [IsActive], [IsDefault], [LastUpdatedBy], [LastUpdatedOn], [CreatedBy], [CreatedOn], [LocationId]) VALUES (1, N'Active', N'Active', 1, 1, 9, CAST(N'2020-06-13T13:37:32.0000000' AS DateTime2), 9, CAST(N'2020-06-13T13:37:32.0000000' AS DateTime2), 2)
SET IDENTITY_INSERT [dbo].[KeyholderStatus] OFF
GO
SET IDENTITY_INSERT [dbo].[KeyHolder] ON 

INSERT [dbo].[KeyHolder] ([Id], [StatusId], [LocationId], [KeySerialNumber], [Name], [State], [Pin]) VALUES (1, NULL, 1, N'1234', N'1234', 1, N'1234')
INSERT [dbo].[KeyHolder] ([Id], [StatusId], [LocationId], [KeySerialNumber], [Name], [State], [Pin]) VALUES (2, 1, 2, N'50062584', N'A0420861', 1, N'1234')
INSERT [dbo].[KeyHolder] ([Id], [StatusId], [LocationId], [KeySerialNumber], [Name], [State], [Pin]) VALUES (3, 1, 2, N'50062591', N'K042076', 1, N'1234')
INSERT [dbo].[KeyHolder] ([Id], [StatusId], [LocationId], [KeySerialNumber], [Name], [State], [Pin]) VALUES (4, 1, 2, N'50062592', N'K042075', 1, N'1234')
INSERT [dbo].[KeyHolder] ([Id], [StatusId], [LocationId], [KeySerialNumber], [Name], [State], [Pin]) VALUES (5, 1, 2, N'50062593', N'K042071', 1, N'1234')
INSERT [dbo].[KeyHolder] ([Id], [StatusId], [LocationId], [KeySerialNumber], [Name], [State], [Pin]) VALUES (6, 1, 2, N'50062594', N'K042070', 1, N'1234')
INSERT [dbo].[KeyHolder] ([Id], [StatusId], [LocationId], [KeySerialNumber], [Name], [State], [Pin]) VALUES (7, 1, 2, N'50062595', N'K042068', 1, N'1234')
INSERT [dbo].[KeyHolder] ([Id], [StatusId], [LocationId], [KeySerialNumber], [Name], [State], [Pin]) VALUES (8, 1, 2, N'50062596', N'K042066', 1, N'1234')
INSERT [dbo].[KeyHolder] ([Id], [StatusId], [LocationId], [KeySerialNumber], [Name], [State], [Pin]) VALUES (9, 1, 2, N'50062597', N'K042072', 1, N'1234')
INSERT [dbo].[KeyHolder] ([Id], [StatusId], [LocationId], [KeySerialNumber], [Name], [State], [Pin]) VALUES (10, 1, 2, N'50062598', N'A042072', 1, N'1234')
INSERT [dbo].[KeyHolder] ([Id], [StatusId], [LocationId], [KeySerialNumber], [Name], [State], [Pin]) VALUES (11, 1, 2, N'50062599', N'A042070', 1, N'1234')
INSERT [dbo].[KeyHolder] ([Id], [StatusId], [LocationId], [KeySerialNumber], [Name], [State], [Pin]) VALUES (12, 1, 2, N'50062600', N'A042066', 1, N'1234')
INSERT [dbo].[KeyHolder] ([Id], [StatusId], [LocationId], [KeySerialNumber], [Name], [State], [Pin]) VALUES (13, 1, 2, N'50062603', N'44491288', 1, N'1234')
INSERT [dbo].[KeyHolder] ([Id], [StatusId], [LocationId], [KeySerialNumber], [Name], [State], [Pin]) VALUES (14, 1, 2, N'50062611', N'test v', 1, N'1234')
SET IDENTITY_INSERT [dbo].[KeyHolder] OFF
GO
SET IDENTITY_INSERT [dbo].[KeyholderSpace] ON 

INSERT [dbo].[KeyholderSpace] ([Id], [KeyHolderId], [SpaceId]) VALUES (1, 2, 2)
INSERT [dbo].[KeyholderSpace] ([Id], [KeyHolderId], [SpaceId]) VALUES (2, 3, 2)
INSERT [dbo].[KeyholderSpace] ([Id], [KeyHolderId], [SpaceId]) VALUES (3, 4, 2)
INSERT [dbo].[KeyholderSpace] ([Id], [KeyHolderId], [SpaceId]) VALUES (4, 5, 2)
INSERT [dbo].[KeyholderSpace] ([Id], [KeyHolderId], [SpaceId]) VALUES (5, 6, 2)
INSERT [dbo].[KeyholderSpace] ([Id], [KeyHolderId], [SpaceId]) VALUES (6, 7, 2)
INSERT [dbo].[KeyholderSpace] ([Id], [KeyHolderId], [SpaceId]) VALUES (7, 8, 2)
INSERT [dbo].[KeyholderSpace] ([Id], [KeyHolderId], [SpaceId]) VALUES (8, 9, 2)
INSERT [dbo].[KeyholderSpace] ([Id], [KeyHolderId], [SpaceId]) VALUES (10, 10, 2)
INSERT [dbo].[KeyholderSpace] ([Id], [KeyHolderId], [SpaceId]) VALUES (11, 11, 2)
INSERT [dbo].[KeyholderSpace] ([Id], [KeyHolderId], [SpaceId]) VALUES (12, 12, 2)
INSERT [dbo].[KeyholderSpace] ([Id], [KeyHolderId], [SpaceId]) VALUES (13, 13, 2)
INSERT [dbo].[KeyholderSpace] ([Id], [KeyHolderId], [SpaceId]) VALUES (14, 14, 2)
SET IDENTITY_INSERT [dbo].[KeyholderSpace] OFF
GO
SET IDENTITY_INSERT [dbo].[User] ON 

INSERT [dbo].[User] ([Id], [Name], [Email], [PasswordHash], [SecurityQuestion], [SecurityQuestionAnswer], [VerificationToken], [VerificationTokenExpiry], [IsEmailVerified], [IsTemporaryPassword], [LastUpdatedBy], [LastUpdatedOn]) VALUES (9, N'Admin', N'admin@admin.com', N'Mg1Nu8VhuFb6oPbvAXgsWpF+bdXQKT1VGbGYRFGadkrW/tLpefR3A9nuV2156wuhQ1uCez6IZxD08x/gmTYxvA==', N'test security question', N'test security answer', N'', CAST(N'2020-06-13T21:37:21.0000000' AS DateTime2), 1, 0, 9, CAST(N'2020-06-12T21:37:29.0000000' AS DateTime2))
INSERT [dbo].[User] ([Id], [Name], [Email], [PasswordHash], [SecurityQuestion], [SecurityQuestionAnswer], [VerificationToken], [VerificationTokenExpiry], [IsEmailVerified], [IsTemporaryPassword], [LastUpdatedBy], [LastUpdatedOn]) VALUES (10, N'user1', N'user1@lexu4g.com', N'C8/hgjH2BTCJjtfzoQtUq9mf/N9kzv74+tCfNV/Rz4QcPBjYDwV72jqHrPFbCsL+lJkkDWChRlb8M8D0nzXh4w==', NULL, NULL, N'56256e7a-f534-4c6c-bc92-ddd0babfe3c3', CAST(N'2020-06-14T11:59:29.0000000' AS DateTime2), 1, 1, 9, CAST(N'2020-06-13T12:31:05.0000000' AS DateTime2))
INSERT [dbo].[User] ([Id], [Name], [Email], [PasswordHash], [SecurityQuestion], [SecurityQuestionAnswer], [VerificationToken], [VerificationTokenExpiry], [IsEmailVerified], [IsTemporaryPassword], [LastUpdatedBy], [LastUpdatedOn]) VALUES (11, N'user2', N'user2@lexu4g.com', N'895/CexM8k70/fNpykQ0m69E0x+fyWoY9dR37864nMGGRjGb7WQo/MmvvuH2TLu7GqGMBsqwdRWyc0crS3ceuQ==', NULL, NULL, N'49f5ecf8-565b-43ac-a4ba-a2d5892307fd', CAST(N'2020-06-14T12:36:19.0000000' AS DateTime2), 1, 1, 9, CAST(N'2020-06-13T12:36:19.0000000' AS DateTime2))
INSERT [dbo].[User] ([Id], [Name], [Email], [PasswordHash], [SecurityQuestion], [SecurityQuestionAnswer], [VerificationToken], [VerificationTokenExpiry], [IsEmailVerified], [IsTemporaryPassword], [LastUpdatedBy], [LastUpdatedOn]) VALUES (12, N'user3', N'user3@lexu4g.com', N'iTvJYt6I+dWA9WDBQ0A8gkRICaPvsKB3W5qXZNpFlqTiJgH744YxGoBRxoZbJZ9mlmSn0bAf1Zz5nWhdfAhJjQ==', NULL, NULL, N'4b9497ea-ad13-476d-a2eb-c4cbf8120f8b', CAST(N'2020-06-14T12:39:25.0000000' AS DateTime2), 1, 1, 9, CAST(N'2020-06-13T13:33:47.0000000' AS DateTime2))
SET IDENTITY_INSERT [dbo].[User] OFF
GO
SET IDENTITY_INSERT [dbo].[UserKeyMapping] ON 

INSERT [dbo].[UserKeyMapping] ([Id], [UserId], [LocationId], [KeySerialNumber], [AppliedOn]) VALUES (1, 12, 1, N'1234', CAST(N'2020-06-13T12:39:25.0000000' AS DateTime2))
SET IDENTITY_INSERT [dbo].[UserKeyMapping] OFF
GO
SET IDENTITY_INSERT [dbo].[UserStatus] ON 

INSERT [dbo].[UserStatus] ([Id], [Name], [Description], [IsActive], [IsDefault], [LastUpdatedBy], [LastUpdatedOn], [CreatedBy], [CreatedOn], [LocationId]) VALUES (1, N'Active', N'Active', 1, 1, 9, CAST(N'2020-06-13T12:52:06.0000000' AS DateTime2), 9, CAST(N'2020-06-13T12:52:06.0000000' AS DateTime2), 1)
SET IDENTITY_INSERT [dbo].[UserStatus] OFF
GO
SET IDENTITY_INSERT [dbo].[UserLocation] ON 

INSERT [dbo].[UserLocation] ([Id], [UserId], [LocationId], [CreatedBy], [CreatedOn], [IsToolKitEnabled], [StatusId], [LastUpdatedBy], [LastUpdatedOn], [State], [DisabledReason]) VALUES (1, 9, 2, 9, CAST(N'2020-06-13T11:35:18.0000000' AS DateTime2), 0, 1, 9, CAST(N'2020-06-13T11:35:18.0000000' AS DateTime2), 1, NULL)
SET IDENTITY_INSERT [dbo].[UserLocation] OFF
GO
SET IDENTITY_INSERT [dbo].[UserPermission] ON 

INSERT [dbo].[UserPermission] ([Id], [UserLocationId], [HasAdminRead], [HasAdminEdit], [HasKeyholderRead], [HasKeyholderEdit], [HasDeviceRead], [HasDeviceEdit], [HasSpaceRead], [HasSpaceEdit], [HasConfigRead], [HasConfigEdit], [LastUpdatedBy], [LastUpdatedOn]) VALUES (1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 9, CAST(N'2020-06-13T11:38:11.0000000' AS DateTime2))
SET IDENTITY_INSERT [dbo].[UserPermission] OFF
GO
SET IDENTITY_INSERT [dbo].[ChangeEmailRequest] ON 

INSERT [dbo].[ChangeEmailRequest] ([Id], [UserId], [Email], [RequestedOn], [VerificationToken], [VerificationTokenExpiry]) VALUES (1, 9, N'nowshad2@lexu4g.com', CAST(N'2020-06-12T21:37:21.0000000' AS DateTime2), N'9be41445-c105-453b-a1d9-49c74afe091d', CAST(N'2020-06-13T21:37:21.0000000' AS DateTime2))
INSERT [dbo].[ChangeEmailRequest] ([Id], [UserId], [Email], [RequestedOn], [VerificationToken], [VerificationTokenExpiry]) VALUES (2, 9, N'user4@lexu4g.com', CAST(N'2020-06-13T13:27:38.0000000' AS DateTime2), N'4392fc1b-056c-4e43-8f16-2df202030e7d', CAST(N'2020-06-14T13:27:38.0000000' AS DateTime2))
SET IDENTITY_INSERT [dbo].[ChangeEmailRequest] OFF
GO
SET IDENTITY_INSERT [dbo].[UserActivity] ON 

INSERT [dbo].[UserActivity] ([Id], [UserId], [ActivityText], [LocationId], [ActivityTime]) VALUES (1, 12, N'a', 1, CAST(N'2020-06-13T13:37:32.0000000' AS DateTime2))
INSERT [dbo].[UserActivity] ([Id], [UserId], [ActivityText], [LocationId], [ActivityTime]) VALUES (2, 12, N'ab', 1, CAST(N'2020-06-13T13:37:32.0000000' AS DateTime2))
INSERT [dbo].[UserActivity] ([Id], [UserId], [ActivityText], [LocationId], [ActivityTime]) VALUES (3, 12, N'c', 1, CAST(N'2020-06-13T13:37:32.0000000' AS DateTime2))
INSERT [dbo].[UserActivity] ([Id], [UserId], [ActivityText], [LocationId], [ActivityTime]) VALUES (4, 12, N'd', 1, CAST(N'2020-06-13T13:37:32.0000000' AS DateTime2))
INSERT [dbo].[UserActivity] ([Id], [UserId], [ActivityText], [LocationId], [ActivityTime]) VALUES (5, 12, N'adfklgkldfjb', 1, CAST(N'2020-06-13T13:37:32.0000000' AS DateTime2))
INSERT [dbo].[UserActivity] ([Id], [UserId], [ActivityText], [LocationId], [ActivityTime]) VALUES (6, 12, N'akhrjhjrheb', 1, CAST(N'2020-06-13T13:37:32.0000000' AS DateTime2))
SET IDENTITY_INSERT [dbo].[UserActivity] OFF
GO
