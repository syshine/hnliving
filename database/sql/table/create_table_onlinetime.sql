USE [hnliving]
GO

/****** Object:  Table [dbo].[hnl_onlinetime]    Script Date: 2018/12/29 星期六 9:53:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[hnl_onlinetime](
	[uid] [int] NOT NULL,
	[total] [int] NOT NULL,
	[year] [int] NOT NULL,
	[month] [smallint] NOT NULL,
	[week] [smallint] NOT NULL,
	[day] [smallint] NOT NULL,
	[updatetime] [datetime] NOT NULL,
 CONSTRAINT [PK_hnl_onlinetime] PRIMARY KEY CLUSTERED 
(
	[uid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[hnl_onlinetime] ADD  CONSTRAINT [DF_hnl_onlinetime_uid]  DEFAULT ((0)) FOR [uid]
GO

ALTER TABLE [dbo].[hnl_onlinetime] ADD  CONSTRAINT [DF_hnl_onlinetime_total]  DEFAULT ((0)) FOR [total]
GO

ALTER TABLE [dbo].[hnl_onlinetime] ADD  CONSTRAINT [DF_hnl_onlinetime_year]  DEFAULT ((0)) FOR [year]
GO

ALTER TABLE [dbo].[hnl_onlinetime] ADD  CONSTRAINT [DF_hnl_onlinetime_month]  DEFAULT ((0)) FOR [month]
GO

ALTER TABLE [dbo].[hnl_onlinetime] ADD  CONSTRAINT [DF_hnl_onlinetime_week]  DEFAULT ((0)) FOR [week]
GO

ALTER TABLE [dbo].[hnl_onlinetime] ADD  CONSTRAINT [DF_hnl_onlinetime_day]  DEFAULT ((0)) FOR [day]
GO

ALTER TABLE [dbo].[hnl_onlinetime] ADD  CONSTRAINT [DF_hnl_onlinetime_updatetime]  DEFAULT (getdate()) FOR [updatetime]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_onlinetime', @level2type=N'COLUMN',@level2name=N'uid'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'总在线时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_onlinetime', @level2type=N'COLUMN',@level2name=N'total'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'今年在线时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_onlinetime', @level2type=N'COLUMN',@level2name=N'year'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'本月在线时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_onlinetime', @level2type=N'COLUMN',@level2name=N'month'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'本周在线时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_onlinetime', @level2type=N'COLUMN',@level2name=N'week'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'今天在线时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_onlinetime', @level2type=N'COLUMN',@level2name=N'day'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后更新时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_onlinetime', @level2type=N'COLUMN',@level2name=N'updatetime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'在线时间表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_onlinetime'
GO


