using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SMMC.Models
{
    public partial class IN705_2018S2_db3shared01Context : DbContext
    {
        public IN705_2018S2_db3shared01Context()
        {
        }

        public IN705_2018S2_db3shared01Context(DbContextOptions<IN705_2018S2_db3shared01Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Contact> Contact { get; set; }
        public virtual DbSet<Enrollment> Enrollment { get; set; }
        public virtual DbSet<EnrollmentEnsemble> EnrollmentEnsemble { get; set; }
        public virtual DbSet<EnrollmentMusicClass> EnrollmentMusicClass { get; set; }
        public virtual DbSet<Ensemble> Ensemble { get; set; }
        public virtual DbSet<EnsemblePerformance> EnsemblePerformance { get; set; }
        public virtual DbSet<Guardian> Guardian { get; set; }
        public virtual DbSet<Instrument> Instrument { get; set; }
        public virtual DbSet<InstrumentInventory> InstrumentInventory { get; set; }
        public virtual DbSet<LessonTime> LessonTime { get; set; }
        public virtual DbSet<Loan> Loan { get; set; }
        public virtual DbSet<LocalMusicians> LocalMusicians { get; set; }
        public virtual DbSet<LocalMusiciansEnsemble> LocalMusiciansEnsemble { get; set; }
        public virtual DbSet<MusicClass> MusicClass { get; set; }
        public virtual DbSet<Payroll> Payroll { get; set; }
        public virtual DbSet<Performance> Performance { get; set; }
        public virtual DbSet<PerformancePiece> PerformancePiece { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<Piece> Piece { get; set; }
        public virtual DbSet<Repairs> Repairs { get; set; }
        public virtual DbSet<SheetMusic> SheetMusic { get; set; }
        public virtual DbSet<SheetMusicTutor> SheetMusicTutor { get; set; }
        public virtual DbSet<Staff> Staff { get; set; }
        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<StudentGuardian> StudentGuardian { get; set; }
        public virtual DbSet<Technician> Technician { get; set; }
        public virtual DbSet<Tutor> Tutor { get; set; }
        public virtual DbSet<TutorType> TutorType { get; set; }
        public virtual DbSet<Venue> Venue { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=fthictsql04.ict.op.ac.nz;Database=IN705_2018S2_db3shared01;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>(entity =>
            {
                entity.Property(e => e.ContactId).HasColumnName("Contact ID");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(320)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.HasIndex(e => new { e.StudentId, e.InstrumentId, e.Grade })
                    .HasName("UC_Enrollment")
                    .IsUnique();

                entity.Property(e => e.EnrollmentId).HasColumnName("Enrollment ID");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.InstrumentId).HasColumnName("Instrument ID");

                entity.Property(e => e.StudentId).HasColumnName("Student ID");

                entity.HasOne(d => d.Instrument)
                    .WithMany(p => p.Enrollment)
                    .HasForeignKey(d => d.InstrumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Enrollment_Instrument");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Enrollment)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Enrollment_Student");
            });

            modelBuilder.Entity<EnrollmentEnsemble>(entity =>
            {
                entity.HasKey(e => new { e.EnrollmentId, e.EnsembleId });

                entity.ToTable("Enrollment Ensemble");

                entity.Property(e => e.EnrollmentId).HasColumnName("Enrollment ID");

                entity.Property(e => e.EnsembleId).HasColumnName("Ensemble ID");

                entity.HasOne(d => d.Enrollment)
                    .WithMany(p => p.EnrollmentEnsemble)
                    .HasForeignKey(d => d.EnrollmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EnrollmentEnsemble_Enrollment");

                entity.HasOne(d => d.Ensemble)
                    .WithMany(p => p.EnrollmentEnsemble)
                    .HasForeignKey(d => d.EnsembleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EnrollmentEnsemble_Ensemble");
            });

            modelBuilder.Entity<EnrollmentMusicClass>(entity =>
            {
                entity.HasKey(e => new { e.EnrollmentId, e.MusicClassId });

                entity.ToTable("Enrollment MusicClass");

                entity.Property(e => e.EnrollmentId).HasColumnName("Enrollment ID");

                entity.Property(e => e.MusicClassId).HasColumnName("MusicClass ID");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.HasOne(d => d.Enrollment)
                    .WithMany(p => p.EnrollmentMusicClass)
                    .HasForeignKey(d => d.EnrollmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Enrollment_MusicClass_Enrollment");

                entity.HasOne(d => d.MusicClass)
                    .WithMany(p => p.EnrollmentMusicClass)
                    .HasForeignKey(d => d.MusicClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Enrollment_MusicClass_MusicClass");
            });

            modelBuilder.Entity<Ensemble>(entity =>
            {
                entity.Property(e => e.EnsembleId).HasColumnName("Ensemble ID");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EnsemblePerformance>(entity =>
            {
                entity.HasKey(e => new { e.EnsembleId, e.PerformanceId });

                entity.ToTable("Ensemble Performance");

                entity.Property(e => e.EnsembleId).HasColumnName("Ensemble ID");

                entity.Property(e => e.PerformanceId).HasColumnName("Performance ID");

                entity.HasOne(d => d.Ensemble)
                    .WithMany(p => p.EnsemblePerformance)
                    .HasForeignKey(d => d.EnsembleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EnsemblePerformance_Ensemble");

                entity.HasOne(d => d.Performance)
                    .WithMany(p => p.EnsemblePerformance)
                    .HasForeignKey(d => d.PerformanceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EnsemblePerformance_MusicClass");
            });

            modelBuilder.Entity<Guardian>(entity =>
            {
                entity.HasIndex(e => e.ContactId)
                    .HasName("UC_Contact_Guardian")
                    .IsUnique();

                entity.HasIndex(e => e.PersonId)
                    .HasName("UC_Person_Guardian")
                    .IsUnique();

                entity.Property(e => e.GuardianId)
                    .HasColumnName("Guardian ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ContactId).HasColumnName("Contact ID");

                entity.Property(e => e.PersonId).HasColumnName("Person ID");

                entity.HasOne(d => d.Contact)
                    .WithOne(p => p.Guardian)
                    .HasForeignKey<Guardian>(d => d.ContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Guardian_Contact");

                entity.HasOne(d => d.GuardianNavigation)
                    .WithOne(p => p.Guardian)
                    .HasForeignKey<Guardian>(d => d.GuardianId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Guardian_Person");
            });

            modelBuilder.Entity<Instrument>(entity =>
            {
                entity.Property(e => e.InstrumentId).HasColumnName("Instrument ID");

                entity.Property(e => e.HeadTutor).HasColumnName("Head Tutor");

                entity.Property(e => e.HireFee).HasColumnName("Hire Fee");

                entity.Property(e => e.Instrument1)
                    .IsRequired()
                    .HasColumnName("Instrument")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.OpenFee).HasColumnName("Open Fee");

                entity.Property(e => e.StudentFee).HasColumnName("Student Fee");

                entity.HasOne(d => d.HeadTutorNavigation)
                    .WithMany(p => p.Instrument)
                    .HasForeignKey(d => d.HeadTutor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Instrument_Tutor");
            });

            modelBuilder.Entity<InstrumentInventory>(entity =>
            {
                entity.ToTable("Instrument Inventory");

                entity.Property(e => e.InstrumentInventoryId).HasColumnName("Instrument Inventory ID");

                entity.Property(e => e.InstrumentId).HasColumnName("Instrument ID");

                entity.Property(e => e.Manufacturer)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PurchaseDate)
                    .HasColumnName("Purchase Date")
                    .HasColumnType("date");

                entity.HasOne(d => d.Instrument)
                    .WithMany(p => p.InstrumentInventory)
                    .HasForeignKey(d => d.InstrumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Instrument_Inventory_Instrument");
            });

            modelBuilder.Entity<LessonTime>(entity =>
            {
                entity.ToTable("Lesson Time");

                entity.Property(e => e.LessonTimeId).HasColumnName("Lesson Time ID");
            });

            modelBuilder.Entity<Loan>(entity =>
            {
                entity.HasKey(e => new { e.InstrumentInventoryId, e.EnrollmentId, e.DateLoaned });

                entity.Property(e => e.InstrumentInventoryId).HasColumnName("Instrument Inventory ID");

                entity.Property(e => e.EnrollmentId).HasColumnName("Enrollment ID");

                entity.Property(e => e.DateLoaned)
                    .HasColumnName("Date Loaned")
                    .HasColumnType("datetime");

                entity.Property(e => e.DateReturned)
                    .HasColumnName("Date Returned")
                    .HasColumnType("date");

                entity.HasOne(d => d.Enrollment)
                    .WithMany(p => p.Loan)
                    .HasForeignKey(d => d.EnrollmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Loan_Enrollment");

                entity.HasOne(d => d.InstrumentInventory)
                    .WithMany(p => p.Loan)
                    .HasForeignKey(d => d.InstrumentInventoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Loan_InstrumentInventory");
            });

            modelBuilder.Entity<LocalMusicians>(entity =>
            {
                entity.ToTable("Local Musicians");

                entity.HasIndex(e => e.ContactId)
                    .HasName("UC_Contact_LocalMusicians")
                    .IsUnique();

                entity.Property(e => e.LocalMusiciansId).HasColumnName("Local Musicians ID");

                entity.Property(e => e.ContactId).HasColumnName("Contact ID");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("First Name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("Last Name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Contact)
                    .WithOne(p => p.LocalMusicians)
                    .HasForeignKey<LocalMusicians>(d => d.ContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LocalMusicians_Contact");
            });

            modelBuilder.Entity<LocalMusiciansEnsemble>(entity =>
            {
                entity.ToTable("Local Musicians Ensemble");

                entity.Property(e => e.LocalMusiciansEnsembleId).HasColumnName("Local Musicians Ensemble ID");

                entity.Property(e => e.EnsembleId).HasColumnName("Ensemble ID");

                entity.Property(e => e.LocalMusiciansId).HasColumnName("Local Musicians ID");

                entity.HasOne(d => d.Ensemble)
                    .WithMany(p => p.LocalMusiciansEnsemble)
                    .HasForeignKey(d => d.EnsembleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LocalMusiciansEnsemble_Ensemble");

                entity.HasOne(d => d.LocalMusicians)
                    .WithMany(p => p.LocalMusiciansEnsemble)
                    .HasForeignKey(d => d.LocalMusiciansId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LocalMusiciansEnsemble_LocalMusicians");
            });

            modelBuilder.Entity<MusicClass>(entity =>
            {
                entity.Property(e => e.MusicClassId).HasColumnName("MusicClass ID");

                entity.Property(e => e.EndDate)
                    .HasColumnName("End Date")
                    .HasColumnType("date");

                entity.Property(e => e.LessonTimeId).HasColumnName("Lesson Time ID");

                entity.Property(e => e.StartDate)
                    .HasColumnName("Start Date")
                    .HasColumnType("date");

                entity.Property(e => e.TutorId).HasColumnName("Tutor ID");

                entity.HasOne(d => d.LessonTime)
                    .WithMany(p => p.MusicClass)
                    .HasForeignKey(d => d.LessonTimeId)
                    .HasConstraintName("FK_MusicClass_Lesson_Time");

                entity.HasOne(d => d.Tutor)
                    .WithMany(p => p.MusicClass)
                    .HasForeignKey(d => d.TutorId)
                    .HasConstraintName("FK_MusicClass_Tutor");
            });

            modelBuilder.Entity<Payroll>(entity =>
            {
                entity.Property(e => e.PayrollId).HasColumnName("Payroll ID");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Notes).IsUnicode(false);

                entity.Property(e => e.StaffId).HasColumnName("Staff ID");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.Payroll)
                    .HasForeignKey(d => d.StaffId)
                    .HasConstraintName("FK_Payroll_Staff");
            });

            modelBuilder.Entity<Performance>(entity =>
            {
                entity.Property(e => e.PerformanceId).HasColumnName("Performance ID");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.VenueId).HasColumnName("Venue ID");

                entity.HasOne(d => d.Venue)
                    .WithMany(p => p.Performance)
                    .HasForeignKey(d => d.VenueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Performance_Venue");
            });

            modelBuilder.Entity<PerformancePiece>(entity =>
            {
                entity.ToTable("Performance Piece");

                entity.Property(e => e.PerformancePieceId).HasColumnName("Performance Piece ID");

                entity.Property(e => e.PerformanceId).HasColumnName("Performance ID");

                entity.Property(e => e.PieceId).HasColumnName("Piece ID");

                entity.HasOne(d => d.Performance)
                    .WithMany(p => p.PerformancePiece)
                    .HasForeignKey(d => d.PerformanceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Performance_Piece_Performance");

                entity.HasOne(d => d.Piece)
                    .WithMany(p => p.PerformancePiece)
                    .HasForeignKey(d => d.PieceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Performance_Piece_Piece");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.Property(e => e.PersonId).HasColumnName("Person ID");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(750)
                    .IsUnicode(false);

                entity.Property(e => e.Dob)
                    .HasColumnName("DOB")
                    .HasColumnType("date");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("First Name")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("Last Name")
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Piece>(entity =>
            {
                entity.Property(e => e.PieceId).HasColumnName("Piece ID");

                entity.Property(e => e.Composer)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.LastPerformedDate)
                    .HasColumnName("Last Performed Date")
                    .HasColumnType("date");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Repairs>(entity =>
            {
                entity.HasKey(e => new { e.InstrumentInventoryId, e.TechnicianId, e.RepairStart });

                entity.Property(e => e.InstrumentInventoryId).HasColumnName("Instrument Inventory ID");

                entity.Property(e => e.TechnicianId).HasColumnName("Technician ID");

                entity.Property(e => e.RepairStart)
                    .HasColumnName("Repair Start")
                    .HasColumnType("datetime");

                entity.Property(e => e.Notes)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.RepairEnd)
                    .HasColumnName("Repair End")
                    .HasColumnType("date");

                entity.HasOne(d => d.InstrumentInventory)
                    .WithMany(p => p.Repairs)
                    .HasForeignKey(d => d.InstrumentInventoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Repairs_InstrumentInventory");

                entity.HasOne(d => d.Technician)
                    .WithMany(p => p.Repairs)
                    .HasForeignKey(d => d.TechnicianId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Repairs_Technician");
            });

            modelBuilder.Entity<SheetMusic>(entity =>
            {
                entity.Property(e => e.SheetMusicId).HasColumnName("Sheet Music ID");

                entity.Property(e => e.Composer)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SheetMusicTutor>(entity =>
            {
                entity.ToTable("SheetMusic Tutor");

                entity.Property(e => e.SheetMusicTutorId).HasColumnName("Sheet Music Tutor ID");

                entity.Property(e => e.AmountLoaned).HasColumnName("Amount Loaned");

                entity.Property(e => e.DateLoaned)
                    .HasColumnName("Date Loaned")
                    .HasColumnType("date");

                entity.Property(e => e.DateReturned)
                    .HasColumnName("Date Returned")
                    .HasColumnType("date");

                entity.Property(e => e.SheetMusicId).HasColumnName("Sheet Music ID");

                entity.Property(e => e.TutorId).HasColumnName("Tutor ID");

                entity.HasOne(d => d.SheetMusic)
                    .WithMany(p => p.SheetMusicTutor)
                    .HasForeignKey(d => d.SheetMusicId)
                    .HasConstraintName("FK_SheetMusic_Tutor_SheetMusic");

                entity.HasOne(d => d.Tutor)
                    .WithMany(p => p.SheetMusicTutor)
                    .HasForeignKey(d => d.TutorId)
                    .HasConstraintName("FK_SheetMusic_Tutor_Tutor");
            });

            modelBuilder.Entity<Staff>(entity =>
            {
                entity.HasIndex(e => e.ContactId)
                    .HasName("UC_Contact_Staff")
                    .IsUnique();

                entity.HasIndex(e => e.PersonId)
                    .HasName("UC_Person_Staff")
                    .IsUnique();

                entity.Property(e => e.StaffId).HasColumnName("Staff ID");

                entity.Property(e => e.ContactId).HasColumnName("Contact ID");

                entity.Property(e => e.LeftDate)
                    .HasColumnName("Left Date")
                    .HasColumnType("date");

                entity.Property(e => e.PersonId).HasColumnName("Person ID");

                entity.Property(e => e.StartDate)
                    .HasColumnName("Start Date")
                    .HasColumnType("date");

                entity.HasOne(d => d.Contact)
                    .WithOne(p => p.Staff)
                    .HasForeignKey<Staff>(d => d.ContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Staff_Contact");

                entity.HasOne(d => d.Person)
                    .WithOne(p => p.Staff)
                    .HasForeignKey<Staff>(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Staff_Person");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasIndex(e => e.PersonId)
                    .HasName("UC_Student_Person")
                    .IsUnique();

                entity.Property(e => e.StudentId).HasColumnName("Student ID");

                entity.Property(e => e.ContactId).HasColumnName("Contact ID");

                entity.Property(e => e.PersonId).HasColumnName("Person ID");

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.Student)
                    .HasForeignKey(d => d.ContactId)
                    .HasConstraintName("FK_Student_Contact");

                entity.HasOne(d => d.Person)
                    .WithOne(p => p.Student)
                    .HasForeignKey<Student>(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Student_Person");
            });

            modelBuilder.Entity<StudentGuardian>(entity =>
            {
                entity.HasKey(e => new { e.StudentId, e.GuardianId });

                entity.ToTable("Student Guardian");

                entity.Property(e => e.StudentId).HasColumnName("Student ID");

                entity.Property(e => e.GuardianId).HasColumnName("Guardian ID");

                entity.HasOne(d => d.Guardian)
                    .WithMany(p => p.StudentGuardian)
                    .HasForeignKey(d => d.GuardianId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentGuardian_Guardian");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.StudentGuardian)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentGuardian_Student");
            });

            modelBuilder.Entity<Technician>(entity =>
            {
                entity.Property(e => e.TechnicianId).HasColumnName("Technician ID");

                entity.Property(e => e.StaffId).HasColumnName("Staff ID");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.Technician)
                    .HasForeignKey(d => d.StaffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Technician_Staff");
            });

            modelBuilder.Entity<Tutor>(entity =>
            {
                entity.Property(e => e.TutorId).HasColumnName("Tutor ID");

                entity.Property(e => e.Atcl).HasColumnName("ATCL");

                entity.Property(e => e.StaffId).HasColumnName("Staff ID");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.Tutor)
                    .HasForeignKey(d => d.StaffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tutor_Staff");
            });

            modelBuilder.Entity<TutorType>(entity =>
            {
                entity.ToTable("Tutor Type");

                entity.HasIndex(e => new { e.TutorId, e.InstrumentId, e.MaxGrade })
                    .HasName("UNIQUE_Tutor_Type")
                    .IsUnique();

                entity.Property(e => e.TutorTypeId).HasColumnName("Tutor Type ID");

                entity.Property(e => e.InstrumentId).HasColumnName("Instrument ID");

                entity.Property(e => e.MaxGrade).HasColumnName("Max Grade");

                entity.Property(e => e.TutorId).HasColumnName("Tutor ID");

                entity.HasOne(d => d.Instrument)
                    .WithMany(p => p.TutorType)
                    .HasForeignKey(d => d.InstrumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TutorType_Instrument");

                entity.HasOne(d => d.Tutor)
                    .WithMany(p => p.TutorType)
                    .HasForeignKey(d => d.TutorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TutorType_Tutor");
            });

            modelBuilder.Entity<Venue>(entity =>
            {
                entity.Property(e => e.VenueId).HasColumnName("Venue ID");

                entity.Property(e => e.VenueName)
                    .IsRequired()
                    .HasColumnName("Venue Name")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });
        }
    }
}
