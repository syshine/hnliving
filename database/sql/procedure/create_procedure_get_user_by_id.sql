USE [hnliving]
GO

/****** Object:  StoredProcedure [dbo].[hnl_getpartuserbyid]    Script Date: 2018/12/28 ÐÇÆÚÎå 10:55:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[hnl_getpartuserbyid]
@uid int
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
  FROM [hnl_users] WHERE [uid]=@uid;
END


GO


