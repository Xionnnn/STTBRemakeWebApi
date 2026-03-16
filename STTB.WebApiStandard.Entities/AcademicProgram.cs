using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Entities;

public partial class AcademicProgram
{
    public long Id { get; set; }

    public string Slug { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string DegreeAbbr { get; set; } = null!;

    public string GraduateProfileMotto { get; set; } = null!;

    public string? GraduateProfileDescription { get; set; }

    public string InformedDescription { get; set; } = null!;

    public string TransformedDescription { get; set; } = null!;

    public string TransformativeDescription { get; set; } = null!;

    public int TotalCredits { get; set; }

    public int StudyDuration { get; set; }

    public string? LearningSystemText { get; set; }

    public bool IsPublished { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<AcademicCourseCategory> AcademicCourseCategories { get; set; } = new List<AcademicCourseCategory>();

    public virtual ICollection<AcademicProgramCost> AcademicProgramCosts { get; set; } = new List<AcademicProgramCost>();

    public virtual ICollection<AcademicProgramGraduateCriterion> AcademicProgramGraduateCriteria { get; set; } = new List<AcademicProgramGraduateCriterion>();

    public virtual ICollection<AcademicProgramNote> AcademicProgramNotes { get; set; } = new List<AcademicProgramNote>();

    public virtual ICollection<AcademicProgramRequirement> AcademicProgramRequirements { get; set; } = new List<AcademicProgramRequirement>();

    public virtual ICollection<AcademicProgramSystem> AcademicProgramSystems { get; set; } = new List<AcademicProgramSystem>();

    public virtual ICollection<DonorScholarshipDetail> DonorScholarshipDetails { get; set; } = new List<DonorScholarshipDetail>();
}
