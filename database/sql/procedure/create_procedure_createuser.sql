USE [hnliving]
GO

/****** Object:  StoredProcedure [dbo].[hnl_createuser]    Script Date: 2018/12/29 ÐÇÆÚÁù 9:44:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[hnl_createuser]
           @username  nchar(20),
           @email char(50),
           @mobile  char(15),
           @password char(32),
           @userrid smallint,
           @nickname nchar(20),
           @avatar char(40),
           @rankcredits int,
           @verifyemail tinyint,
           @verifymobile tinyint,
		   @liftbantime datetime,
           @salt nchar(6),
           
           @lastvisittime datetime,
           @lastvisitip char(15),
		   @lastvisitrgid smallint,
           @registertime datetime,
           @registerip char(15),
		   @registerrgid smallint,
           @gender  tinyint,
           @realname  nvarchar(10),
           @bday  datetime,
           @idcard  varchar(18),
           @regionid  smallint,
           @address  nvarchar(150),
           @bio  nvarchar(300)

AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @uid int;
	INSERT INTO [hnl_users]
           ([username]
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
		   ,[salt])
     VALUES
           (@username,
           @email, 
           @mobile,
           @password,
           @userrid,
           @nickname,
           @avatar,
           @rankcredits,
           @verifyemail,
           @verifymobile,
		   @liftbantime,
		   @salt);
    SET @uid = SCOPE_IDENTITY();
	INSERT INTO [hnl_userdetails]
           ([uid]
		   ,[lastvisittime]
           ,[lastvisitip]
		   ,[lastvisitrgid]
		   ,[registertime]
		   ,[registerip]
		   ,[registerrgid]
           ,[gender]
           ,[realname]
           ,[bday]
           ,[idcard]
           ,[regionid]
           ,[address]
           ,[bio])
     VALUES
           (@uid
		   ,@lastvisittime
           ,@lastvisitip
		   ,@lastvisitrgid
		   ,@registertime
		   ,@registerip
		   ,@registerrgid
           ,@gender
           ,@realname
           ,@bday
           ,@idcard
           ,@regionid
           ,@address
           ,@bio);
    INSERT INTO [hnl_onlinetime]
           ([uid]
           ,[total]
           ,[year]
           ,[month]
           ,[week]
           ,[day]
           ,[updatetime])
     VALUES
           (@uid
           ,0
           ,0
           ,0
           ,0
           ,0
           ,@registertime)
	SELECT @uid AS 'uid';
END


GO


