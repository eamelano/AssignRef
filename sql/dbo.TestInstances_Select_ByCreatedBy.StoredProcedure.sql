GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Edgar Melano
-- Create date: 2023 / 04 / 17
-- Description: Paginated select filtered by UserId
-- Code Reviewer: Sydney Wells

-- MODIFIED BY:
-- MODIFIED DATE:
-- Code Reviewer:
-- Note:
-- =============================================
CREATE PROC [dbo].[TestInstances_Select_ByCreatedBy]
	@UserId int
	,@PageIndex int
	,@PageSize int

as

/*
	DECLARE	
		@UserId int = 8
		,@PageIndex int = 0
		,@PageSize int = 10

	EXECUTE 
		dbo.TestInstances_Select_ByCreatedBy
			@UserId
			,@PageIndex
			,@PageSize
*/

BEGIN

	DECLARE @Offset int = @PageIndex * @PageSize

	SELECT	
		ti.[Id] as InstanceId
		,t.[Id] as TestId
		,t.[Name] as TestName
		,tt.[Id] as TestTypeId
		,tt.[Name] as TestTypeName
		,u.[Id]
		,u.[FirstName]
		,u.[LastName]
		,u.[Email]
		,u.[AvatarUrl]
		,TotalCount = COUNT(1) OVER()
	FROM	
		dbo.TestInstances as ti	
			inner join dbo.Tests as t
				ON ti.TestId = t.Id 
			inner join dbo.TestTypes as tt
				ON t.TestTypeId = tt.Id 
			inner join Dbo.Users as u
				ON ti.UserId = u.Id
	WHERE	
		ti.UserId = @UserId
	ORDER BY	
		ti.UserId

	OFFSET	@Offset ROWS
	FETCH NEXT @PageSize ROWS ONLY

END
GO
