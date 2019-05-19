------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--  Procedures
------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[AssignEnsembles]
AS 
BEGIN
	DELETE FROM [Enrollment Ensemble]
    SET NOCOUNT ON 
	DECLARE @StudentID int
	DECLARE @Grade int
	DECLARE @EnrollmentID int
	DECLARE @EnrollmentCount int

	SELECT * INTO #Students FROM Student

	WHILE exists (SELECT * FROM #Students)
	BEGIN
		SELECT TOP 1 @StudentID = [Student ID]
		FROM #Students

		SELECT DISTINCT * INTO #Enrollments FROM Enrollment WHERE [Student ID] = @StudentID AND Grade =(SELECT MAX(Grade) FROM Enrollment WHERE [Student ID] = @StudentID)

		SELECT @EnrollmentID = [Enrollment ID], @Grade = Grade
			FROM #Enrollments

		SELECT @EnrollmentCount = COUNT(*) FROM [Enrollment Ensemble] WHERE [Enrollment ID] = @EnrollmentID
		IF @EnrollmentCount = 0
			BEGIN
				IF @Grade >= 1 AND @Grade <= 3
					BEGIN
						BEGIN
							INSERT INTO [Enrollment Ensemble] VALUES (@EnrollmentID, 0)
						END
					END
				ELSE IF @Grade >= 4 AND @Grade <= 6
					BEGIN
						INSERT INTO [Enrollment Ensemble] VALUES (@EnrollmentID, 1)
					END
				ELSE IF @Grade >= 7 AND @Grade <= 8
					BEGIN
						INSERT INTO [Enrollment Ensemble] VALUES (@EnrollmentID, 2)
					END
			END
		DROP TABLE #Enrollments
		DELETE #Students WHERE [Student ID] = @StudentID
	END
	DROP TABLE #Students
END

------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


create procedure [Get Available Pieces] @performanceID int
as

DECLARE @years int = -3

select p.* from Piece p 
left join [Performance Piece] pp on pp.[Piece ID] = p.[Piece ID] 
left join [Performance] per on per.[Performance ID] = pp.[Performance ID]
where pp.[Performance ID] != @performanceID or pp.[Performance ID] is null
except
(select p.* from Piece p 
left join [Performance Piece] pp on pp.[Piece ID] = p.[Piece ID] 
where p.[Last Performed Date] > DATEADD(year, @years, GetDate()))

------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [GetEnrollment] @classID int
AS	

declare @gradeInstrument table 
(
    instrument int,
    grade int
)

insert into @gradeInstrument
select top 1 e.[Instrument ID], e.Grade
from MusicClass mc

join[Enrollment MusicClass] emc on mc.[MusicClass ID] = emc.[MusicClass ID]
join Enrollment e on e.[Enrollment ID] = emc.[Enrollment ID]
where mc.[MusicClass ID] = @classID
select e.* from Enrollment e
left join[Enrollment MusicClass] em on e.[Enrollment ID] = em.[Enrollment ID]
left join MusicClass mc on em.[MusicClass ID] = mc.[MusicClass ID]
join Student s on e.[Student ID] = s.[Student ID]
join Person p on s.[Person ID] = p.[Person ID]
where em.[Enrollment ID] is null 
and e.Grade in (select grade from @gradeInstrument)
and e.[Instrument ID] in (select instrument from @gradeInstrument)


------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Loan Instrument] @enrollmentID int
AS	
	
insert into Loan

	select distinct top 1 ii.[Instrument Inventory ID], @enrollmentID , GETDATE(), null -- Make null
	from Enrollment e 
	join Instrument i on e.[Instrument ID] = i.[Instrument ID]
	join [Instrument Inventory] ii on i.[Instrument ID] = ii.[Instrument ID]	
	where [Instrument Inventory ID] 
	not in		
		(select [Instrument Inventory ID] from Repairs 
			where[Repair End] is null 
				and (select count(Repairs.[Repair Start]) from Repairs) > 0) 
	and [Instrument Inventory ID]  not in
		(select [Instrument Inventory ID] from Loan 
			where[Date Returned] is null 
				and (select count(Loan.[Date Loaned]) from Loan) > 0)
	and [Enrollment ID] =  @enrollmentID

