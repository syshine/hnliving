USE [hnliving]
GO

/****** Object:  StoredProcedure [dbo].[hnl_getpartuserbyname]    Script Date: 2018/12/28 ÐÇÆÚÎå 10:31:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[hnl_getpartuserbyname]
@username nchar(20)
AS
BEGIN
	SET NOCOUNT ON;
		SELECT [uid]
      ,[username]
      ,[email]
      ,[mobile]
      ,[password]
      ,[userrid]
      ,[nickname]
      ,[avatar]
      ,[rankcredits]
      ,[verifyemail]
      ,[verifymobile]
      ,[liftbantime]
      ,[salt]
  FROM [hnl_users] WHERE [username]=@username;
END


GO


