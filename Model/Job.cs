using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GrobsJobsRazorPages.Model;

public partial class Job
{
    [Key]
    public int Id { get; set; }
    [Required]
    [Display(Name = "Job Name")]
    public string Name { get; set; }
    [Required]
    [Display(Name = "Job Description")]
    public string Description { get; set; }
    [Required]
    public string JobType { get; set; }
    [Required]
    [Display(Name = "Job Poster Id")]
    public string JobPosterId { get; set; }
    [Required]
    [Display(Name = "Job Poster User Name")]
    public string JobPosterNormalizedUserName { get; set; }
    [Required]
    public DateTime DateTimePosted { get; set; }
}
