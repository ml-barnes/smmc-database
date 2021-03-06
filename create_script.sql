USE [IN705_2018S2_db3shared01]
GO
/****** Object:  Table [dbo].[Contact]    Script Date: 25/11/2018 10:21:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contact](
	[Contact ID] [int] IDENTITY(0,1) NOT NULL,
	[Email] [varchar](320) NOT NULL,
	[Phone] [varchar](15) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Contact ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Enrollment]    Script Date: 25/11/2018 10:21:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Enrollment](
	[Enrollment ID] [int] IDENTITY(0,1) NOT NULL,
	[Student ID] [int] NOT NULL,
	[Instrument ID] [int] NOT NULL,
	[Grade] [int] NOT NULL,
	[Date] [date] NOT NULL,
	[Paid] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Enrollment ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UC_Enrollment] UNIQUE NONCLUSTERED 
(
	[Student ID] ASC,
	[Instrument ID] ASC,
	[Grade] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Enrollment Ensemble]    Script Date: 25/11/2018 10:21:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Enrollment Ensemble](
	[Enrollment ID] [int] NOT NULL,
	[Ensemble ID] [int] NOT NULL,
 CONSTRAINT [PK_EnrollmentEnsemble] PRIMARY KEY CLUSTERED 
(
	[Enrollment ID] ASC,
	[Ensemble ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Enrollment MusicClass]    Script Date: 25/11/2018 10:21:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Enrollment MusicClass](
	[Enrollment ID] [int] NOT NULL,
	[MusicClass ID] [int] NOT NULL,
	[Date] [date] NOT NULL,
 CONSTRAINT [PK_Enrollment_MusicClass] PRIMARY KEY CLUSTERED 
(
	[Enrollment ID] ASC,
	[MusicClass ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ensemble]    Script Date: 25/11/2018 10:21:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ensemble](
	[Ensemble ID] [int] IDENTITY(0,1) NOT NULL,
	[Type] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Ensemble ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ensemble Performance]    Script Date: 25/11/2018 10:21:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ensemble Performance](
	[Ensemble ID] [int] NOT NULL,
	[Performance ID] [int] NOT NULL,
 CONSTRAINT [PK_EnsemblePerformance] PRIMARY KEY CLUSTERED 
(
	[Ensemble ID] ASC,
	[Performance ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Guardian]    Script Date: 25/11/2018 10:21:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Guardian](
	[Guardian ID] [int] IDENTITY(0,1) NOT NULL,
	[Contact ID] [int] NOT NULL,
	[Person ID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Guardian ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UC_Contact_Guardian] UNIQUE NONCLUSTERED 
(
	[Contact ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UC_Person_Guardian] UNIQUE NONCLUSTERED 
(
	[Person ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Instrument]    Script Date: 25/11/2018 10:21:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Instrument](
	[Instrument ID] [int] IDENTITY(0,1) NOT NULL,
	[Instrument] [varchar](20) NOT NULL,
	[Student Fee] [int] NOT NULL,
	[Open Fee] [int] NULL,
	[Hire Fee] [int] NULL,
	[Head Tutor] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Instrument ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Instrument Inventory]    Script Date: 25/11/2018 10:21:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Instrument Inventory](
	[Instrument Inventory ID] [int] IDENTITY(0,1) NOT NULL,
	[Instrument ID] [int] NOT NULL,
	[Purchase Date] [date] NOT NULL,
	[Manufacturer] [varchar](100) NOT NULL,
	[Cost] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Instrument Inventory ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Lesson Time]    Script Date: 25/11/2018 10:21:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Lesson Time](
	[Lesson Time ID] [int] IDENTITY(850,50) NOT NULL,
	[Time] [time](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Lesson Time ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Loan]    Script Date: 25/11/2018 10:21:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Loan](
	[Instrument Inventory ID] [int] NOT NULL,
	[Enrollment ID] [int] NOT NULL,
	[Date Loaned] [datetime] NOT NULL,
	[Date Returned] [date] NULL,
 CONSTRAINT [PK_Loan] PRIMARY KEY CLUSTERED 
(
	[Instrument Inventory ID] ASC,
	[Enrollment ID] ASC,
	[Date Loaned] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Local Musicians]    Script Date: 25/11/2018 10:21:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Local Musicians](
	[Local Musicians ID] [int] IDENTITY(0,1) NOT NULL,
	[Contact ID] [int] NOT NULL,
	[First Name] [varchar](100) NOT NULL,
	[Last Name] [varchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Local Musicians ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UC_Contact_LocalMusicians] UNIQUE NONCLUSTERED 
(
	[Contact ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Local Musicians Ensemble]    Script Date: 25/11/2018 10:21:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Local Musicians Ensemble](
	[Local Musicians Ensemble ID] [int] IDENTITY(0,1) NOT NULL,
	[Local Musicians ID] [int] NOT NULL,
	[Ensemble ID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Local Musicians Ensemble ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MusicClass]    Script Date: 25/11/2018 10:21:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MusicClass](
	[MusicClass ID] [int] IDENTITY(0,1) NOT NULL,
	[Tutor ID] [int] NULL,
	[Lesson Time ID] [int] NULL,
	[Start Date] [date] NOT NULL,
	[End Date] [date] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MusicClass ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payroll]    Script Date: 25/11/2018 10:21:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payroll](
	[Payroll ID] [int] IDENTITY(0,1) NOT NULL,
	[Staff ID] [int] NOT NULL,
	[Date] [date] NOT NULL,
	[Amount] [int] NOT NULL,
	[Notes] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Payroll ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Performance]    Script Date: 25/11/2018 10:21:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Performance](
	[Performance ID] [int] IDENTITY(0,1) NOT NULL,
	[Date] [date] NOT NULL,
	[Time] [time](7) NOT NULL,
	[Venue ID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Performance ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Performance Piece]    Script Date: 25/11/2018 10:21:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Performance Piece](
	[Performance Piece ID] [int] IDENTITY(0,1) NOT NULL,
	[Performance ID] [int] NOT NULL,
	[Piece ID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Performance Piece ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Person]    Script Date: 25/11/2018 10:21:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Person](
	[Person ID] [int] IDENTITY(0,1) NOT NULL,
	[First Name] [varchar](250) NOT NULL,
	[Last Name] [varchar](250) NOT NULL,
	[DOB] [date] NOT NULL,
	[Address] [varchar](750) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Person ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Piece]    Script Date: 25/11/2018 10:21:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Piece](
	[Piece ID] [int] IDENTITY(0,1) NOT NULL,
	[Title] [varchar](200) NOT NULL,
	[Composer] [varchar](200) NOT NULL,
	[Last Performed Date] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[Piece ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Repairs]    Script Date: 25/11/2018 10:21:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Repairs](
	[Instrument Inventory ID] [int] NOT NULL,
	[Technician ID] [int] NOT NULL,
	[Repair Start] [datetime] NOT NULL,
	[Notes] [varchar](max) NOT NULL,
	[Repair End] [date] NULL,
 CONSTRAINT [PK_Repairs] PRIMARY KEY CLUSTERED 
(
	[Instrument Inventory ID] ASC,
	[Technician ID] ASC,
	[Repair Start] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SheetMusic]    Script Date: 25/11/2018 10:21:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SheetMusic](
	[Sheet Music ID] [int] IDENTITY(0,1) NOT NULL,
	[Title] [varchar](200) NOT NULL,
	[Composer] [varchar](200) NOT NULL,
	[Amount] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Sheet Music ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SheetMusic Tutor]    Script Date: 25/11/2018 10:21:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SheetMusic Tutor](
	[Sheet Music Tutor ID] [int] IDENTITY(0,1) NOT NULL,
	[Sheet Music ID] [int] NOT NULL,
	[Tutor ID] [int] NOT NULL,
	[Amount Loaned] [int] NOT NULL,
	[Date Loaned] [date] NOT NULL,
	[Date Returned] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[Sheet Music Tutor ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Staff]    Script Date: 25/11/2018 10:21:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Staff](
	[Staff ID] [int] IDENTITY(0,1) NOT NULL,
	[Person ID] [int] NOT NULL,
	[Contact ID] [int] NOT NULL,
	[Start Date] [date] NOT NULL,
	[Left Date] [date] NULL,
	[Hours] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Staff ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UC_Contact_Staff] UNIQUE NONCLUSTERED 
(
	[Contact ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UC_Person_Staff] UNIQUE NONCLUSTERED 
(
	[Person ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Student]    Script Date: 25/11/2018 10:21:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Student](
	[Student ID] [int] IDENTITY(10000,1) NOT NULL,
	[Person ID] [int] NOT NULL,
	[Contact ID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Student ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UC_Student_Person] UNIQUE NONCLUSTERED 
(
	[Person ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Student Guardian]    Script Date: 25/11/2018 10:21:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Student Guardian](
	[Student ID] [int] NOT NULL,
	[Guardian ID] [int] NOT NULL,
 CONSTRAINT [PK_StudentGuardian] PRIMARY KEY CLUSTERED 
(
	[Student ID] ASC,
	[Guardian ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Technician]    Script Date: 25/11/2018 10:21:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Technician](
	[Technician ID] [int] IDENTITY(0,1) NOT NULL,
	[Staff ID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Technician ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tutor]    Script Date: 25/11/2018 10:21:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tutor](
	[Tutor ID] [int] IDENTITY(0,1) NOT NULL,
	[Staff ID] [int] NOT NULL,
	[ATCL] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Tutor ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tutor Type]    Script Date: 25/11/2018 10:21:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tutor Type](
	[Tutor Type ID] [int] IDENTITY(0,1) NOT NULL,
	[Tutor ID] [int] NOT NULL,
	[Instrument ID] [int] NOT NULL,
	[Max Grade] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Tutor Type ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UNIQUE_Tutor_Type] UNIQUE NONCLUSTERED 
(
	[Tutor ID] ASC,
	[Instrument ID] ASC,
	[Max Grade] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Venue]    Script Date: 25/11/2018 10:21:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Venue](
	[Venue ID] [int] IDENTITY(0,1) NOT NULL,
	[Venue Name] [varchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Venue ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Enrollment]  WITH CHECK ADD  CONSTRAINT [FK_Enrollment_Instrument] FOREIGN KEY([Instrument ID])
REFERENCES [dbo].[Instrument] ([Instrument ID])
GO
ALTER TABLE [dbo].[Enrollment] CHECK CONSTRAINT [FK_Enrollment_Instrument]
GO
ALTER TABLE [dbo].[Enrollment]  WITH CHECK ADD  CONSTRAINT [FK_Enrollment_Student] FOREIGN KEY([Student ID])
REFERENCES [dbo].[Student] ([Student ID])
GO
ALTER TABLE [dbo].[Enrollment] CHECK CONSTRAINT [FK_Enrollment_Student]
GO
ALTER TABLE [dbo].[Enrollment Ensemble]  WITH CHECK ADD  CONSTRAINT [FK_EnrollmentEnsemble_Enrollment] FOREIGN KEY([Enrollment ID])
REFERENCES [dbo].[Enrollment] ([Enrollment ID])
GO
ALTER TABLE [dbo].[Enrollment Ensemble] CHECK CONSTRAINT [FK_EnrollmentEnsemble_Enrollment]
GO
ALTER TABLE [dbo].[Enrollment Ensemble]  WITH CHECK ADD  CONSTRAINT [FK_EnrollmentEnsemble_Ensemble] FOREIGN KEY([Ensemble ID])
REFERENCES [dbo].[Ensemble] ([Ensemble ID])
GO
ALTER TABLE [dbo].[Enrollment Ensemble] CHECK CONSTRAINT [FK_EnrollmentEnsemble_Ensemble]
GO
ALTER TABLE [dbo].[Enrollment MusicClass]  WITH CHECK ADD  CONSTRAINT [FK_Enrollment_MusicClass_Enrollment] FOREIGN KEY([Enrollment ID])
REFERENCES [dbo].[Enrollment] ([Enrollment ID])
GO
ALTER TABLE [dbo].[Enrollment MusicClass] CHECK CONSTRAINT [FK_Enrollment_MusicClass_Enrollment]
GO
ALTER TABLE [dbo].[Enrollment MusicClass]  WITH CHECK ADD  CONSTRAINT [FK_Enrollment_MusicClass_MusicClass] FOREIGN KEY([MusicClass ID])
REFERENCES [dbo].[MusicClass] ([MusicClass ID])
GO
ALTER TABLE [dbo].[Enrollment MusicClass] CHECK CONSTRAINT [FK_Enrollment_MusicClass_MusicClass]
GO
ALTER TABLE [dbo].[Ensemble Performance]  WITH CHECK ADD  CONSTRAINT [FK_EnsemblePerformance_Ensemble] FOREIGN KEY([Ensemble ID])
REFERENCES [dbo].[Ensemble] ([Ensemble ID])
GO
ALTER TABLE [dbo].[Ensemble Performance] CHECK CONSTRAINT [FK_EnsemblePerformance_Ensemble]
GO
ALTER TABLE [dbo].[Ensemble Performance]  WITH CHECK ADD  CONSTRAINT [FK_EnsemblePerformance_MusicClass] FOREIGN KEY([Performance ID])
REFERENCES [dbo].[Performance] ([Performance ID])
GO
ALTER TABLE [dbo].[Ensemble Performance] CHECK CONSTRAINT [FK_EnsemblePerformance_MusicClass]
GO
ALTER TABLE [dbo].[Guardian]  WITH CHECK ADD  CONSTRAINT [FK_Guardian_Contact] FOREIGN KEY([Contact ID])
REFERENCES [dbo].[Contact] ([Contact ID])
GO
ALTER TABLE [dbo].[Guardian] CHECK CONSTRAINT [FK_Guardian_Contact]
GO
ALTER TABLE [dbo].[Guardian]  WITH CHECK ADD  CONSTRAINT [FK_Guardian_Person] FOREIGN KEY([Guardian ID])
REFERENCES [dbo].[Person] ([Person ID])
GO
ALTER TABLE [dbo].[Guardian] CHECK CONSTRAINT [FK_Guardian_Person]
GO
ALTER TABLE [dbo].[Instrument]  WITH CHECK ADD  CONSTRAINT [FK_Instrument_Tutor] FOREIGN KEY([Head Tutor])
REFERENCES [dbo].[Tutor] ([Tutor ID])
GO
ALTER TABLE [dbo].[Instrument] CHECK CONSTRAINT [FK_Instrument_Tutor]
GO
ALTER TABLE [dbo].[Instrument Inventory]  WITH CHECK ADD  CONSTRAINT [FK_Instrument_Inventory_Instrument] FOREIGN KEY([Instrument ID])
REFERENCES [dbo].[Instrument] ([Instrument ID])
GO
ALTER TABLE [dbo].[Instrument Inventory] CHECK CONSTRAINT [FK_Instrument_Inventory_Instrument]
GO
ALTER TABLE [dbo].[Loan]  WITH CHECK ADD  CONSTRAINT [FK_Loan_Enrollment] FOREIGN KEY([Enrollment ID])
REFERENCES [dbo].[Enrollment] ([Enrollment ID])
GO
ALTER TABLE [dbo].[Loan] CHECK CONSTRAINT [FK_Loan_Enrollment]
GO
ALTER TABLE [dbo].[Loan]  WITH CHECK ADD  CONSTRAINT [FK_Loan_InstrumentInventory] FOREIGN KEY([Instrument Inventory ID])
REFERENCES [dbo].[Instrument Inventory] ([Instrument Inventory ID])
GO
ALTER TABLE [dbo].[Loan] CHECK CONSTRAINT [FK_Loan_InstrumentInventory]
GO
ALTER TABLE [dbo].[Local Musicians]  WITH CHECK ADD  CONSTRAINT [FK_LocalMusicians_Contact] FOREIGN KEY([Contact ID])
REFERENCES [dbo].[Contact] ([Contact ID])
GO
ALTER TABLE [dbo].[Local Musicians] CHECK CONSTRAINT [FK_LocalMusicians_Contact]
GO
ALTER TABLE [dbo].[Local Musicians Ensemble]  WITH CHECK ADD  CONSTRAINT [FK_LocalMusiciansEnsemble_Ensemble] FOREIGN KEY([Ensemble ID])
REFERENCES [dbo].[Ensemble] ([Ensemble ID])
GO
ALTER TABLE [dbo].[Local Musicians Ensemble] CHECK CONSTRAINT [FK_LocalMusiciansEnsemble_Ensemble]
GO
ALTER TABLE [dbo].[Local Musicians Ensemble]  WITH CHECK ADD  CONSTRAINT [FK_LocalMusiciansEnsemble_LocalMusicians] FOREIGN KEY([Local Musicians ID])
REFERENCES [dbo].[Local Musicians] ([Local Musicians ID])
GO
ALTER TABLE [dbo].[Local Musicians Ensemble] CHECK CONSTRAINT [FK_LocalMusiciansEnsemble_LocalMusicians]
GO
ALTER TABLE [dbo].[MusicClass]  WITH CHECK ADD  CONSTRAINT [FK_MusicClass_Lesson_Time] FOREIGN KEY([Lesson Time ID])
REFERENCES [dbo].[Lesson Time] ([Lesson Time ID])
GO
ALTER TABLE [dbo].[MusicClass] CHECK CONSTRAINT [FK_MusicClass_Lesson_Time]
GO
ALTER TABLE [dbo].[MusicClass]  WITH CHECK ADD  CONSTRAINT [FK_MusicClass_Tutor] FOREIGN KEY([Tutor ID])
REFERENCES [dbo].[Tutor] ([Tutor ID])
GO
ALTER TABLE [dbo].[MusicClass] CHECK CONSTRAINT [FK_MusicClass_Tutor]
GO
ALTER TABLE [dbo].[Payroll]  WITH CHECK ADD  CONSTRAINT [FK_Payroll_Staff] FOREIGN KEY([Staff ID])
REFERENCES [dbo].[Staff] ([Staff ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Payroll] CHECK CONSTRAINT [FK_Payroll_Staff]
GO
ALTER TABLE [dbo].[Performance]  WITH CHECK ADD  CONSTRAINT [FK_Performance_Venue] FOREIGN KEY([Venue ID])
REFERENCES [dbo].[Venue] ([Venue ID])
GO
ALTER TABLE [dbo].[Performance] CHECK CONSTRAINT [FK_Performance_Venue]
GO
ALTER TABLE [dbo].[Performance Piece]  WITH CHECK ADD  CONSTRAINT [FK_Performance_Piece_Performance] FOREIGN KEY([Performance ID])
REFERENCES [dbo].[Performance] ([Performance ID])
GO
ALTER TABLE [dbo].[Performance Piece] CHECK CONSTRAINT [FK_Performance_Piece_Performance]
GO
ALTER TABLE [dbo].[Performance Piece]  WITH CHECK ADD  CONSTRAINT [FK_Performance_Piece_Piece] FOREIGN KEY([Piece ID])
REFERENCES [dbo].[Piece] ([Piece ID])
GO
ALTER TABLE [dbo].[Performance Piece] CHECK CONSTRAINT [FK_Performance_Piece_Piece]
GO
ALTER TABLE [dbo].[Repairs]  WITH CHECK ADD  CONSTRAINT [FK_Repairs_InstrumentInventory] FOREIGN KEY([Instrument Inventory ID])
REFERENCES [dbo].[Instrument Inventory] ([Instrument Inventory ID])
GO
ALTER TABLE [dbo].[Repairs] CHECK CONSTRAINT [FK_Repairs_InstrumentInventory]
GO
ALTER TABLE [dbo].[Repairs]  WITH CHECK ADD  CONSTRAINT [FK_Repairs_Technician] FOREIGN KEY([Technician ID])
REFERENCES [dbo].[Technician] ([Technician ID])
GO
ALTER TABLE [dbo].[Repairs] CHECK CONSTRAINT [FK_Repairs_Technician]
GO
ALTER TABLE [dbo].[SheetMusic Tutor]  WITH CHECK ADD  CONSTRAINT [FK_SheetMusic_Tutor_SheetMusic] FOREIGN KEY([Sheet Music ID])
REFERENCES [dbo].[SheetMusic] ([Sheet Music ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SheetMusic Tutor] CHECK CONSTRAINT [FK_SheetMusic_Tutor_SheetMusic]
GO
ALTER TABLE [dbo].[SheetMusic Tutor]  WITH CHECK ADD  CONSTRAINT [FK_SheetMusic_Tutor_Tutor] FOREIGN KEY([Tutor ID])
REFERENCES [dbo].[Tutor] ([Tutor ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SheetMusic Tutor] CHECK CONSTRAINT [FK_SheetMusic_Tutor_Tutor]
GO
ALTER TABLE [dbo].[Staff]  WITH CHECK ADD  CONSTRAINT [FK_Staff_Contact] FOREIGN KEY([Contact ID])
REFERENCES [dbo].[Contact] ([Contact ID])
GO
ALTER TABLE [dbo].[Staff] CHECK CONSTRAINT [FK_Staff_Contact]
GO
ALTER TABLE [dbo].[Staff]  WITH CHECK ADD  CONSTRAINT [FK_Staff_Person] FOREIGN KEY([Person ID])
REFERENCES [dbo].[Person] ([Person ID])
GO
ALTER TABLE [dbo].[Staff] CHECK CONSTRAINT [FK_Staff_Person]
GO
ALTER TABLE [dbo].[Student]  WITH CHECK ADD  CONSTRAINT [FK_Student_Contact] FOREIGN KEY([Contact ID])
REFERENCES [dbo].[Contact] ([Contact ID])
GO
ALTER TABLE [dbo].[Student] CHECK CONSTRAINT [FK_Student_Contact]
GO
ALTER TABLE [dbo].[Student]  WITH CHECK ADD  CONSTRAINT [FK_Student_Person] FOREIGN KEY([Person ID])
REFERENCES [dbo].[Person] ([Person ID])
GO
ALTER TABLE [dbo].[Student] CHECK CONSTRAINT [FK_Student_Person]
GO
ALTER TABLE [dbo].[Student Guardian]  WITH CHECK ADD  CONSTRAINT [FK_StudentGuardian_Guardian] FOREIGN KEY([Guardian ID])
REFERENCES [dbo].[Guardian] ([Guardian ID])
GO
ALTER TABLE [dbo].[Student Guardian] CHECK CONSTRAINT [FK_StudentGuardian_Guardian]
GO
ALTER TABLE [dbo].[Student Guardian]  WITH CHECK ADD  CONSTRAINT [FK_StudentGuardian_Student] FOREIGN KEY([Student ID])
REFERENCES [dbo].[Student] ([Student ID])
GO
ALTER TABLE [dbo].[Student Guardian] CHECK CONSTRAINT [FK_StudentGuardian_Student]
GO
ALTER TABLE [dbo].[Technician]  WITH CHECK ADD  CONSTRAINT [FK_Technician_Staff] FOREIGN KEY([Staff ID])
REFERENCES [dbo].[Staff] ([Staff ID])
GO
ALTER TABLE [dbo].[Technician] CHECK CONSTRAINT [FK_Technician_Staff]
GO
ALTER TABLE [dbo].[Tutor]  WITH CHECK ADD  CONSTRAINT [FK_Tutor_Staff] FOREIGN KEY([Staff ID])
REFERENCES [dbo].[Staff] ([Staff ID])
GO
ALTER TABLE [dbo].[Tutor] CHECK CONSTRAINT [FK_Tutor_Staff]
GO
ALTER TABLE [dbo].[Tutor Type]  WITH CHECK ADD  CONSTRAINT [FK_TutorType_Instrument] FOREIGN KEY([Instrument ID])
REFERENCES [dbo].[Instrument] ([Instrument ID])
GO
ALTER TABLE [dbo].[Tutor Type] CHECK CONSTRAINT [FK_TutorType_Instrument]
GO
ALTER TABLE [dbo].[Tutor Type]  WITH CHECK ADD  CONSTRAINT [FK_TutorType_Tutor] FOREIGN KEY([Tutor ID])
REFERENCES [dbo].[Tutor] ([Tutor ID])
GO
ALTER TABLE [dbo].[Tutor Type] CHECK CONSTRAINT [FK_TutorType_Tutor]
GO
ALTER TABLE [dbo].[Enrollment]  WITH CHECK ADD CHECK  (([Grade]=(8) OR [Grade]=(7) OR [Grade]=(6) OR [Grade]=(5) OR [Grade]=(4) OR [Grade]=(3) OR [Grade]=(2) OR [Grade]=(1) OR [Grade]=(0)))
GO
ALTER TABLE [dbo].[Tutor Type]  WITH CHECK ADD CHECK  (([Max Grade]=(8) OR [Max Grade]=(6)))
GO
