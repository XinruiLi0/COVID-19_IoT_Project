USE [master]
GO
/****** Object:  Database [ivmsdb]    Script Date: 2021/3/21 18:07:02 ******/
CREATE DATABASE [ivmsdb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ivmsdb', FILENAME = N'D:\rdsdbdata\DATA\ivmsdb.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 10%)
 LOG ON 
( NAME = N'ivmsdb_log', FILENAME = N'D:\rdsdbdata\DATA\ivmsdb_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [ivmsdb] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ivmsdb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ivmsdb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ivmsdb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ivmsdb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ivmsdb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ivmsdb] SET ARITHABORT OFF 
GO
ALTER DATABASE [ivmsdb] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ivmsdb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ivmsdb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ivmsdb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ivmsdb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ivmsdb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ivmsdb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ivmsdb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ivmsdb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ivmsdb] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ivmsdb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ivmsdb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ivmsdb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ivmsdb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ivmsdb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ivmsdb] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ivmsdb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ivmsdb] SET RECOVERY FULL 
GO
ALTER DATABASE [ivmsdb] SET  MULTI_USER 
GO
ALTER DATABASE [ivmsdb] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ivmsdb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ivmsdb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ivmsdb] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [ivmsdb] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [ivmsdb] SET QUERY_STORE = OFF
GO
USE [ivmsdb]
GO
/****** Object:  User [reader]    Script Date: 2021/3/21 18:07:03 ******/
CREATE USER [reader] FOR LOGIN [reader] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [admin]    Script Date: 2021/3/21 18:07:03 ******/
CREATE USER [admin] FOR LOGIN [admin] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_datareader] ADD MEMBER [reader]
GO
ALTER ROLE [db_owner] ADD MEMBER [admin]
GO
/****** Object:  Table [dbo].[AccountLogin]    Script Date: 2021/3/21 18:07:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountLogin](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](50) NOT NULL,
	[UserPassword] [varchar](50) NULL,
	[UserRole] [int] NULL,
	[UserEmail] [varchar](50) NULL,
	[BluetoothID] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ConfirmedCases]    Script Date: 2021/3/21 18:07:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ConfirmedCases](
	[ID] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CurrentContact]    Script Date: 2021/3/21 18:07:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CurrentContact](
	[Guard_ID] [int] NULL,
	[Visitor_ID] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DataForML]    Script Date: 2021/3/21 18:07:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DataForML](
	[SourceID] [int] NOT NULL,
	[TargetID] [int] NOT NULL,
	[Age] [int] NOT NULL,
	[HasInfectedBefore] [int] NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[Periods] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[CloseContact] [int] NOT NULL,
	[ClosePeriods] [int] NOT NULL,
	[HasPredicted] [int] NOT NULL,
	[distance] [float] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GuardDevices]    Script Date: 2021/3/21 18:07:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GuardDevices](
	[ID] [int] NULL,
	[DeviceID] [nvarchar](255) NULL,
	[Description] [nvarchar](255) NULL,
	[VisitorTemperature] [float] NULL,
	[VisitorID] [int] NULL,
	[LastUpdated] [datetime] NULL,
UNIQUE NONCLUSTERED 
(
	[DeviceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GuardInfo]    Script Date: 2021/3/21 18:07:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GuardInfo](
	[ID] [int] NULL,
	[Address] [varchar](50) NOT NULL,
	[longitude] [float] NULL,
	[latitude] [float] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HealthStatus]    Script Date: 2021/3/21 18:07:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HealthStatus](
	[ID] [int] NULL,
	[Age] [int] NULL,
	[HasInfectedBefore] [int] NULL,
	[UserStatus] [int] NOT NULL,
	[Predict] [int] NOT NULL,
	[lastPredict] [datetime] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PersonalContact]    Script Date: 2021/3/21 18:07:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PersonalContact](
	[ID] [int] NULL,
	[Contact_ID] [int] NULL,
	[Guard_ID] [int] NULL,
	[StartTime] [datetime] NOT NULL,
	[EndTime] [datetime] NULL,
	[ManualUpdate] [bit] NULL,
	[CloseContact] [int] NOT NULL,
	[ClosePeriods] [int] NOT NULL,
	[distance] [float] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[test]    Script Date: 2021/3/21 18:07:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[test](
	[column1] [int] NULL,
	[column2] [char](255) NULL,
	[column3] [float] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[DataForML] ADD  DEFAULT ((0)) FOR [Status]
GO
ALTER TABLE [dbo].[DataForML] ADD  DEFAULT ((0)) FOR [CloseContact]
GO
ALTER TABLE [dbo].[DataForML] ADD  DEFAULT ((0)) FOR [ClosePeriods]
GO
ALTER TABLE [dbo].[DataForML] ADD  DEFAULT ((0)) FOR [HasPredicted]
GO
ALTER TABLE [dbo].[DataForML] ADD  DEFAULT ((60.0)) FOR [distance]
GO
ALTER TABLE [dbo].[HealthStatus] ADD  DEFAULT ((0)) FOR [UserStatus]
GO
ALTER TABLE [dbo].[HealthStatus] ADD  DEFAULT ((0)) FOR [Predict]
GO
ALTER TABLE [dbo].[HealthStatus] ADD  DEFAULT (getdate()) FOR [lastPredict]
GO
ALTER TABLE [dbo].[PersonalContact] ADD  DEFAULT ((0)) FOR [CloseContact]
GO
ALTER TABLE [dbo].[PersonalContact] ADD  DEFAULT ((0)) FOR [ClosePeriods]
GO
ALTER TABLE [dbo].[PersonalContact] ADD  DEFAULT ((60.0)) FOR [distance]
GO
ALTER TABLE [dbo].[ConfirmedCases]  WITH CHECK ADD FOREIGN KEY([ID])
REFERENCES [dbo].[AccountLogin] ([ID])
GO
ALTER TABLE [dbo].[CurrentContact]  WITH CHECK ADD FOREIGN KEY([Guard_ID])
REFERENCES [dbo].[AccountLogin] ([ID])
GO
ALTER TABLE [dbo].[CurrentContact]  WITH CHECK ADD FOREIGN KEY([Visitor_ID])
REFERENCES [dbo].[AccountLogin] ([ID])
GO
ALTER TABLE [dbo].[GuardDevices]  WITH CHECK ADD FOREIGN KEY([ID])
REFERENCES [dbo].[AccountLogin] ([ID])
GO
ALTER TABLE [dbo].[GuardInfo]  WITH CHECK ADD FOREIGN KEY([ID])
REFERENCES [dbo].[AccountLogin] ([ID])
GO
ALTER TABLE [dbo].[HealthStatus]  WITH CHECK ADD FOREIGN KEY([ID])
REFERENCES [dbo].[AccountLogin] ([ID])
GO
ALTER TABLE [dbo].[PersonalContact]  WITH CHECK ADD FOREIGN KEY([Contact_ID])
REFERENCES [dbo].[AccountLogin] ([ID])
GO
ALTER TABLE [dbo].[PersonalContact]  WITH CHECK ADD FOREIGN KEY([Guard_ID])
REFERENCES [dbo].[AccountLogin] ([ID])
GO
ALTER TABLE [dbo].[PersonalContact]  WITH CHECK ADD FOREIGN KEY([ID])
REFERENCES [dbo].[AccountLogin] ([ID])
GO
USE [master]
GO
ALTER DATABASE [ivmsdb] SET  READ_WRITE 
GO
