USE [hnliving]
GO

/****** Object:  Table [dbo].[hnl_userranks]    Script Date: 2018/12/29 星期六 9:28:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[hnl_userranks](
	[userrid] [smallint] IDENTITY(1,1) NOT NULL,
	[system] [int] NOT NULL,
	[title] [nchar](50) NOT NULL,
	[avatar] [char](50) NOT NULL,
	[limitdays] [int] NOT NULL,
 CONSTRAINT [PK_hnl_userranks] PRIMARY KEY CLUSTERED 
(
	[userrid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[hnl_userranks] ADD  CONSTRAINT [DF_hnl_userranks_system]  DEFAULT ((0)) FOR [system]
GO

ALTER TABLE [dbo].[hnl_userranks] ADD  CONSTRAINT [DF_hnl_userranks_title]  DEFAULT ('') FOR [title]
GO

ALTER TABLE [dbo].[hnl_userranks] ADD  CONSTRAINT [DF_hnl_userranks_avatar]  DEFAULT ('') FOR [avatar]
GO

ALTER TABLE [dbo].[hnl_userranks] ADD  CONSTRAINT [DF_hnl_userranks_limitdays]  DEFAULT ((0)) FOR [limitdays]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户等级id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_userranks', @level2type=N'COLUMN',@level2name=N'userrid'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否为系统等级' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_userranks', @level2type=N'COLUMN',@level2name=N'system'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_userranks', @level2type=N'COLUMN',@level2name=N'title'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'头像' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_userranks', @level2type=N'COLUMN',@level2name=N'avatar'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'限制天数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_userranks', @level2type=N'COLUMN',@level2name=N'limitdays'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户等级表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_userranks'
GO


