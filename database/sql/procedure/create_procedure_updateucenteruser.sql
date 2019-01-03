USE [hnliving]
GO

/****** Object:  StoredProcedure [dbo].[hnl_updateucenteruser]    Script Date: 2019/1/3 ÐÇÆÚËÄ 14:42:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[hnl_updateucenteruser]
@uid int,
@username nchar(20),
@nickname nchar(20),
@avatar char(40),
@gender  tinyint,
@realname  nvarchar(10),
@bday  datetime,
@idcard varchar(18),
@regionid  smallint,
@address  nvarchar(150),
@bio  nvarchar(300)
AS
BEGIN

UPDATE [hnl_users] SET [username]=@username,[nickname]=@nickname,[avatar]=@avatar WHERE [uid]=@uid;
 
UPDATE [hnl_userdetails] SET [gender]=@gender,[realname]=@realname,[bday]=@bday,[idcard]=@idcard,[regionid]=@regionid,[address]=@address,[bio]=@bio WHERE [uid]=@uid;

END


GO


