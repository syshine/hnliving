USE [hnliving]
GO

/****** Object:  StoredProcedure [dbo].[hnl_getuserbyid]    Script Date: 2019/1/3 ÐÇÆÚËÄ 12:32:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[hnl_getuserbyid] 
@uid int
AS
BEGIN
	SET NOCOUNT ON;

SELECT TOP 1 
	   [temp1].[uid]
      ,[temp1].[username]
      ,[temp1].[email]
      ,[temp1].[mobile]
      ,[temp1].[password]
      ,[temp1].[userrid]
      ,[temp1].[nickname]
      ,[temp1].[avatar]
      ,[temp1].[rankcredits]
      ,[temp1].[verifyemail]
      ,[temp1].[verifymobile]
      ,[temp1].[liftbantime]
      ,[temp1].[salt]
      ,[temp2].[lastvisittime]
      ,[temp2].[lastvisitip]
      ,[temp2].[lastvisitrgid]
      ,[temp2].[registertime]
      ,[temp2].[registerip]
      ,[temp2].[registerrgid]
      ,[temp2].[gender]
      ,[temp2].[realname]
      ,[temp2].[bday]
      ,[temp2].[idcard]
      ,[temp2].[regionid]
      ,[temp2].[address]
      ,[temp2].[bio]
FROM [hnl_users] AS [temp1] LEFT JOIN [hnl_userdetails] AS [temp2] ON [temp1].[uid]=[temp2].[uid] 
WHERE [temp1].[uid]=@uid
END


GO


