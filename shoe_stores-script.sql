USE [shoestores]
GO
/****** Object:  Table [dbo].[brand_store]    Script Date: 3/11/2016 11:44:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[brand_store](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[store_id] [int] NULL,
	[brand_id] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[brands]    Script Date: 3/11/2016 11:44:29 AM ******/
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
/****** Object:  Table [dbo].[stores]    Script Date: 3/11/2016 11:44:29 AM ******/
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
SET IDENTITY_INSERT [dbo].[brands] ON 

INSERT [dbo].[brands] ([id], [name], [logo]) VALUES (1, N'Keen', N'keen-logo.png')
INSERT [dbo].[brands] ([id], [name], [logo]) VALUES (2, N'Adidas', N'adidas.png')
INSERT [dbo].[brands] ([id], [name], [logo]) VALUES (3, N'New Balance', N'newbalance.png')
INSERT [dbo].[brands] ([id], [name], [logo]) VALUES (4, N'Nike', N'nike.png')
INSERT [dbo].[brands] ([id], [name], [logo]) VALUES (5, N'Puma', N'puma.png')
INSERT [dbo].[brands] ([id], [name], [logo]) VALUES (6, N'Saucony', N'saucony.png')
SET IDENTITY_INSERT [dbo].[brands] OFF
SET IDENTITY_INSERT [dbo].[stores] ON 

INSERT [dbo].[stores] ([id], [name], [url]) VALUES (1, N'Keen Garage', N'keen.com')
SET IDENTITY_INSERT [dbo].[stores] OFF
