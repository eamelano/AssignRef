GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Edgar Melano
-- Create date: 2023 / 04 / 17
-- Description: Paginated select all
-- Code Reviewer: Sydney Wells

-- MODIFIED BY:
-- MODIFIED DATE:
-- Code Reviewer:
-- Note:
-- =============================================
CREATE PROC	[dbo].[TestInstances_SelectAll]
	@PageIndex int
	,@PageSize int

as

/*
	DECLARE	
		@PageIndex int = 0
		,@PageSize int = 30

	EXECUTE 
		dbo.TestInstances_SelectAll
			@PageIndex
			,@PageSize
*/

BEGIN

	DECLARE @Offset int = @PageIndex * @PageSize

	SELECT	
		ti.[Id] as instanceId
		,t.[Id] as testId
		,t.[Name] as testName
		,tt.[Id] as testTypeId
		,tt.[Name] as testTypeName
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
	ORDER BY	
		ti.UserId

	OFFSET	@Offset ROWS
	FETCH NEXT @PageSize ROWS ONLY

END
GO
