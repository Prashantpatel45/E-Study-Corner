USE [EStudyDB]
GO
/****** Object:  Table [dbo].[AdminAssignmentMaster]    Script Date: 23-09-2023 19:53:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AdminAssignmentMaster](
	[Upload_Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](150) NULL,
	[Description] [varchar](400) NULL,
	[FileName] [varchar](200) NULL,
	[Upload_DT] [datetime] NULL,
 CONSTRAINT [PK_AdminAssignmentMaster] PRIMARY KEY CLUSTERED 
(
	[Upload_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EnquiryMaster]    Script Date: 23-09-2023 19:53:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EnquiryMaster](
	[Enquiry_Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](150) NULL,
	[Email] [varchar](150) NULL,
	[Mobile_No] [varchar](15) NULL,
	[Message] [varchar](max) NULL,
	[Enquiry_DT] [datetime] NULL,
 CONSTRAINT [PK_EnquiryMaster] PRIMARY KEY CLUSTERED 
(
	[Enquiry_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FeedbackMaster]    Script Date: 23-09-2023 19:53:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FeedbackMaster](
	[Feedback_Id] [int] IDENTITY(1,1) NOT NULL,
	[Topic] [varchar](100) NULL,
	[Message] [varchar](300) NULL,
	[UserId] [varchar](150) NULL,
	[Feedback_DT] [datetime] NULL,
 CONSTRAINT [PK_FeedbackMaster] PRIMARY KEY CLUSTERED 
(
	[Feedback_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LoginMaster]    Script Date: 23-09-2023 19:53:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoginMaster](
	[EmailId] [varchar](150) NOT NULL,
	[Pass] [varchar](360) NULL,
	[Utype] [varchar](150) NULL,
 CONSTRAINT [PK_LoginMaster] PRIMARY KEY CLUSTERED 
(
	[EmailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NotificationMaster]    Script Date: 23-09-2023 19:53:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotificationMaster](
	[Notification_Id] [int] IDENTITY(1,1) NOT NULL,
	[Noti_Message] [varchar](150) NULL,
	[Noti_Dt] [varchar](100) NULL,
 CONSTRAINT [PK_NotificationMaster] PRIMARY KEY CLUSTERED 
(
	[Notification_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StudyMaster]    Script Date: 23-09-2023 19:53:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StudyMaster](
	[Name] [varchar](150) NULL,
	[DOB] [varchar](160) NULL,
	[Gender] [varchar](50) NULL,
	[EmailId] [varchar](150) NOT NULL,
	[CollegeName] [varchar](200) NULL,
	[Course] [varchar](15) NULL,
	[Yearofcourse] [int] NULL,
	[MobileNo] [varchar](20) NULL,
	[UserPic] [varchar](300) NULL,
	[Address] [varchar](400) NULL,
	[Registered_On] [datetime] NULL,
 CONSTRAINT [PK_StudyMaster] PRIMARY KEY CLUSTERED 
(
	[EmailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UploadAssignmentMaster]    Script Date: 23-09-2023 19:53:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UploadAssignmentMaster](
	[Upload_Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](150) NULL,
	[Description] [varchar](400) NULL,
	[FileName] [varchar](200) NULL,
	[Upload_DT] [datetime] NULL,
	[Uploaded_By] [varchar](150) NOT NULL,
 CONSTRAINT [PK_UploadAssignmentMaster] PRIMARY KEY CLUSTERED 
(
	[Upload_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UploadStudyMaterial]    Script Date: 23-09-2023 19:53:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UploadStudyMaterial](
	[Materail_Id] [int] IDENTITY(1,1) NOT NULL,
	[Subject] [varchar](70) NULL,
	[Title] [varchar](150) NULL,
	[Description] [varchar](400) NULL,
	[FileName] [varchar](200) NULL,
	[Upload_DT] [datetime] NULL,
 CONSTRAINT [PK_UploadStudyMaterial] PRIMARY KEY CLUSTERED 
(
	[Materail_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[FeedbackMaster]  WITH CHECK ADD  CONSTRAINT [FK_FeedbackMaster_StudyMaster] FOREIGN KEY([UserId])
REFERENCES [dbo].[StudyMaster] ([EmailId])
GO
ALTER TABLE [dbo].[FeedbackMaster] CHECK CONSTRAINT [FK_FeedbackMaster_StudyMaster]
GO
ALTER TABLE [dbo].[UploadAssignmentMaster]  WITH CHECK ADD  CONSTRAINT [FK_UploadAssignmentMaster_StudyMaster] FOREIGN KEY([Uploaded_By])
REFERENCES [dbo].[StudyMaster] ([EmailId])
GO
ALTER TABLE [dbo].[UploadAssignmentMaster] CHECK CONSTRAINT [FK_UploadAssignmentMaster_StudyMaster]
GO
