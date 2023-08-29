GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TestInstances](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TestId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[DateCreated] [datetime2](7) NOT NULL,
	[DateModified] [datetime2](7) NOT NULL,
	[StatusId] [int] NOT NULL,
 CONSTRAINT [PK_TestInstances] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[TestInstances] ADD  CONSTRAINT [DF_TestInstances_DateCreated]  DEFAULT (getutcdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[TestInstances] ADD  CONSTRAINT [DF_TestInstances_DateModified]  DEFAULT (getutcdate()) FOR [DateModified]
GO
ALTER TABLE [dbo].[TestInstances] ADD  CONSTRAINT [DF_TestInstances_StatusId]  DEFAULT ((1)) FOR [StatusId]
GO
ALTER TABLE [dbo].[TestInstances]  WITH CHECK ADD  CONSTRAINT [FK_TestInstances_Tests] FOREIGN KEY([TestId])
REFERENCES [dbo].[Tests] ([Id])
GO
ALTER TABLE [dbo].[TestInstances] CHECK CONSTRAINT [FK_TestInstances_Tests]
GO
ALTER TABLE [dbo].[TestInstances]  WITH CHECK ADD  CONSTRAINT [FK_TestInstances_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[TestInstances] CHECK CONSTRAINT [FK_TestInstances_Users]
GO