------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	
CREATE PROCEDURE [dbo].[paypeople]
AS 
BEGIN 
    SET NOCOUNT ON 
	DECLARE @TutorStaffID int
	DEClARE @TutorID int
	DECLARE @Max int
	DECLARE @TutorHours decimal
	DECLARE @Head int
	DECLARE @Technician int

	---- Paying Tutors ----
	SELECT * INTO #Tutors from Tutor

	WHILE exists (SELECT * FROM #Tutors)
	BEGIN
		SELECT TOP 1 @TutorStaffID = [Staff ID], @TutorID = [Tutor ID]
		FROM #Tutors
		
		SELECT @Head = COUNT(*) FROM Instrument WHERE [Head Tutor] = @TutorID
		SELECT @Technician = COUNT(*) FROM Technician WHERE [Staff ID] = @TutorStaffID
		SELECT @TutorHours = Hours FROM Staff WHERE [Staff ID] = @TutorStaffID
		SELECT @Max = MAX([Max Grade]) FROM [Tutor Type] WHERE [Tutor ID] = @TutorID

		IF (@Head > 0)
		BEGIN
			print(@Head)
			INSERT INTO Payroll VALUES (@TutorStaffID, GETDATE(), 30 * @TutorHours, 'Head tutor pay')
		END
		ELSE IF (@Technician IS NOT NULL)
		BEGIN
			INSERT INTO Payroll VALUES (@TutorStaffID, GETDATE(), 25 * @TutorHours, 'Technician & tutor pay')
		END
		ELSE IF (@Max IS NOT NULL)
		BEGIN
			IF (@Max > 6)
				BEGIN
					INSERT INTO Payroll VALUES (@TutorStaffID, GETDATE(), 25 * @TutorHours, 'Senior pay')
				END
			ELSE
				BEGIN
					INSERT INTO Payroll VALUES (@TutorStaffID, GETDATE(), 18 * @TutorHours, 'Junior pay')
				END
		END

		DELETE #Tutors WHERE [Staff ID] = @TutorStaffID
	END
	DROP TABLE #Tutors

	---- Paying Technicians ----
	DECLARE @TechStaffID int
	DEClARE @TechID int
	DECLARE @TechHours decimal
	DECLARE @Tutor int
	SELECT * INTO #Technician from Technician

	WHILE exists (SELECT * FROM #Technician)
	BEGIN
		SELECT TOP 1 @TechID = [Technician ID], @TechStaffID = [Staff ID]
		FROM #Technician

		SELECT @Tutor = COUNT(*) FROM Tutor WHERE [Staff ID] = @TechStaffID

		IF(@Tutor < 1)
		BEGIN
			SELECT @TechHours = Hours FROM Staff WHERE [Staff ID] = @TechStaffID
			INSERT INTO Payroll VALUES (@TechStaffID, GETDATE(), 25 * @TechHours, 'Technician Pay')
		END
		DELETE #Technician WHERE [Staff ID] = @TechStaffID
	END
	DROP TABLE #Technician

END

------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--  Triggers
------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE TRIGGER [dbo].[Create MusicClass]
   ON [dbo].[Enrollment]
   After INSERT
AS
BEGIN

	-- Declare variables to use as constants

	DECLARE @classStartSize int = 3
	DECLARE @classMaxSize int = 9
	
	DECLARE @insertedEnrollment TABLE 
	(
		[Enrollment ID] int,
		[Instrument ID] int,
		[Grade] int
	)	

	insert into @insertedEnrollment

	select [Enrollment ID], [Instrument ID], [Grade] from inserted	

	-- IF instrument is recorder then max class size changes

	if (select i.[Instrument] from @insertedEnrollment ie join Instrument i on ie.[Instrument ID] = i.[Instrument ID]) like 'Record%'
	begin
		set @classMaxSize = 17
	end	

	-- Check enrollment MusicClass for any MusicClasses that aren't full	

	DECLARE @notFullMusicClassIDs TABLE 
	(
		[MusicClass ID] int		
	)

	insert into @notFullMusicClassIDs

		select ec.[MusicClass ID]

			from [Enrollment MusicClass] ec join Enrollment e on ec.[Enrollment ID] = e.[Enrollment ID]

			where e.[Instrument ID] = (select [Instrument ID] from @insertedEnrollment) 
	
			and e.Grade = (select [Grade] from @insertedEnrollment) 

			group by ec.[MusicClass ID]

			having COUNT(ec.[MusicClass ID]) < @classMaxSize

	-- If there is no result, then there isn't a MusicClass to join. Do we need to create a MusicClass now?
	-- Check enrollments for any other count of Enrollments of same grade and instrument, who aren't already in enrollment MusicClass	

	if not exists (select [MusicClass ID] from @notFullMusicClassIDs)	

		BEGIN
			declare @enrollmentCount int

			select @enrollmentCount = count(e.[Enrollment ID]) 

				from Enrollment e left join [Enrollment MusicClass] ec on e.[Enrollment ID] = ec.[Enrollment ID]

				where [Instrument ID] = (select [Instrument ID] from @insertedEnrollment) 
	
				and Grade = (select [Grade] from @insertedEnrollment) 
	
				and ec.[Enrollment ID] is null			

			-- If count 3 we need to create a MusicClass

			if @enrollmentCount = @classStartSize

				BEGIN
					DECLARE @Date datetime

					-- Get dates for creating music class.

					DECLARE @first_saturday datetime
					DECLARE @sixth_saturday datetime
					DECLARE @sixth_last_saturday datetime

					-- Get next year
					DECLARE @Year int = YEAR(GETDATE())+1

					-- If we are in january, then get this year
					if MONTH(GETDATE()) < 2
					begin
						SET @Year = YEAR(GETDATE())
					end

					-- Get year of next year
					SET @Date = DATEADD(YEAR, @Year - 1900, 0)

					-- Get first saturday of next year
					SET @first_saturday = DATEADD(DAY, (@@DATEFIRST - DATEPART(WEEKDAY, @Date) + 
						(7 - @@DATEFIRST) * 2) % 6, @Date)

					-- Now get 6th
					SET @sixth_saturday = DATEADD(WEEK, +5, @first_saturday)

					-- Add a year
					set @Date = DATEADD(YEAR, +1, @Date)

					-- Get 6th last
					set @sixth_last_saturday = DATEADD(wk, -5, DATEADD(wk, DATEDIFF(wk, 0, @Date), -2))					
					

					DECLARE @MusicClassID TABLE ( id int ) -- Store created MusicClass ID in this temp table

					-- Create a music class with appropriate tutor

					insert into MusicClass ([Tutor ID], [Start Date], [End Date])

						output inserted.[MusicClass ID] INTO @MusicClassID

						select top 1 [Tutor ID], CAST(@sixth_saturday AS DATE), CAST(@sixth_last_saturday AS DATE)

						from [Tutor Type] 
						
						where [Instrument ID] = (select [Instrument ID] from @insertedEnrollment) 

						and (select Grade from @insertedEnrollment) <= [Max Grade]	

					---- Create Enrollment MusicClass for people in Enrollment

					insert into [Enrollment MusicClass]

						select e.[Enrollment ID], (select id from @MusicClassID), CAST(GETDATE() AS DATE) 

						from Enrollment e left join [Enrollment MusicClass] ec on e.[Enrollment ID] = ec.[Enrollment ID]

						where [Instrument ID] = (select [Instrument ID] from @insertedEnrollment)  
	
						and Grade = (select [Grade] from @insertedEnrollment)  
	
						and ec.[Enrollment ID] is null
				END
		END

	-- ELSE: There is a MusicClass with available spots for us to join. Take the first now full MusicClass and add ourself to it

	ELSE
		BEGIN
			insert into [Enrollment MusicClass]
			select (select [Enrollment ID] from @insertedEnrollment), (select top 1 [MusicClass ID] from @notFullMusicClassIDs), CAST(GETDATE() AS DATE)
		END	

	
END


------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE TRIGGER [DELETE Contact]
   ON Contact
   INSTEAD OF DELETE
AS 
BEGIN 
 DELETE FROM [Guardian] WHERE [Contact ID] IN (SELECT [Contact ID] FROM DELETED)
 DELETE FROM [Local Musicians] WHERE [Contact ID] IN (SELECT [Contact ID] FROM DELETED) 
 DELETE FROM [Staff] WHERE [Contact ID] IN (SELECT [Contact ID] FROM DELETED)  
 DELETE FROM Contact WHERE [Contact ID] IN (SELECT [Contact ID] FROM DELETED) 
END


------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


GO

CREATE TRIGGER [DELETE Enrollment]
   ON Enrollment
   INSTEAD OF DELETE
AS 
BEGIN 
 DELETE FROM [Loan] WHERE [Enrollment ID] IN (SELECT [Enrollment ID] FROM DELETED) 
 DELETE FROM [Enrollment Ensemble] WHERE [Enrollment ID] IN (SELECT [Enrollment ID] FROM DELETED) 
 DELETE FROM [Enrollment MusicClass] WHERE [Enrollment ID] IN (SELECT [Enrollment ID] FROM DELETED)
 DELETE FROM Enrollment WHERE [Enrollment ID] IN (SELECT [Enrollment ID] FROM DELETED) 
END


------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE TRIGGER [DELETE Ensemble]
   ON Ensemble
   INSTEAD OF DELETE
AS 
BEGIN 
 DELETE FROM [Ensemble Performance] WHERE [Ensemble ID] IN (SELECT [Ensemble ID] FROM DELETED) 
 DELETE FROM [Enrollment Ensemble] WHERE [Ensemble ID] IN (SELECT [Ensemble ID] FROM DELETED) 
 DELETE FROM Ensemble WHERE [Ensemble ID] IN (SELECT [Ensemble ID] FROM DELETED) 
END


------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE TRIGGER [DELETE Guardian]
   ON Guardian
   INSTEAD OF DELETE
AS 
BEGIN 
 DELETE FROM [Student Guardian] WHERE [Guardian ID] IN (SELECT [Guardian ID] FROM DELETED) 
 DELETE FROM Guardian WHERE [Guardian ID] IN (SELECT [Guardian ID] FROM DELETED) 
END


------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


CREATE TRIGGER [DELETE Instrument]
   ON Instrument
   INSTEAD OF DELETE
AS 
BEGIN 
 DELETE FROM [Tutor Type] WHERE [Instrument ID] IN (SELECT [Instrument ID] FROM DELETED) 
 DELETE FROM [Enrollment] WHERE [Instrument ID] IN (SELECT [Instrument ID] FROM DELETED)
 DELETE FROM [Instrument Inventory] WHERE [Instrument ID] IN (SELECT [Instrument ID] FROM DELETED)
 DELETE FROM Instrument WHERE [Instrument ID] IN (SELECT [Instrument ID] FROM DELETED) 
END

------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


CREATE TRIGGER [DELETE Instrument Inventory]
   ON [Instrument Inventory]
   INSTEAD OF DELETE
AS 
BEGIN 
 DELETE FROM [Repairs] WHERE [Instrument Inventory ID] IN (SELECT [Instrument Inventory ID] FROM DELETED) 
 DELETE FROM [Loan] WHERE [Instrument Inventory ID] IN (SELECT [Instrument Inventory ID] FROM DELETED) 
 DELETE FROM [Instrument Inventory] WHERE [Instrument Inventory ID] IN (SELECT [Instrument Inventory ID] FROM DELETED) 
END

------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE TRIGGER [DELETE Lesson Time]
   ON [Lesson Time]
   INSTEAD OF DELETE
AS 
BEGIN  
 DELETE FROM [MusicClass] WHERE [Lesson Time ID] IN (SELECT [Lesson Time ID] FROM DELETED)  
 DELETE FROM [Lesson Time] WHERE [Lesson Time ID] IN (SELECT [Lesson Time ID] FROM DELETED) 
END


------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE TRIGGER [DELETE Local Musicians]
   ON [Local Musicians]
   INSTEAD OF DELETE
AS 
BEGIN 
 DELETE FROM [Local Musicians Ensemble]  WHERE [Local Musicians ID] IN (SELECT [Local Musicians ID] FROM DELETED) 
 DELETE FROM [Local Musicians]  WHERE [Local Musicians ID] IN (SELECT [Local Musicians ID] FROM DELETED) 
END


------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE TRIGGER [DELETE MusicClass]
   ON MusicClass
   INSTEAD OF DELETE
AS 
BEGIN  
 DELETE FROM [Enrollment MusicClass] WHERE [MusicClass ID] IN (SELECT [MusicClass ID] FROM DELETED)  
 DELETE FROM MusicClass WHERE [MusicClass ID] IN (SELECT [MusicClass ID] FROM DELETED) 
END


------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


CREATE TRIGGER [DELETE Performance]
   ON Performance
   INSTEAD OF DELETE
AS 
BEGIN 
 DELETE FROM [Ensemble Performance] WHERE [Performance ID] IN (SELECT [Performance ID] FROM DELETED) 
 DELETE FROM Performance WHERE [Performance ID] IN (SELECT [Performance ID] FROM DELETED) 
END


------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE TRIGGER [DELETE Person]
   ON Person
   INSTEAD OF DELETE
AS 
BEGIN 
 DELETE FROM [Guardian] WHERE [Person ID] IN (SELECT [Person ID] FROM DELETED) 
 DELETE FROM Staff WHERE [Person ID] IN (SELECT [Person ID] FROM DELETED) 
 DELETE FROM Student WHERE [Person ID] IN (SELECT [Person ID] FROM DELETED) 
 DELETE FROM Person WHERE [Person ID] IN (SELECT [Person ID] FROM DELETED) 
END


------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE TRIGGER [DELETE SheetMusic]
   ON SheetMusic
   INSTEAD OF DELETE
AS 
BEGIN 
 DELETE FROM [SheetMusic Tutor] WHERE [Sheet Music ID] IN (SELECT [Sheet Music ID] FROM DELETED) 
 DELETE FROM [SheetMusic] WHERE [Sheet Music ID] IN (SELECT [Sheet Music ID] FROM DELETED) 
 
END


------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE TRIGGER [DELETE Staff]
   ON Staff
   INSTEAD OF DELETE
AS 
BEGIN 
 DELETE FROM [Technician] WHERE [Staff ID] IN (SELECT [Staff ID]  FROM DELETED) 
 DELETE FROM [Tutor] WHERE [Staff ID] IN (SELECT [Staff ID]  FROM DELETED) 
 DELETE FROM [Payroll] WHERE [Staff ID] IN (SELECT [Staff ID]  FROM DELETED)
 DELETE FROM Staff WHERE [Staff ID] IN (SELECT [Staff ID] FROM DELETED) 
END


------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE TRIGGER [DELETE Student]
   ON Student
   INSTEAD OF DELETE
AS 
BEGIN 
 DELETE FROM [Student Guardian] WHERE [Student ID] IN (SELECT [Student ID] FROM DELETED) 
 DELETE FROM Enrollment WHERE [Student ID] IN (SELECT [Student ID] FROM DELETED) 
 DELETE FROM Student WHERE [Student ID] IN (SELECT [Student ID] FROM DELETED) 
END


------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


CREATE TRIGGER [DELETE Technician]
   ON Technician
   INSTEAD OF DELETE
AS 
BEGIN 
 DELETE FROM [Repairs] WHERE [Technician ID] IN (SELECT [Technician ID]  FROM DELETED) 
 DELETE FROM [Technician] WHERE [Technician ID] IN (SELECT [Technician ID]  FROM DELETED) 
END


------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


CREATE TRIGGER [DELETE Tutor]
   ON  Tutor
   INSTEAD OF DELETE
AS 
BEGIN 
 DELETE FROM [Tutor Type] WHERE [Tutor ID] IN (SELECT [Tutor ID]  FROM DELETED) 
 DELETE FROM [SheetMusic Tutor] WHERE [Tutor ID] IN (SELECT [Tutor ID]  FROM DELETED) 
 DELETE FROM [Instrument] WHERE [Head Tutor] IN (SELECT [Tutor ID]  FROM DELETED) 
 DELETE FROM [Tutor] WHERE [Tutor ID] IN (SELECT [Tutor ID]  FROM DELETED) 
END


------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


CREATE TRIGGER [DELETE Venue]
   ON  Venue
   INSTEAD OF DELETE
AS 
BEGIN 
 DELETE FROM [Performance] WHERE [Venue ID] IN (SELECT [Venue ID]  FROM DELETED) 
  DELETE FROM [Venue] WHERE [Venue ID] IN (SELECT [Venue ID]  FROM DELETED) 
 
END

