GO

CREATE TYPE [dbo].[AnswerOptions_Batch] AS TABLE(
	[Text] [nvarchar](500) NULL,
	[Value] [nvarchar](100) NULL,
	[AdditionalInfo] [nvarchar](200) NULL
)
GO
