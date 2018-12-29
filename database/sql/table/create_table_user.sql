USE [hnliving]
GO

/****** Object:  Table [dbo].[hnl_users]    Script Date: 2018/12/28 ������ 10:16:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[hnl_users](
	[uid] [int] IDENTITY(1,1) NOT NULL,
	[username] [nchar](20) NOT NULL,
	[email] [char](50) NOT NULL,
	[mobile] [char](15) NOT NULL,
	[password] [char](32) NOT NULL,
	[userrid] [smallint] NOT NULL,
	[nickname] [nchar](20) NOT NULL,
	[avatar] [char](40) NOT NULL,
	[rankcredits] [int] NOT NULL,
	[verifyemail] [tinyint] NOT NULL,
	[verifymobile] [tinyint] NOT NULL,
	[liftbantime] [datetime] NOT NULL,
	[salt] [nchar](6) NOT NULL,
 CONSTRAINT [PK_hnl_users] PRIMARY KEY CLUSTERED 
(
	[uid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[hnl_users] ADD  CONSTRAINT [DF_hnl_users_username]  DEFAULT ('') FOR [username]
GO

ALTER TABLE [dbo].[hnl_users] ADD  CONSTRAINT [DF_hnl_users_email]  DEFAULT ('') FOR [email]
GO

ALTER TABLE [dbo].[hnl_users] ADD  CONSTRAINT [DF_hnl_users_mobile]  DEFAULT ('') FOR [mobile]
GO

ALTER TABLE [dbo].[hnl_users] ADD  CONSTRAINT [DF_hnl_users_password]  DEFAULT ('') FOR [password]
GO

ALTER TABLE [dbo].[hnl_users] ADD  CONSTRAINT [DF_hnl_users_userrid]  DEFAULT ((0)) FOR [userrid]
GO

ALTER TABLE [dbo].[hnl_users] ADD  CONSTRAINT [DF_hnl_users_nickname]  DEFAULT ('') FOR [nickname]
GO

ALTER TABLE [dbo].[hnl_users] ADD  CONSTRAINT [DF_hnl_users_avatar]  DEFAULT ('') FOR [avatar]
GO

ALTER TABLE [dbo].[hnl_users] ADD  CONSTRAINT [DF_hnl_users_rankcredits]  DEFAULT ((0)) FOR [rankcredits]
GO

ALTER TABLE [dbo].[hnl_users] ADD  CONSTRAINT [DF_hnl_users_verifyemail]  DEFAULT ((0)) FOR [verifyemail]
GO

ALTER TABLE [dbo].[hnl_users] ADD  CONSTRAINT [DF_hnl_users_verifymobile]  DEFAULT ((0)) FOR [verifymobile]
GO

ALTER TABLE [dbo].[hnl_users] ADD  CONSTRAINT [DF_hnl_users_liftbantime]  DEFAULT ('1900-1-1 00:00:00') FOR [liftbantime]
GO

ALTER TABLE [dbo].[hnl_users] ADD  CONSTRAINT [DF_hnl_users_salt]  DEFAULT ('') FOR [salt]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�û�id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_users', @level2type=N'COLUMN',@level2name=N'uid'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�û�����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_users', @level2type=N'COLUMN',@level2name=N'username'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_users', @level2type=N'COLUMN',@level2name=N'email'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ֻ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_users', @level2type=N'COLUMN',@level2name=N'mobile'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_users', @level2type=N'COLUMN',@level2name=N'password'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�û��ȼ�id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_users', @level2type=N'COLUMN',@level2name=N'userrid'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ǳ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_users', @level2type=N'COLUMN',@level2name=N'nickname'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ͷ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_users', @level2type=N'COLUMN',@level2name=N'avatar'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ȼ�����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_users', @level2type=N'COLUMN',@level2name=N'rankcredits'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ƿ���֤����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_users', @level2type=N'COLUMN',@level2name=N'verifyemail'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ƿ���֤�ֻ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_users', @level2type=N'COLUMN',@level2name=N'verifymobile'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_users', @level2type=N'COLUMN',@level2name=N'liftbantime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ֵ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_users', @level2type=N'COLUMN',@level2name=N'salt'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�û���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_users'
GO


