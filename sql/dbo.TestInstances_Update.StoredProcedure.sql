GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Edgar Melano
-- Create date: 2023/04/17
-- Description: Update proc for TestInstances
-- Code Reviewer: Sydney Wells

-- MODIFIED BY:
-- MODIFIED DATE: 
-- Code Reviewer:
-- Note:
-- =============================================
CREATE PROC [dbo].[TestInstances_Update]
	@Id int
	,@TestId int
	,@UserId int
	,@StatusId int

as

/*
	DECLARE	
	 	@Id int = 32
		,@TestId int = 11
		,@UserId int = 8
		,@StatusId int = 1

	SELECT	
	 	Id
		,TestId
		,UserId
		,DateCreated
		,DateModified
		,StatusId
	FROM	
	 	dbo.TestInstances
	WHERE	
	 	Id = @Id

	EXECUTE 
	 	dbo.TestInstances_Update
			@Id
			,@TestId
			,@UserId
			,@StatusId

	SELECT	
	 	Id
		,TestId
		,UserId
		,DateCreated
		,DateModified
		,StatusId
	FROM	
	 	dbo.TestInstances
	WHERE	
	 	Id = @Id
*/

BEGIN

	DECLARE @DateNow datetime2(7) = GETUTCDATE()

	UPDATE	
	 	[dbo].[TestInstances]
	SET		
	 	[TestId] = @TestId
		,[UserId] = @UserId
		,[DateModified] = @DateNow
		,[StatusId] = @StatusId
	WHERE	
	 	Id = @Id

END
GO
