using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace STTB.WebApiStandard.Entities;

public partial class SttbDbContext : DbContext
{
    public SttbDbContext()
    {
    }

    public SttbDbContext(DbContextOptions<SttbDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AcademicCourse> AcademicCourses { get; set; }

    public virtual DbSet<AcademicCourseCategory> AcademicCourseCategories { get; set; }

    public virtual DbSet<AcademicProgram> AcademicPrograms { get; set; }

    public virtual DbSet<AcademicProgramCost> AcademicProgramCosts { get; set; }

    public virtual DbSet<AcademicProgramCostCategory> AcademicProgramCostCategories { get; set; }

    public virtual DbSet<AcademicProgramCostCategoryMap> AcademicProgramCostCategoryMaps { get; set; }

    public virtual DbSet<AcademicProgramGraduateCriterion> AcademicProgramGraduateCriteria { get; set; }

    public virtual DbSet<AcademicProgramNote> AcademicProgramNotes { get; set; }

    public virtual DbSet<AcademicProgramRequirement> AcademicProgramRequirements { get; set; }

    public virtual DbSet<AcademicProgramSystem> AcademicProgramSystems { get; set; }

    public virtual DbSet<AdmissionDeadline> AdmissionDeadlines { get; set; }

    public virtual DbSet<Asset> Assets { get; set; }

    public virtual DbSet<DonorMember> DonorMembers { get; set; }

    public virtual DbSet<DonorScholarshipDetail> DonorScholarshipDetails { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventCategory> EventCategories { get; set; }

    public virtual DbSet<EventCategoryMap> EventCategoryMaps { get; set; }

    public virtual DbSet<EventOrganizer> EventOrganizers { get; set; }

    public virtual DbSet<FoundationAdministrator> FoundationAdministrators { get; set; }

    public virtual DbSet<Lecturer> Lecturers { get; set; }

    public virtual DbSet<LecturerDegree> LecturerDegrees { get; set; }

    public virtual DbSet<LecturerDegreeMap> LecturerDegreeMaps { get; set; }

    public virtual DbSet<LecturerRole> LecturerRoles { get; set; }

    public virtual DbSet<LecturerRoleMap> LecturerRoleMaps { get; set; }

    public virtual DbSet<LibraryMember> LibraryMembers { get; set; }

    public virtual DbSet<MediaCollection> MediaCollections { get; set; }

    public virtual DbSet<MediaItem> MediaItems { get; set; }

    public virtual DbSet<MediaItemTopic> MediaItemTopics { get; set; }

    public virtual DbSet<MediaTopicCategory> MediaTopicCategories { get; set; }

    public virtual DbSet<NewsCategory> NewsCategories { get; set; }

    public virtual DbSet<NewsPost> NewsPosts { get; set; }

    public virtual DbSet<NewsPostCategory> NewsPostCategories { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RolePermission> RolePermissions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserPermission> UserPermissions { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=sttb_db;Username=postgres;Password=postgres312005");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AcademicCourse>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("academic_courses_pkey");

            entity.ToTable("academic_courses");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Credits).HasColumnName("credits");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Category).WithMany(p => p.AcademicCourses)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("academic_courses_category_id_fkey");
        });

        modelBuilder.Entity<AcademicCourseCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("academic_course_categories_pkey");

