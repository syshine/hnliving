USE [hnliving]
GO

/****** Object:  Table [dbo].[hnl_filterwords]    Script Date: 2018/12/29 ÐÇÆÚÁù 9:23:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[hnl_filterwords](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[match] [nvarchar](250) NOT NULL,
	[replace] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_hnl_filterword] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[hnl_filterwords] ADD  CONSTRAINT [DF_hnl_filterwords_match]  DEFAULT ('') FOR [match]
GO

ALTER TABLE [dbo].[hnl_filterwords] ADD  CONSTRAINT [DF_hnl_filterwords_replace]  DEFAULT ('') FOR [replace]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'×ÔÔöid' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_filterwords', @level2type=N'COLUMN',@level2name=N'id'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Æ¥Åä´Ê' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_filterwords', @level2type=N'COLUMN',@level2name=N'match'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ìæ»»´Ê' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_filterwords', @level2type=N'COLUMN',@level2name=N'replace'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'É¸Ñ¡´Ê±í' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hnl_filterwords'
GO


