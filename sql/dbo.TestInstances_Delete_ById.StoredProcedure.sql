GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Edgar Melano
-- Create date: 2023/04/12
-- Description:	Soft Delete for TestInstances table. 
-- Code Reviewer: Sydney Wells

-- MODIFIED BY: 
-- MODIFIED DATE:
-- Code Reviewer:
-- Note:
-- =============================================
CREATE PROC [dbo].[TestInstances_Delete_ById]
	@Id int

as
/*
	DECLARE	
		@Id int = 32

	SELECT	
		Id as InstanceId
		,TestId
		,UserId
		,StatusId
		,DateModified
	FROM	
		dbo.TestInstances
	WHERE	
		Id = @Id

	EXECUTE	
		dbo.TestInstances_Delete_ById
			@Id

	SELECT	
		Id as InstanceId
		,TestId
		,UserId
		,StatusId
		,DateModified
	FROM	
		dbo.TestInstances
	WHERE	
		Id = @Id
*/
BEGIN
	
	DECLARE	
		@DateNow datetime2(7) = GETUTCDATE();

	UPDATE	
		dbo.TestInstances
	SET
		StatusId = 0
		,DateModified = @DateNow
	WHERE	
		Id = @Id
		
END
GO