            entity.ToTable("academic_course_categories");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
            entity.Property(e => e.ProgramId).HasColumnName("program_id");
            entity.Property(e => e.TotalCredits).HasColumnName("total_credits");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Program).WithMany(p => p.AcademicCourseCategories)
                .HasForeignKey(d => d.ProgramId)
                .HasConstraintName("academic_course_categories_program_id_fkey");
        });

        modelBuilder.Entity<AcademicProgram>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("academic_programs_pkey");

            entity.ToTable("academic_programs");

            entity.HasIndex(e => e.Slug, "academic_programs_slug_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DegreeAbbr)
                .HasMaxLength(30)
                .HasColumnName("degree_abbr");
            entity.Property(e => e.GraduateProfileDescription).HasColumnName("graduate_profile_description");
            entity.Property(e => e.GraduateProfileMotto)
                .HasMaxLength(150)
                .HasColumnName("graduate_profile_motto");
            entity.Property(e => e.InformedDescription).HasColumnName("informed_description");
            entity.Property(e => e.IsPublished)
                .HasDefaultValue(true)
                .HasColumnName("is_published");
            entity.Property(e => e.LearningSystemText).HasColumnName("learning_system_text");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Slug)
                .HasMaxLength(255)
                .HasColumnName("slug");
            entity.Property(e => e.StudyDuration).HasColumnName("study_duration");
            entity.Property(e => e.TotalCredits).HasColumnName("total_credits");
            entity.Property(e => e.TransformativeDescription).HasColumnName("transformative_description");
            entity.Property(e => e.TransformedDescription).HasColumnName("transformed_description");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<AcademicProgramCost>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("academic_program_costs_pkey");

            entity.ToTable("academic_program_costs");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AcademicProgramId).HasColumnName("academic_program_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasPrecision(14, 2)
                .HasColumnName("price");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.AcademicProgram).WithMany(p => p.AcademicProgramCosts)
                .HasForeignKey(d => d.AcademicProgramId)
                .HasConstraintName("academic_program_costs_academic_program_id_fkey");
        });

        modelBuilder.Entity<AcademicProgramCostCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("academic_program_cost_categories_pkey");

            entity.ToTable("academic_program_cost_categories");

            entity.HasIndex(e => e.CategoryName, "academic_program_cost_categories_category_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(150)
                .HasColumnName("category_name");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<AcademicProgramCostCategoryMap>(entity =>
        {
            entity.HasKey(e => new { e.AcademicProgramCostId, e.AcademicProgramCostCategoryId }).HasName("academic_program_cost_category_map_pkey");

            entity.ToTable("academic_program_cost_category_map");

            entity.Property(e => e.AcademicProgramCostId).HasColumnName("academic_program_cost_id");
            entity.Property(e => e.AcademicProgramCostCategoryId).HasColumnName("academic_program_cost_category_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.AcademicProgramCostCategory).WithMany(p => p.AcademicProgramCostCategoryMaps)
                .HasForeignKey(d => d.AcademicProgramCostCategoryId)
                .HasConstraintName("academic_program_cost_categor_academic_program_cost_catego_fkey");

            entity.HasOne(d => d.AcademicProgramCost).WithMany(p => p.AcademicProgramCostCategoryMaps)
                .HasForeignKey(d => d.AcademicProgramCostId)
                .HasConstraintName("academic_program_cost_category_ma_academic_program_cost_id_fkey");
        });

        modelBuilder.Entity<AcademicProgramGraduateCriterion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("academic_program_graduate_criteria_pkey");

            entity.ToTable("academic_program_graduate_criteria");

            entity.HasIndex(e => new { e.ProgramId, e.CriterionName }, "academic_program_graduate_criteri_program_id_criterion_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.CriterionName)
                .HasMaxLength(30)
                .HasColumnName("criterion_name");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.ProgramId).HasColumnName("program_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Program).WithMany(p => p.AcademicProgramGraduateCriteria)
                .HasForeignKey(d => d.ProgramId)
                .HasConstraintName("academic_program_graduate_criteria_program_id_fkey");
        });

        modelBuilder.Entity<AcademicProgramNote>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("academic_program_notes_pkey");

            entity.ToTable("academic_program_notes");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.NoteText).HasColumnName("note_text");
            entity.Property(e => e.ProgramId).HasColumnName("program_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Program).WithMany(p => p.AcademicProgramNotes)
                .HasForeignKey(d => d.ProgramId)
                .HasConstraintName("academic_program_notes_program_id_fkey");
        });

        modelBuilder.Entity<AcademicProgramRequirement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("academic_program_requirements_pkey");

            entity.ToTable("academic_program_requirements");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.ProgramId).HasColumnName("program_id");
            entity.Property(e => e.RequirementText).HasColumnName("requirement_text");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Program).WithMany(p => p.AcademicProgramRequirements)
                .HasForeignKey(d => d.ProgramId)
                .HasConstraintName("academic_program_requirements_program_id_fkey");
        });

        modelBuilder.Entity<AcademicProgramSystem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("academic_program_systems_pkey");

            entity.ToTable("academic_program_systems");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.ProgramId).HasColumnName("program_id");
            entity.Property(e => e.SystemText).HasColumnName("system_text");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Program).WithMany(p => p.AcademicProgramSystems)
                .HasForeignKey(d => d.ProgramId)
                .HasConstraintName("academic_program_systems_program_id_fkey");
        });

        modelBuilder.Entity<AdmissionDeadline>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("admission_deadlines_pkey");

            entity.ToTable("admission_deadlines");

            entity.HasIndex(e => e.AcademicYear, "admission_deadlines_academic_year_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AcademicYear)
                .HasMaxLength(9)
                .HasColumnName("academic_year");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.FirstBatchClosingAt).HasColumnName("first_batch_closing_at");
            entity.Property(e => e.SecondBatchClosingAt).HasColumnName("second_batch_closing_at");
            entity.Property(e => e.ThirdBatchClosingAt).HasColumnName("third_batch_closing_at");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Asset>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("assets_pkey");

            entity.ToTable("assets");

            entity.HasIndex(e => e.FilePath, "assets_file_path_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AltText)
                .HasMaxLength(255)
                .HasColumnName("alt_text");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.FileName)
                .HasMaxLength(255)
                .HasColumnName("file_name");
            entity.Property(e => e.FilePath).HasColumnName("file_path");
            entity.Property(e => e.Height).HasColumnName("height");
            entity.Property(e => e.MimeType)
                .HasMaxLength(100)
                .HasColumnName("mime_type");
            entity.Property(e => e.ModelId).HasColumnName("model_id");
            entity.Property(e => e.ModelType)
                .HasMaxLength(255)
                .HasColumnName("model_type");
            entity.Property(e => e.SizeBytes).HasColumnName("size_bytes");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
            entity.Property(e => e.Width).HasColumnName("width");
        });

        modelBuilder.Entity<DonorMember>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("donor_members_pkey");

            entity.ToTable("donor_members");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.Contact)
                .HasMaxLength(50)
                .HasColumnName("contact");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DonationAmount)
                .HasPrecision(15, 2)
                .HasColumnName("donation_amount");
            entity.Property(e => e.DonationType)
                .HasMaxLength(50)
                .HasColumnName("donation_type");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.ProofOfDonationPath).HasColumnName("proof_of_donation_path");
            entity.Property(e => e.ProofOfSupport).HasColumnName("proof_of_support");
            entity.Property(e => e.Salutation)
                .HasMaxLength(50)
                .HasColumnName("salutation");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<DonorScholarshipDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("donor_scholarship_details_pkey");

            entity.ToTable("donor_scholarship_details");

            entity.HasIndex(e => e.DonorMemberId, "donor_scholarship_details_donor_member_id_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AcademicProgramId).HasColumnName("academic_program_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DonorMemberId).HasColumnName("donor_member_id");
            entity.Property(e => e.StudentName)
                .HasMaxLength(255)
                .HasColumnName("student_name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.AcademicProgram).WithMany(p => p.DonorScholarshipDetails)
                .HasForeignKey(d => d.AcademicProgramId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("donor_scholarship_details_academic_program_id_fkey");

            entity.HasOne(d => d.DonorMember).WithOne(p => p.DonorScholarshipDetail)
                .HasForeignKey<DonorScholarshipDetail>(d => d.DonorMemberId)
                .HasConstraintName("donor_scholarship_details_donor_member_id_fkey");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("events_pkey");

            entity.ToTable("events");

            entity.HasIndex(e => e.Slug, "events_slug_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.EndAt).HasColumnName("end_at");
            entity.Property(e => e.EventOrganizerId).HasColumnName("event_organizer_id");
            entity.Property(e => e.IsPublished)
                .HasDefaultValue(true)
                .HasColumnName("is_published");
            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .HasColumnName("location");
            entity.Property(e => e.Slug)
                .HasMaxLength(255)
                .HasColumnName("slug");
            entity.Property(e => e.StartAt).HasColumnName("start_at");
            entity.Property(e => e.Summary).HasColumnName("summary");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.EventOrganizer).WithMany(p => p.Events)
                .HasForeignKey(d => d.EventOrganizerId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("events_event_organizer_id_fkey");
        });

        modelBuilder.Entity<EventCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("event_categories_pkey");

            entity.ToTable("event_categories");

            entity.HasIndex(e => e.Name, "event_categories_name_key").IsUnique();

            entity.HasIndex(e => e.Slug, "event_categories_slug_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Slug)
                .HasMaxLength(120)
                .HasColumnName("slug");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<EventCategoryMap>(entity =>
        {
            entity.HasKey(e => new { e.EventId, e.CategoryId }).HasName("event_category_map_pkey");

            entity.ToTable("event_category_map");

            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Category).WithMany(p => p.EventCategoryMaps)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("event_category_map_category_id_fkey");

            entity.HasOne(d => d.Event).WithMany(p => p.EventCategoryMaps)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("event_category_map_event_id_fkey");
        });

        modelBuilder.Entity<EventOrganizer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("event_organizers_pkey");

            entity.ToTable("event_organizers");

            entity.HasIndex(e => e.Name, "event_organizers_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
            entity.Property(e => e.OrganizerType)
                .HasMaxLength(20)
                .HasColumnName("organizer_type");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<FoundationAdministrator>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("foundation_administrators_pkey");

            entity.ToTable("foundation_administrators");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AdminName)
                .HasMaxLength(150)
                .HasColumnName("admin_name");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Division)
                .HasMaxLength(30)
                .HasColumnName("division");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasColumnName("role");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Lecturer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("lecturers_pkey");

            entity.ToTable("lecturers");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.JoinedAt).HasColumnName("joined_at");
            entity.Property(e => e.LecturerName)
                .HasMaxLength(150)
                .HasColumnName("lecturer_name");
            entity.Property(e => e.OrganizationalRole)
                .HasMaxLength(150)
                .HasDefaultValueSql("'Anggota'::character varying")
                .HasColumnName("organizational_role");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<LecturerDegree>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("lecturer_degrees_pkey");

            entity.ToTable("lecturer_degrees");

            entity.HasIndex(e => e.DegreeName, "lecturer_degrees_degree_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DegreeName)
                .HasMaxLength(100)
                .HasColumnName("degree_name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<LecturerDegreeMap>(entity =>
        {
            entity.HasKey(e => new { e.LecturerId, e.LecturerDegreeId }).HasName("lecturer_degree_map_pkey");

            entity.ToTable("lecturer_degree_map");

            entity.Property(e => e.LecturerId).HasColumnName("lecturer_id");
            entity.Property(e => e.LecturerDegreeId).HasColumnName("lecturer_degree_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.LecturerDegree).WithMany(p => p.LecturerDegreeMaps)
                .HasForeignKey(d => d.LecturerDegreeId)
                .HasConstraintName("lecturer_degree_map_lecturer_degree_id_fkey");

            entity.HasOne(d => d.Lecturer).WithMany(p => p.LecturerDegreeMaps)
                .HasForeignKey(d => d.LecturerId)
                .HasConstraintName("lecturer_degree_map_lecturer_id_fkey");
        });

        modelBuilder.Entity<LecturerRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("lecturer_roles_pkey");

            entity.ToTable("lecturer_roles");

            entity.HasIndex(e => e.RoleName, "lecturer_roles_role_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.RoleName)
                .HasMaxLength(100)
                .HasColumnName("role_name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<LecturerRoleMap>(entity =>
        {
            entity.HasKey(e => new { e.LecturerId, e.LecturerRoleId }).HasName("lecturer_role_map_pkey");

            entity.ToTable("lecturer_role_map");

            entity.Property(e => e.LecturerId).HasColumnName("lecturer_id");
            entity.Property(e => e.LecturerRoleId).HasColumnName("lecturer_role_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Lecturer).WithMany(p => p.LecturerRoleMaps)
                .HasForeignKey(d => d.LecturerId)
                .HasConstraintName("lecturer_role_map_lecturer_id_fkey");

            entity.HasOne(d => d.LecturerRole).WithMany(p => p.LecturerRoleMaps)
                .HasForeignKey(d => d.LecturerRoleId)
                .HasConstraintName("lecturer_role_map_lecturer_role_id_fkey");
        });

        modelBuilder.Entity<LibraryMember>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("library_members_pkey");

            entity.ToTable("library_members");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.Contact)
                .HasMaxLength(50)
                .HasColumnName("contact");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasColumnName("full_name");
            entity.Property(e => e.InstitutionName)
                .HasMaxLength(255)
                .HasColumnName("institution_name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<MediaCollection>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("media_collections_pkey");

            entity.ToTable("media_collections");

            entity.HasIndex(e => e.Name, "media_collections_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<MediaItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("media_items_pkey");

            entity.ToTable("media_items");

            entity.HasIndex(e => e.Slug, "media_items_slug_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AuthorName)
                .HasMaxLength(150)
                .HasColumnName("author_name");
            entity.Property(e => e.AuthorPosition)
                .HasMaxLength(150)
                .HasColumnName("author_position");
            entity.Property(e => e.CollectionId).HasColumnName("collection_id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IsPublished)
                .HasDefaultValue(true)
                .HasColumnName("is_published");
            entity.Property(e => e.MediaFormat)
                .HasMaxLength(20)
                .HasColumnName("media_format");
            entity.Property(e => e.PublishedAt).HasColumnName("published_at");
            entity.Property(e => e.Slug)
                .HasMaxLength(255)
                .HasColumnName("slug");
            entity.Property(e => e.Theme)
                .HasMaxLength(255)
                .HasColumnName("theme");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
            entity.Property(e => e.VideoUrl).HasColumnName("video_url");

            entity.HasOne(d => d.Collection).WithMany(p => p.MediaItems)
                .HasForeignKey(d => d.CollectionId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("media_items_collection_id_fkey");
        });

        modelBuilder.Entity<MediaItemTopic>(entity =>
        {
            entity.HasKey(e => new { e.MediaItemId, e.TopicCategoryId }).HasName("media_item_topics_pkey");

            entity.ToTable("media_item_topics");

            entity.Property(e => e.MediaItemId).HasColumnName("media_item_id");
            entity.Property(e => e.TopicCategoryId).HasColumnName("topic_category_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.MediaItem).WithMany(p => p.MediaItemTopics)
                .HasForeignKey(d => d.MediaItemId)
                .HasConstraintName("media_item_topics_media_item_id_fkey");

            entity.HasOne(d => d.TopicCategory).WithMany(p => p.MediaItemTopics)
                .HasForeignKey(d => d.TopicCategoryId)
                .HasConstraintName("media_item_topics_topic_category_id_fkey");
        });

        modelBuilder.Entity<MediaTopicCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("media_topic_categories_pkey");

            entity.ToTable("media_topic_categories");

            entity.HasIndex(e => e.Name, "media_topic_categories_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<NewsCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("news_categories_pkey");

            entity.ToTable("news_categories");

            entity.HasIndex(e => e.Name, "news_categories_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<NewsPost>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("news_posts_pkey");

            entity.ToTable("news_posts");

            entity.HasIndex(e => e.Slug, "news_posts_slug_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.IsFeatured).HasColumnName("is_featured");
            entity.Property(e => e.IsPublished)
                .HasDefaultValue(true)
                .HasColumnName("is_published");
            entity.Property(e => e.PublishedAt).HasColumnName("published_at");
            entity.Property(e => e.Slug)
                .HasMaxLength(255)
                .HasColumnName("slug");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<NewsPostCategory>(entity =>
        {
            entity.HasKey(e => new { e.NewsPostId, e.CategoryId }).HasName("news_post_categories_pkey");

            entity.ToTable("news_post_categories");

            entity.Property(e => e.NewsPostId).HasColumnName("news_post_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Category).WithMany(p => p.NewsPostCategories)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("news_post_categories_category_id_fkey");

            entity.HasOne(d => d.NewsPost).WithMany(p => p.NewsPostCategories)
                .HasForeignKey(d => d.NewsPostId)
                .HasConstraintName("news_post_categories_news_post_id_fkey");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("permissions_pkey");

            entity.ToTable("permissions");

            entity.HasIndex(e => e.Name, "permissions_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.HasIndex(e => e.Name, "roles_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.HasKey(e => new { e.RoleId, e.PermissionId }).HasName("role_permissions_pkey");

            entity.ToTable("role_permissions");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.PermissionId).HasColumnName("permission_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Permission).WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.PermissionId)
                .HasConstraintName("role_permissions_permission_id_fkey");

            entity.HasOne(d => d.Role).WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("role_permissions_role_id_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(150)
                .HasColumnName("full_name");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.LastLoginAt).HasColumnName("last_login_at");
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<UserPermission>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.PermissionId }).HasName("user_permissions_pkey");

            entity.ToTable("user_permissions");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.PermissionId).HasColumnName("permission_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Permission).WithMany(p => p.UserPermissions)
                .HasForeignKey(d => d.PermissionId)
                .HasConstraintName("user_permissions_permission_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.UserPermissions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("user_permissions_user_id_fkey");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId }).HasName("user_roles_pkey");

            entity.ToTable("user_roles");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("user_roles_role_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("user_roles_user_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
