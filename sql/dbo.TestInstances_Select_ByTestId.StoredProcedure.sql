GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Edgar Melano
-- Create date: 2023 / 04 / 17
-- Description: Select By TestId
-- Code Reviewer: Sydney Wells

-- MODIFIED BY: 
-- MODIFIED DATE: 
-- Code Reviewer:
-- Note:
-- =============================================
CREATE PROC [dbo].[TestInstances_Select_ByTestId]
	@Id int
	,@PageIndex int
	,@PageSize int

as

/*
	DECLARE 
		@Id int = 10
		,@PageIndex int = 0
		,@PageSize int = 10

	EXECUTE	
		dbo.TestInstances_Select_ByTestId
			@Id
			,@PageIndex
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
				on ti.TestId = t.Id
			inner join dbo.TestTypes as tt
				on t.TestTypeId = tt.Id
			inner join dbo.Users as u
				on ti.UserId = u.Id
	WHERE	
		t.Id = @Id
	ORDER BY	
		ti.Id

	OFFSET	@Offset ROWS
	FETCH NEXT @PageSize ROWS ONLY

END
GO
