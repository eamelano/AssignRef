USE [AssignRef]
GO
/****** Object:  UserDefinedTableType [dbo].[AnswerOptions_Batch]    Script Date: 4/28/2023 4:26:01 PM ******/
CREATE TYPE [dbo].[AnswerOptions_Batch] AS TABLE(
	[Text] [nvarchar](500) NULL,
	[Value] [nvarchar](100) NULL,
	[AdditionalInfo] [nvarchar](200) NULL
)
GO
