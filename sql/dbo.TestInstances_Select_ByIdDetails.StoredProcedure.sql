GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Edgar Melano
-- Create date: 2023 / 04 / 17
-- Description: Detailed select using Instance Id.
-- Code Reviewer: Sydney Wells

-- MODIFIED BY: 
-- MODIFIED DATE:
-- Code Reviewer:
-- Note:
-- =============================================
CREATE PROC [dbo].[TestInstances_Select_ByIdDetails]
	@Id int

as

/*
	DECLARE	
		@Id int = 30

	EXECUTE 
		dbo.TestInstances_Select_ByIdDetails
			@Id
*/

BEGIN

	SELECT	
		ti.[Id]
		,st.[Id] as statusId
		,st.[Name] as [status]
		,u.[Id] as UserId
		,u.[FirstName]
		,u.[LastName]
		,u.[Mi]
		,u.[AvatarUrl]
		,u.DateCreated
		,u.DateModified
		,t.[Id] as TestId
		,t.[Name] as TestName
		,t.[Description]
		,t.DateCreated
		,t.DateModified
		,t.CreatedBy
		,st2.Id as TestStatusId
		,st2.[Name] as TestStatus
		,tt.[Id] as [TestTypeId]
		,tt.[Name] as [TestTypeName]
		,Questions = 
			(
			SELECT DISTINCT	
				tq.[Id]
				,qt.[Id] AS [TypeId]
				,qt.[Name] as [Type]
				,tq.[Question]
				,tq.[HelpText]
				,tq.[IsRequired]
				,tq.[IsMultipleAllowed]
				,tq.[TestId]
				,st.[Id] as [StatusId]
				,st.[Name] as [Status]
				,tq.[SortOrder]
				,AnswerOptions =	
					(
					SELECT	
						tqa.[Id]
						,tqa.[QuestionId]
						,tqa.[Text]
						,tqa.[Value]
						,tqa.[AdditionalInfo]
					FROM	
						dbo.TestQuestionAnswerOptions AS tqa
					WHERE	
						tqa.QuestionId = tq.Id
					FOR JSON PATH
					)
				,Answers =	
					(
					SELECT
						ta.[Id]
						,ta.[QuestionId]
						,JSON_QUERY
							((
							SELECT 
								tqaos.[Id]
								,tqaos.[Text]
								,tqaos.[Value]
								,ta.Answer
							FROM	
								dbo.TestQuestionAnswerOptions AS tqaos
									inner join dbo.TestAnswers AS ta
										ON tqaos.Id = ta.AnswerOptionId
							WHERE	
								tqaos.[QuestionId] = tq.Id
							FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
							)) AS [AnswerOption]
					FROM		
						dbo.TestAnswers ta
					inner join dbo.TestQuestionAnswerOptions tqao
						ON ta.AnswerOptionId = tqao.Id
					WHERE	
						ta.QuestionId = tq.Id
					FOR JSON PATH
					)
			FROM	
				dbo.TestQuestions as tq 
					inner join dbo.QuestionTypes AS qt 
						ON tq.QuestionTypeId = qt.Id
					inner join dbo.StatusTypes AS st 
						ON st.Id = tq.StatusId
			WHERE	
				t.Id = tq.TestId
			FOR	JSON PATH
			)
	FROM	
		dbo.TestInstances as ti 
			inner join dbo.Users AS u 
				ON ti.UserId = u.Id
			inner join dbo.Tests AS t 
				ON ti.TestId = t.Id
			inner join dbo.TestTypes AS tt 
				ON t.TestTypeId = tt.Id
			inner join dbo.StatusTypes AS st 
				ON ti.StatusId = st.Id
			inner join dbo.StatusTypes AS st2 
				ON st2.Id = t.StatusId
	WHERE	
		ti.[Id] = @Id

END
GO
