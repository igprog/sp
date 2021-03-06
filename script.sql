USE [hrs]
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'AdminType'

GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'UserType'

GO
/****** Object:  Table [dbo].[Users]    Script Date: 11/12/2017 6:50:35 PM ******/
DROP TABLE [dbo].[Users]
GO
/****** Object:  Table [dbo].[Students]    Script Date: 11/12/2017 6:50:35 PM ******/
DROP TABLE [dbo].[Students]
GO
/****** Object:  Table [dbo].[SchoolClasses]    Script Date: 11/12/2017 6:50:35 PM ******/
DROP TABLE [dbo].[SchoolClasses]
GO
/****** Object:  Table [dbo].[Scheduler]    Script Date: 11/12/2017 6:50:35 PM ******/
DROP TABLE [dbo].[Scheduler]
GO
/****** Object:  Table [dbo].[Realizations]    Script Date: 11/12/2017 6:50:35 PM ******/
DROP TABLE [dbo].[Realizations]
GO
/****** Object:  Table [dbo].[MailMessages]    Script Date: 11/12/2017 6:50:35 PM ******/
DROP TABLE [dbo].[MailMessages]
GO
/****** Object:  Table [dbo].[ClientServices]    Script Date: 11/12/2017 6:50:35 PM ******/
DROP TABLE [dbo].[ClientServices]
GO
/****** Object:  Table [dbo].[Clients]    Script Date: 11/12/2017 6:50:35 PM ******/
DROP TABLE [dbo].[Clients]
GO
/****** Object:  Table [dbo].[CheckIn]    Script Date: 11/12/2017 6:50:35 PM ******/
DROP TABLE [dbo].[CheckIn]
GO
/****** Object:  Table [dbo].[CheckIn]    Script Date: 11/12/2017 6:50:35 PM ******/
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
/****** Object:  Table [dbo].[Clients]    Script Date: 11/12/2017 6:50:35 PM ******/
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
/****** Object:  Table [dbo].[ClientServices]    Script Date: 11/12/2017 6:50:35 PM ******/
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
/****** Object:  Table [dbo].[MailMessages]    Script Date: 11/12/2017 6:50:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MailMessages](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](50) NULL,
	[GroupEmails] [nvarchar](max) NULL,
	[Subject] [nvarchar](50) NULL,
	[Message] [nvarchar](max) NULL,
	[Date] [datetime] NULL,
 CONSTRAINT [PK_Massages] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Realizations]    Script Date: 11/12/2017 6:50:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Realizations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SchoolClass] [int] NULL,
	[Leader] [int] NULL,
	[Duration] [int] NULL,
	[Date] [datetime] NULL,
	[Description] [nvarchar](max) NULL,
	[Note] [nvarchar](max) NULL,
	[Substitute] [nvarchar](50) NULL,
	[Type] [int] NULL,
	[Students] [nvarchar](max) NULL,
 CONSTRAINT [PK_Realizations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Scheduler]    Script Date: 11/12/2017 6:50:35 PM ******/
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
/****** Object:  Table [dbo].[SchoolClasses]    Script Date: 11/12/2017 6:50:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SchoolClasses](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[School] [nvarchar](50) NULL,
	[SchoolClass] [nvarchar](50) NULL,
	[Leader] [int] NULL,
	[DefaultHours] [int] NULL,
	[Teacher] [nvarchar](50) NULL,
 CONSTRAINT [PK_SchoolClasses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Students]    Script Date: 11/12/2017 6:50:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Students](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SchoolClass] [int] NULL,
	[Teacher] [nvarchar](50) NULL,
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[Gender] [int] NULL,
	[BirthDate] [datetime] NULL,
	[BirthPlace] [nvarchar](50) NULL,
	[Address] [nvarchar](max) NULL,
	[Height] [int] NULL,
	[Weight] [int] NULL,
	[FootSize] [nvarchar](50) NULL,
	[TShirtSize] [nvarchar](50) NULL,
	[ParentFirstName] [nvarchar](50) NULL,
	[ParentLastName] [nvarchar](50) NULL,
	[Phone] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NULL,
	[Job] [nvarchar](50) NULL,
 CONSTRAINT [PK_Students] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 11/12/2017 6:50:35 PM ******/
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
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0: pravna osoba; 1: fizicka osoba' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'UserType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0: superviosor; 1: standard user' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'AdminType'
GO
