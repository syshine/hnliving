USE [hnliving]
GO

/****** Object:  StoredProcedure [dbo].[hnl_getuidbyusername]    Script Date: 2018/12/29 ÐÇÆÚÁù 9:26:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[hnl_getuidbyusername] 
@username nchar(20)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT [uid] FROM [hnl_users] WHERE [username]=@username;
END


GO


