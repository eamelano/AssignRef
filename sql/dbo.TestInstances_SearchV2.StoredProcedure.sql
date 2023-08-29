GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Edgar Melano
-- Create date: 2023 / 05 / 03
-- Description: Paginated search
-- Code Reviewer: Sydney Wells

-- MODIFIED BY:
-- MODIFIED DATE:
-- Code Reviewer:
-- Note:
-- =============================================

CREATE PROC	[dbo].[TestInstances_SearchV2]
	@PageIndex int
	,@PageSize int
	,@Query nvarchar(50) = Null
	,@StartDate datetime2(7) = Null
	,@EndDate datetime2(7) = Null

as

/*
		DECLARE	
			@PageIndex int = 0
			,@PageSize int = 30
			,@Query nvarchar(50) = ''

		EXECUTE 
			dbo.TestInstances_SearchV2
				@PageIndex
				,@PageSize
				,@Query
*/

BEGIN

	DECLARE @Offset int = @PageIndex * @PageSize

	SELECT	
		ti.[Id] as instanceId
		,ti.DateCreated
		,st.Id
		,st.[Name]
		,t.[Id] as testId
		,t.[Name] as testName
		,tt.[Id] as testTypeId
		,tt.[Name] as testTypeName
		,u.[Id]
		,u.[FirstName]
		,u.[LastName]
		,u.[Email]
		,u.[AvatarUrl]
		,QuestionCount = 
			(
			SELECT 
				COUNT(tq.Question)
			FROM
				dbo.TestQuestions as tq
			WHERE
				tq.TestId = ti.TestId
			)
		,CorrectAnswers = 
			(
			SELECT 
				COUNT(tqao.IsCorrect)
			FROM
				dbo.TestQuestionAnswerOptions as tqao
					inner join dbo.TestAnswers as ta
						ON tqao.Id = ta.AnswerOptionId
			WHERE
				ta.InstanceId = ti.Id AND tqao.IsCorrect = 1
			)
		,TotalCount = COUNT(1) OVER()
	FROM	
		dbo.TestInstances as ti	
			inner join dbo.Tests as t 
				ON ti.TestId = t.Id 
			inner join	dbo.TestTypes as tt 
				ON t.TestTypeId = tt.Id 
			inner join dbo.Users as u 
				ON ti.UserId = u.Id
			inner join dbo.StatusTypes as st 
				ON ti.StatusId = st.Id
	WHERE	
		(u.Id LIKE '%' + @Query + '%' OR 
		u.[FirstName] LIKE '%' + @Query + '%' OR
		u.[LastName] LIKE '%' + @Query + '%' OR
		tt.[Name] LIKE '%' + @Query + '%' OR
		st.[Name] LIKE '%' + @Query + '%' OR
		@Query IS NULL) AND
		(@StartDate IS NULL OR 
		@EndDate IS NULL OR 
		ti.DateCreated BETWEEN @StartDate AND @EndDate)
			
	ORDER BY	
		ti.UserId
	
	OFFSET	@Offset ROWS
	FETCH NEXT @PageSize ROWS ONLY

END
GO
