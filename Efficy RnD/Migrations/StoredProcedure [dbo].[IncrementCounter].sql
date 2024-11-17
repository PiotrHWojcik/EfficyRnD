/****** Object:  StoredProcedure [dbo].[IncrementCounter]    Script Date: 18.11.2024 00:46:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[IncrementCounter] 
	@CounterId INT 
AS 
BEGIN 
	SET NOCOUNT ON; 
	
	UPDATE Counters 
	SET Value = Value + 1 
	WHERE Id = @CounterId; 
END
