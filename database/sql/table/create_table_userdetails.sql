USE [hnliving]
GO

/****** Object:  Table [dbo].[hnl_userdetails]    Script Date: 2018/12/29 星期六 9:52:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[hnl_userdetails](
	[uid] [int] NOT NULL,
	[lastvisittime] [datetime] NOT NULL,
	[lastvisitip] [char](15) NOT NULL,
	[lastvisitrgid] [smallint] NOT NULL,
	[registertime] [datetime] NOT NULL,
	[registerip] [char](15) NOT NULL,
	[registerrgid] [smallint] NOT NULL,
	[gender] [tinyint] NOT NULL,
	[realname] [nvarchar](10) NOT NULL,
	[bday] [datetime] NOT NULL,
	[idcard] [varchar](18) NOT NULL,
	[regionid] [smallint] NOT NULL,
	[address] [nvarchar](150) NOT NULL,
	[bio] [nvarchar](300) NOT NULL,
 CONSTRAINT [PK_hnl_userdetails] PRIMARY KEY CLUSTERED 
(
	[uid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[hnl_userdetails] ADD  CONSTRAINT [DF_hnl_userdetails_uid]  DEFAULT ((0)) FOR [uid]
GO

ALTER TABLE [dbo].[hnl_userdetails] ADD  CONSTRAINT [DF_hnl_userdetails_lastvisittime]  DEFAULT (getdate()) FOR [lastvisittime]
GO

ALTER TABLE [dbo].[hnl_userdetails] ADD  CONSTRAINT [DF_hnl_userdetails_lastvisitip]  DEFAULT ('') FOR [lastvisitip]
GO

ALTER TABLE [dbo].[hnl_userdetails] ADD  CONSTRAINT [DF_hnl_userdetails_lastvisitrgid]  DEFAULT ((-1)) FOR [lastvisitrgid]
GO

ALTER TABLE [dbo].[hnl_userdetails] ADD  CONSTRAINT [DF_hnl_userdetails_registertime]  DEFAULT (getdate()) FOR [registertime]
GO

ALTER TABLE [dbo].[hnl_userdetails] ADD  CONSTRAINT [DF_hnl_userdetails_registerip]  DEFAULT ('') FOR [registerip]
GO

ALTER TABLE [dbo].[hnl_userdetails] ADD  CONSTRAINT [DF_hnl_userdetails_registerrgid]  DEFAULT ((-1)) FOR [registerrgid]
GO

ALTER TABLE [dbo].[hnl_userdetails] ADD  CONSTRAINT [DF_hnl_userdetails_gender]  DEFAULT ((0)) FOR [gender]
GO

ALTER TABLE [dbo].[hnl_userdetails] ADD  CONSTRAINT [DF_hnl_userdetails_realname]  DEFAULT ('') FOR [realname]
GO

ALTER TABLE [dbo].[hnl_userdetails] ADD  CONSTRAINT [DF_hnl_userdetails_bday]  DEFAULT ('1900-1-1') FOR [bday]
GO

ALTER TABLE [dbo].[hnl_userdetails] ADD  CONSTRAINT [DF_hnl_userdetails_idcard]  DEFAULT ('') FOR [idcard]
GO

ALTER TABLE [dbo].[hnl_userdetails] ADD  CONSTRAINT [DF_hnl_userdetails_regionid]  DEFAULT ((0)) FOR [regionid]
GO

ALTER TABLE [dbo].[hnl_userdetails] ADD  CONSTRAINT [DF_hnl_userdetails_address]  DEFAULT ('') FOR [address]
GO

ALTER TABLE [dbo].[hnl_userdetails] ADD  CONSTRAINT [DF_hnl_userdetails_bio]  DEFAULT ('') FOR [bio]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_userdetails', @level2type=N'COLUMN',@level2name=N'uid'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后访问时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_userdetails', @level2type=N'COLUMN',@level2name=N'lastvisittime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后访问ip' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_userdetails', @level2type=N'COLUMN',@level2name=N'lastvisitip'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后访问区域id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_userdetails', @level2type=N'COLUMN',@level2name=N'lastvisitrgid'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'注册时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_userdetails', @level2type=N'COLUMN',@level2name=N'registertime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'注册ip' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_userdetails', @level2type=N'COLUMN',@level2name=N'registerip'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'注册区域id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_userdetails', @level2type=N'COLUMN',@level2name=N'registerrgid'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'性别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_userdetails', @level2type=N'COLUMN',@level2name=N'gender'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'真实姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_userdetails', @level2type=N'COLUMN',@level2name=N'realname'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出生日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_userdetails', @level2type=N'COLUMN',@level2name=N'bday'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'身份证号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_userdetails', @level2type=N'COLUMN',@level2name=N'idcard'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所在区域id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_userdetails', @level2type=N'COLUMN',@level2name=N'regionid'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'详细地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_userdetails', @level2type=N'COLUMN',@level2name=N'address'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'简介' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_userdetails', @level2type=N'COLUMN',@level2name=N'bio'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户详细信息表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_userdetails'
GO


