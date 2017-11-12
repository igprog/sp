USE [Spartacus]
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'AdminType'

GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'UserType'

GO
/****** Object:  Table [dbo].[Users]    Script Date: 3/5/2017 5:02:24 PM ******/
DROP TABLE [dbo].[Users]
GO
/****** Object:  Table [dbo].[Translations]    Script Date: 3/5/2017 5:02:24 PM ******/
DROP TABLE [dbo].[Translations]
GO
/****** Object:  Table [dbo].[Scheduler]    Script Date: 3/5/2017 5:02:24 PM ******/
DROP TABLE [dbo].[Scheduler]
GO
/****** Object:  Table [dbo].[Options]    Script Date: 3/5/2017 5:02:24 PM ******/
DROP TABLE [dbo].[Options]
GO
/****** Object:  Table [dbo].[ClientServices]    Script Date: 3/5/2017 5:02:24 PM ******/
DROP TABLE [dbo].[ClientServices]
GO
/****** Object:  Table [dbo].[Clients]    Script Date: 3/5/2017 5:02:24 PM ******/
DROP TABLE [dbo].[Clients]
GO
/****** Object:  Table [dbo].[CheckIn]    Script Date: 3/5/2017 5:02:24 PM ******/
DROP TABLE [dbo].[CheckIn]
GO
/****** Object:  Table [dbo].[CheckIn]    Script Date: 3/5/2017 5:02:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CheckIn](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClientId] [int] NOT NULL,
	[Service] [nvarchar](max) NULL,
	[CheckInTime] [datetime] NULL,
	[UserId] [int] NULL,
 CONSTRAINT [PK_CheckIn] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Clients]    Script Date: 3/5/2017 5:02:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clients](
	[ClientId] [int] IDENTITY(1,1000) NOT NULL,
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NULL,
	[Phone] [nvarchar](50) NULL,
	[BirthDate] [datetime] NULL,
	[IsActive] [int] NULL,
 CONSTRAINT [PK_Clients] PRIMARY KEY CLUSTERED 
(
	[ClientId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ClientServices]    Script Date: 3/5/2017 5:02:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClientServices](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClientId] [int] NOT NULL,
	[Service] [nvarchar](50) NULL,
	[Option] [nvarchar](50) NULL,
	[Quantity] [int] NULL,
	[Unit] [nvarchar](50) NULL,
	[Price] [nvarchar](50) NULL,
	[ActivationDate] [datetime] NULL,
	[ExpirationDate] [datetime] NULL,
	[QuantityLeft] [int] NULL,
 CONSTRAINT [PK_ClientServices] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Options]    Script Date: 3/5/2017 5:02:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Options](
	[Id] [uniqueidentifier] NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[Cod] [nvarchar](50) NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Options] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Scheduler]    Script Date: 3/5/2017 5:02:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Scheduler](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Content] [nvarchar](50) NULL,
	[StartDate] [bigint] NULL,
	[EndDate] [bigint] NULL,
	[Room] [int] NULL,
 CONSTRAINT [PK_Scheduler] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Translations]    Script Date: 3/5/2017 5:02:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Translations](
	[TranslationId] [uniqueidentifier] NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Language1] [nvarchar](50) NULL,
	[Language2] [nvarchar](50) NULL,
	[Language3] [nvarchar](50) NULL,
	[Language4] [nvarchar](50) NULL,
	[Language5] [nvarchar](50) NULL,
 CONSTRAINT [PK_Translations] PRIMARY KEY CLUSTERED 
(
	[TranslationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 3/5/2017 5:02:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,100) NOT NULL,
	[UserType] [int] NULL,
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[CompanyName] [nvarchar](50) NULL,
	[Address] [nvarchar](50) NULL,
	[PostalCode] [nvarchar](50) NULL,
	[City] [nvarchar](50) NULL,
	[Country] [nvarchar](50) NULL,
	[Pin] [nvarchar](50) NULL,
	[Phone] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](200) NOT NULL,
	[AdminType] [int] NULL,
	[UserGroupId] [int] NULL,
	[ActivationDate] [datetime] NULL,
	[ExpirationDate] [datetime] NULL,
	[IsActive] [int] NULL,
	[IPAddress] [nvarchar](50) NULL,
 CONSTRAINT [PK_Users_1] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[CheckIn] ON 

INSERT [dbo].[CheckIn] ([Id], [ClientId], [Service], [CheckInTime], [UserId]) VALUES (1, 2001, N'Teretana (8-22h) 10000000 dolazaka', CAST(N'2017-03-05 15:45:14.333' AS DateTime), 1)
INSERT [dbo].[CheckIn] ([Id], [ClientId], [Service], [CheckInTime], [UserId]) VALUES (2, 2001, N'Teretana (8-22h) 10000000 dolazaka', CAST(N'2017-03-05 15:46:02.640' AS DateTime), 1)
INSERT [dbo].[CheckIn] ([Id], [ClientId], [Service], [CheckInTime], [UserId]) VALUES (3, 2001, N'Teretana (8-22h) 10000000 dolazaka', CAST(N'2017-03-05 15:48:41.270' AS DateTime), 1)
INSERT [dbo].[CheckIn] ([Id], [ClientId], [Service], [CheckInTime], [UserId]) VALUES (4, 2001, N'10000000 dolazaka', CAST(N'2017-03-05 15:48:49.107' AS DateTime), 1)
INSERT [dbo].[CheckIn] ([Id], [ClientId], [Service], [CheckInTime], [UserId]) VALUES (6, 2001, N'Teretana (8-22h) neograničeno dolazaka', CAST(N'2017-03-05 15:51:06.787' AS DateTime), 1)
INSERT [dbo].[CheckIn] ([Id], [ClientId], [Service], [CheckInTime], [UserId]) VALUES (8, 2001, N'Teretana (8-22h) 12 dolazaka', CAST(N'2017-03-05 16:28:08.893' AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[CheckIn] OFF
SET IDENTITY_INSERT [dbo].[Clients] ON 

INSERT [dbo].[Clients] ([ClientId], [FirstName], [LastName], [Email], [Phone], [BirthDate], [IsActive]) VALUES (1, N'Igor', N'Gašparović', N'igprog@yahoo.com', N'098330966', CAST(N'1977-07-01 00:00:00.000' AS DateTime), 0)
INSERT [dbo].[Clients] ([ClientId], [FirstName], [LastName], [Email], [Phone], [BirthDate], [IsActive]) VALUES (1001, N'Erik', N'Gašparović', N'098330966', N'jardula@yahoo.com', CAST(N'2017-03-30 02:00:00.000' AS DateTime), 0)
INSERT [dbo].[Clients] ([ClientId], [FirstName], [LastName], [Email], [Phone], [BirthDate], [IsActive]) VALUES (2001, N'Sanja', N'Reljić', N'sanja@gmail.com', N'098330966', CAST(N'2017-03-05 00:00:00.000' AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[Clients] OFF
SET IDENTITY_INSERT [dbo].[ClientServices] ON 

INSERT [dbo].[ClientServices] ([Id], [ClientId], [Service], [Option], [Quantity], [Unit], [Price], [ActivationDate], [ExpirationDate], [QuantityLeft]) VALUES (1, 2001, N'Teretana', N'8-22h', 10000000, N'dolazaka', N'250', CAST(N'2017-03-04 21:38:21.970' AS DateTime), CAST(N'2017-04-04 21:38:21.970' AS DateTime), 9999994)
INSERT [dbo].[ClientServices] ([Id], [ClientId], [Service], [Option], [Quantity], [Unit], [Price], [ActivationDate], [ExpirationDate], [QuantityLeft]) VALUES (2, 2001, N'Teretana', N'8-22h', 12, N'dolazaka', N'190', CAST(N'2017-03-04 21:39:54.223' AS DateTime), CAST(N'2017-04-04 21:39:54.223' AS DateTime), 10)
INSERT [dbo].[ClientServices] ([Id], [ClientId], [Service], [Option], [Quantity], [Unit], [Price], [ActivationDate], [ExpirationDate], [QuantityLeft]) VALUES (3, 2001, N'Crossfit', N'8-16', 8, N'sati', N'100', CAST(N'2017-03-04 21:41:36.940' AS DateTime), CAST(N'2017-04-04 21:41:36.940' AS DateTime), 0)
SET IDENTITY_INSERT [dbo].[ClientServices] OFF
SET IDENTITY_INSERT [dbo].[Scheduler] ON 

INSERT [dbo].[Scheduler] ([Id], [Content], [StartDate], [EndDate], [Room]) VALUES (1, N'Aktivnost...', 1488711600000, 1488720600000, 1)
INSERT [dbo].[Scheduler] ([Id], [Content], [StartDate], [EndDate], [Room]) VALUES (5, N'Aktivnost...sala 2', 1488884400000, 1488898800000, 2)
INSERT [dbo].[Scheduler] ([Id], [Content], [StartDate], [EndDate], [Room]) VALUES (8, N'Aktivnost...sala 2', 1488972600000, 1488983400000, 2)
INSERT [dbo].[Scheduler] ([Id], [Content], [StartDate], [EndDate], [Room]) VALUES (10, N'Aktivnost...2', 1488972600000, 1488983400000, 1)
INSERT [dbo].[Scheduler] ([Id], [Content], [StartDate], [EndDate], [Room]) VALUES (11, N'Aktivnost...', 1488798000000, 1488808800000, 1)
INSERT [dbo].[Scheduler] ([Id], [Content], [StartDate], [EndDate], [Room]) VALUES (12, N'Aktivnost...2', 1489057200000, 1489071600000, 2)
INSERT [dbo].[Scheduler] ([Id], [Content], [StartDate], [EndDate], [Room]) VALUES (13, N'sala 3', 1488799800000, 1488810600000, 3)
SET IDENTITY_INSERT [dbo].[Scheduler] OFF
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserId], [UserType], [FirstName], [LastName], [CompanyName], [Address], [PostalCode], [City], [Country], [Pin], [Phone], [Email], [UserName], [Password], [AdminType], [UserGroupId], [ActivationDate], [ExpirationDate], [IsActive], [IPAddress]) VALUES (1, 0, N'Spartacus', N'Spartacus', N'', N'', N'', N'', N'', N'', N'', N'spartacus@gmail.com', N'admin', N'rN/ymThWzGVqdbXkGEqEX8Xqz38BrmtkDAm6Z5KRvH0=', 0, 453, CAST(N'2017-03-05 10:47:18.337' AS DateTime), CAST(N'2017-03-05 10:47:18.337' AS DateTime), 1, N'')
INSERT [dbo].[Users] ([UserId], [UserType], [FirstName], [LastName], [CompanyName], [Address], [PostalCode], [City], [Country], [Pin], [Phone], [Email], [UserName], [Password], [AdminType], [UserGroupId], [ActivationDate], [ExpirationDate], [IsActive], [IPAddress]) VALUES (101, 0, N'Igor', N'Gašparović', N'', N'', N'', N'', N'', N'', N'', N'igprog@yahoo.com', N'igor', N'lQr0bcokppTd5n5nik0L+Q==', 1, 453, CAST(N'2017-03-05 11:04:19.243' AS DateTime), CAST(N'2017-03-05 11:04:19.243' AS DateTime), 1, N'')
INSERT [dbo].[Users] ([UserId], [UserType], [FirstName], [LastName], [CompanyName], [Address], [PostalCode], [City], [Country], [Pin], [Phone], [Email], [UserName], [Password], [AdminType], [UserGroupId], [ActivationDate], [ExpirationDate], [IsActive], [IPAddress]) VALUES (201, 0, N'Erik', N'Gašparović', N'', N'', N'', N'', N'', N'', N'', N'erik@gmail.com', N'erik', N'lQr0bcokppTd5n5nik0L+Q==', 1, 453, CAST(N'2017-03-05 11:09:23.623' AS DateTime), CAST(N'2017-03-05 11:09:23.623' AS DateTime), 1, N'')
SET IDENTITY_INSERT [dbo].[Users] OFF
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0: pravna osoba; 1: fizicka osoba' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'UserType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0: superviosor; 1: standard user' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'AdminType'
GO
