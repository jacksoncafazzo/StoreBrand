USE [storebrand]
GO
/****** Object:  Table [dbo].[brands]    Script Date: 3/3/2016 11:39:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[brands](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](255) NULL,
	[logo] [varchar](255) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[store_brand]    Script Date: 3/3/2016 11:39:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[store_brand](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[store_id] [int] NULL,
	[brand_id] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[stores]    Script Date: 3/3/2016 11:39:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[stores](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](255) NULL,
	[url] [varchar](255) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
