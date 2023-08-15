USE [AssignRef]
GO
/****** Object:  StoredProcedure [dbo].[TestInstances_Insert]    Script Date: 4/26/2023 10:39:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Edgar Melano
-- Create date: 2023/04/17
-- Description: INSERT proc for TestInstances
-- Code Reviewer: Sydney Wells

-- MODIFIED BY:
-- MODIFIED DATE: 
-- Code Reviewer:
-- Note:
-- =============================================

CREATE PROC [dbo].[TestInstances_Insert]
	@TestId int
	,@UserId int
	,@StatusId int
	,@Id int OUTPUT

as

/*

	DECLARE	@TestId int = 10
			,@UserId int = 8
			,@StatusId int = 1
			,@Id int

	EXECUTE	dbo.TestInstances_Insert
			@TestId
			,@UserId
			,@StatusId
			,@Id OUTPUT

	SELECT	Id
			,TestId
			,UserId
			,DateCreated
			,DateModified
			,StatusId
	FROM	dbo.TestInstances
	WHERE	Id = @Id

*/

BEGIN

	INSERT INTO	[dbo].[TestInstances]
				([TestId]
				,[UserId]
				,[StatusId])
     VALUES
				(@TestId
				,@UserId
				,@StatusId)

	SET	@Id = SCOPE_IDENTITY()

END


GO
