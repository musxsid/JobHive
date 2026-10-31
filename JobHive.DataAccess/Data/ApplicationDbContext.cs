using JobHive.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace JobHive.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Job> Jobs { get; set; } = null!;
        public DbSet<JobApplication> JobApplications { get; set; } = null!;
        public DbSet<CandidateProfile> CandidateProfiles { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Job - Category relationship
            modelBuilder.Entity<Job>()
                .HasOne(j => j.Category)
                .WithMany(c => c.Jobs)
                .HasForeignKey(j => j.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Job - JobApplication relationship
            modelBuilder.Entity<JobApplication>()
                .HasOne(a => a.Job)
                .WithMany(j => j.Applications)
                .HasForeignKey(a => a.JobId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes for fast multi-parameter search & compound query performance
            modelBuilder.Entity<Job>()
                .HasIndex(j => j.Title);

            modelBuilder.Entity<Job>()
                .HasIndex(j => j.Location);

            modelBuilder.Entity<Job>()
                .HasIndex(j => j.SalaryMin);

            modelBuilder.Entity<Job>()
                .HasIndex(j => j.CategoryId);

            modelBuilder.Entity<Job>()
                .HasIndex(j => j.JobType);

            // Seed Categories matching UI
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Development & IT", Icon = "bi-laptop", DisplayOrder = 1, Description = "Software engineering, cloud architecture, and full stack development." },
                new Category { Id = 2, Name = "Design & Branding", Icon = "bi-palette", DisplayOrder = 2, Description = "UI/UX design, visual identity, product graphics, and branding." },
                new Category { Id = 3, Name = "Sales & Marketing", Icon = "bi-megaphone", DisplayOrder = 3, Description = "Digital marketing, brand growth, SEO, and business development." },
                new Category { Id = 4, Name = "Customer Services", Icon = "bi-headset", DisplayOrder = 4, Description = "Customer success, technical support, and client experience." },
                new Category { Id = 5, Name = "Accounting & Finance", Icon = "bi-pie-chart", DisplayOrder = 5, Description = "Corporate finance, auditing, payroll, and investment analysis." },
                new Category { Id = 6, Name = "Human Resources", Icon = "bi-people", DisplayOrder = 6, Description = "Talent acquisition, employee engagement, and HR operations." },
                new Category { Id = 7, Name = "Production & Operation", Icon = "bi-gear", DisplayOrder = 7, Description = "Supply chain, quality control, manufacturing, and logistics." },
                new Category { Id = 8, Name = "Engineering / Architecture", Icon = "bi-tools", DisplayOrder = 8, Description = "Civil, structural, electrical, and systems engineering." }
            );

            // Seed Jobs
            modelBuilder.Entity<Job>().HasData(
                new Job
                {
                    Id = 1,
                    Title = "Senior Full Stack .NET Developer",
                    CompanyName = "CodeArrest Softwares",
                    CompanyLogo = "bi-cpu",
                    Location = "Bangalore, India",
                    IsRemote = true,
                    JobType = "Full-Time",
                    ExperienceLevel = "Senior Level",
                    SalaryMin = 1400000,
                    SalaryMax = 2200000,
                    Description = "We are seeking an experienced .NET Core Engineer to build high-scale web platforms, optimize database execution times, and lead architecture initiatives.",
                    Requirements = "• 4+ years of hands-on experience with C#, ASP.NET Core MVC/WebAPI\n• Strong command over Entity Framework Core and SQL Server\n• Experience designing normalized database schemas and compound query optimizations\n• Familiarity with modern frontend integration (Bootstrap, JavaScript, Razor)",
                    Responsibilities = "• Design, develop, and maintain clean and testable backend services\n• Profile database queries to minimize latency and optimize throughput\n• Collaborate with product managers and recruiters to deliver seamless user workflows",
                    Benefits = "Health Insurance, Flexible Remote Work, Annual Performance Bonus, Learning Stipend",
                    CategoryId = 1,
                    PostedDate = DateTime.UtcNow.AddDays(-2),
                    EmployerEmail = "careers@codearrest.com",
                    IsActive = true
                },
                new Job
                {
                    Id = 2,
                    Title = "Lead UI/UX Product Designer",
                    CompanyName = "PixelCraft Studios",
                    CompanyLogo = "bi-brush",
                    Location = "Pune, India",
                    IsRemote = true,
                    JobType = "Full-Time",
                    ExperienceLevel = "Lead / Manager",
                    SalaryMin = 1600000,
                    SalaryMax = 2500000,
                    Description = "Looking for a creative and data-informed UI/UX Designer to craft intuitive, accessible, and visually stunning web and mobile application experiences.",
                    Requirements = "• 5+ years experience in product design, Figma, and design systems\n• Deep understanding of user research, wireframing, and interactive prototyping\n• Portfolio demonstrating end-to-end user journeys and clean modern typography",
                    Responsibilities = "• Create high-fidelity design prototypes and design tokens\n• Collaborate directly with engineering teams to ensure pixel-perfect implementation\n• Conduct usability testing and gather feedback from candidates and employers",
                    Benefits = "Work From Anywhere, Equipment Allowance, Health & Wellness Cover",
                    CategoryId = 2,
                    PostedDate = DateTime.UtcNow.AddDays(-5),
                    EmployerEmail = "talent@pixelcraft.io",
                    IsActive = true
                },
                new Job
                {
                    Id = 3,
                    Title = "Data Analyst & Business Intelligence",
                    CompanyName = "DataVerve Analytics",
                    CompanyLogo = "bi-bar-chart",
                    Location = "Hyderabad, India",
                    IsRemote = false,
                    JobType = "Full-Time",
                    ExperienceLevel = "Mid Level",
                    SalaryMin = 900000,
                    SalaryMax = 1500000,
                    Description = "Analyze key recruitment metrics, candidate pipeline flow, and employer conversion data to extract actionable business intelligence.",
                    Requirements = "• 2+ years of experience with SQL, Power BI, and Python\n• Proven track record transforming complex relational datasets into clear dashboards\n• Excellent communication skills to present insights to leadership",
                    Responsibilities = "• Build and maintain automated SQL Server ETL pipelines and executive reports\n• Partner with product teams to track user behavior and funnel bottlenecks\n• Optimize SQL analytical queries for fast dashboard rendering",
                    Benefits = "Provident Fund, Medical Insurance, Gym Membership, Performance Incentives",
                    CategoryId = 1,
                    PostedDate = DateTime.UtcNow.AddDays(-3),
                    EmployerEmail = "hiring@dataverve.com",
                    IsActive = true
                },
                new Job
                {
                    Id = 4,
                    Title = "Growth Marketing Specialist",
                    CompanyName = "NextGen Media",
                    CompanyLogo = "bi-graph-up-arrow",
                    Location = "Delhi NCR, India",
                    IsRemote = true,
                    JobType = "Full-Time",
                    ExperienceLevel = "Mid Level",
                    SalaryMin = 800000,
                    SalaryMax = 1300000,
                    Description = "Drive organic and paid user acquisition for JobHive. Lead omnichannel digital marketing campaigns to connect job seekers with top recruiters.",
                    Requirements = "• 3+ years experience with Google Ads, SEO, LinkedIn marketing, and conversion optimization\n• Strong analytical mindset and data-backed experimentation approach\n• Experience in B2B and recruitment marketing is a plus",
                    Responsibilities = "• Plan and execute multi-channel user acquisition campaigns\n• Optimize landing pages and search engine visibility for job listings\n• Track CAC, retention rates, and channel ROI",
                    Benefits = "Remote First, Quarterly Bonuses, Mental Health Support",
                    CategoryId = 3,
                    PostedDate = DateTime.UtcNow.AddDays(-7),
                    EmployerEmail = "jobs@nextgenmedia.com",
                    IsActive = true
                },
                new Job
                {
                    Id = 5,
                    Title = "Customer Support & Success Lead",
                    CompanyName = "SupportHero Global",
                    CompanyLogo = "bi-chat-dots",
                    Location = "Bangalore, India",
                    IsRemote = true,
                    JobType = "Full-Time",
                    ExperienceLevel = "Lead / Manager",
                    SalaryMin = 750000,
                    SalaryMax = 1200000,
                    Description = "Manage candidate and recruiter inquiries, resolve platform issues promptly, and establish high customer satisfaction benchmarks.",
                    Requirements = "• 3+ years in tech support, Zendesk/Freshdesk, and team leadership\n• Superb empathetic communication and troubleshooting skills\n• Ability to work across varied shift windows when needed",
                    Responsibilities = "• Supervise tier-1 and tier-2 customer support channels\n• Escalate technical bugs to the software engineering team with reproduction steps\n• Maintain average response time under 15 minutes",
                    Benefits = "Night Shift Allowance, Internet Reimbursement, Comprehensive Healthcare",
                    CategoryId = 4,
                    PostedDate = DateTime.UtcNow.AddDays(-1),
                    EmployerEmail = "careers@supporthero.com",
                    IsActive = true
                },
                new Job
                {
                    Id = 6,
                    Title = "Financial Analyst & Auditor",
                    CompanyName = "Apex Financial Group",
                    CompanyLogo = "bi-currency-exchange",
                    Location = "Mumbai, India",
                    IsRemote = false,
                    JobType = "Full-Time",
                    ExperienceLevel = "Senior Level",
                    SalaryMin = 1200000,
                    SalaryMax = 1800000,
                    Description = "Oversee financial modeling, statutory compliance, audit prep, and budgeting forecasts for fast-growing technology enterprises.",
                    Requirements = "• CA / CFA / MBA Finance with 4+ years of relevant corporate finance experience\n• Advanced financial modeling in Excel and ERP tools\n• Deep knowledge of Indian taxation, GST, and audit procedures",
                    Responsibilities = "• Formulate quarterly budget forecasts and variance analyses\n• Conduct internal audit checkpoints to guarantee 100% regulatory compliance\n• Present quarterly fiscal reviews to executive management",
                    Benefits = "Competitive Bonus, Retirement Savings Plan, Executive Health Insurance",
                    CategoryId = 5,
                    PostedDate = DateTime.UtcNow.AddDays(-4),
                    EmployerEmail = "recruitment@apexfin.com",
                    IsActive = true
                },
                new Job
                {
                    Id = 7,
                    Title = "Technical Recruiter & Talent Partner",
                    CompanyName = "HireSphere Consultancy",
                    CompanyLogo = "bi-person-check",
                    Location = "Bangalore, India",
                    IsRemote = true,
                    JobType = "Full-Time",
                    ExperienceLevel = "Mid Level",
                    SalaryMin = 700000,
                    SalaryMax = 1100000,
                    Description = "Source, evaluate, and place world-class software engineers, product managers, and data scientists across high-growth startups.",
                    Requirements = "• 2+ years of tech recruitment experience in India tech markets\n• Proven sourcing techniques across LinkedIn, GitHub, and job portals\n• Strong relationship-building skills with hiring managers and candidates",
                    Responsibilities = "• Manage end-to-end recruitment lifecycle from sourcing to offer negotiation\n• Conduct initial technical screening and profile alignment\n• Partner with tech leads to understand evolving team skill requirements",
                    Benefits = "Uncapped Placement Commissions, Flexible Hours, Annual Offsite",
                    CategoryId = 6,
                    PostedDate = DateTime.UtcNow.AddDays(-6),
                    EmployerEmail = "hr@hiresphere.io",
                    IsActive = true
                },
                new Job
                {
                    Id = 8,
                    Title = "DevOps & Cloud Systems Engineer",
                    CompanyName = "CloudSphere Tech",
                    CompanyLogo = "bi-cloud-check",
                    Location = "Pune, India",
                    IsRemote = true,
                    JobType = "Contract",
                    ExperienceLevel = "Senior Level",
                    SalaryMin = 1500000,
                    SalaryMax = 2400000,
                    Description = "Manage automated CI/CD pipelines, containerized microservices in Docker/Kubernetes, and ensure 99.99% cloud infrastructure reliability.",
                    Requirements = "• 4+ years in AWS/Azure cloud architecture, Terraform, and Docker\n• Strong Linux administration and Bash/PowerShell scripting\n• Experience with SQL Server clustering, database backups, and network security",
                    Responsibilities = "• Maintain infrastructure-as-code and streamline multi-stage deployment pipelines\n• Implement observability dashboards with Prometheus and Grafana\n• Conduct security auditing and disaster recovery drills",
                    Benefits = "Flexible Contract Terms, Remote Work, Tech Gear Allowance",
                    CategoryId = 1,
                    PostedDate = DateTime.UtcNow.AddDays(-3),
                    EmployerEmail = "cloud-hiring@cloudsphere.net",
                    IsActive = true
                }
            );

            // Seed Sample Candidate Profile
            modelBuilder.Entity<CandidateProfile>().HasData(
                new CandidateProfile
                {
                    Id = 1,
                    FullName = "Siddharth Sharma",
                    Email = "siddharth@example.com",
                    Phone = "+91 98765 43210",
                    Headline = "Full Stack .NET Core Software Developer",
                    Location = "Kota / Bangalore, India",
                    Skills = "C#, ASP.NET Core MVC, Entity Framework Core, SQL Server, HTML5, CSS3, JavaScript, REST APIs",
                    ExperienceYears = 2,
                    ResumeFileName = "Siddharth_Sharma_Resume.pdf",
                    ResumePath = "/uploads/resumes/sample_resume.pdf",
                    Bio = "Software developer intern at CodeArrest Softwares. Passionate about designing robust relational schemas, optimizing SQL database query times, and building intuitive user interfaces.",
                    UpdatedAt = DateTime.UtcNow
                }
            );

            // Seed Sample Job Applications across various workflow stages
            modelBuilder.Entity<JobApplication>().HasData(
                new JobApplication
                {
                    Id = 1,
                    JobId = 1,
                    ApplicantName = "Siddharth Sharma",
                    ApplicantEmail = "siddharth@example.com",
                    ApplicantPhone = "+91 98765 43210",
                    CoverLetter = "I have extensive experience working with .NET Core MVC, EF Core, and SQL Server. I built JobHive to streamline multi-role recruitment workflows and compound filtering.",
                    ResumeFileName = "Siddharth_Sharma_Resume.pdf",
                    ResumePath = "/uploads/resumes/sample_resume.pdf",
                    AppliedDate = DateTime.UtcNow.AddDays(-2),
                    Status = "Interview",
                    RecruiterNotes = "Candidate cleared technical round 1 with distinction on EF Core relational modeling."
                },
                new JobApplication
                {
                    Id = 2,
                    JobId = 1,
                    ApplicantName = "Ananya Verma",
                    ApplicantEmail = "ananya.verma@example.com",
                    ApplicantPhone = "+91 91234 56789",
                    CoverLetter = "Excited to apply for the Senior .NET position. I bring 4 years of backend C# experience building enterprise microservices.",
                    ResumeFileName = "Ananya_Verma_Resume.pdf",
                    ResumePath = "/uploads/resumes/sample_resume.pdf",
                    AppliedDate = DateTime.UtcNow.AddDays(-4),
                    Status = "Under Review",
                    RecruiterNotes = "Portfolio reviewed by tech lead. Scheduled for code evaluation."
                },
                new JobApplication
                {
                    Id = 3,
                    JobId = 2,
                    ApplicantName = "Rohan Mehta",
                    ApplicantEmail = "rohan.mehta@example.com",
                    ApplicantPhone = "+91 97890 12345",
                    CoverLetter = "I specialize in clean design systems and responsive web typography. Excited about the vision at PixelCraft.",
                    ResumeFileName = "Rohan_Mehta_Portfolio.pdf",
                    ResumePath = "/uploads/resumes/sample_resume.pdf",
                    AppliedDate = DateTime.UtcNow.AddDays(-5),
                    Status = "Offered",
                    RecruiterNotes = "Offer letter rolled out with competitive joining bonus."
                },
                new JobApplication
                {
                    Id = 4,
                    JobId = 3,
                    ApplicantName = "Priya Nair",
                    ApplicantEmail = "priya.nair@example.com",
                    ApplicantPhone = "+91 99887 76655",
                    CoverLetter = "Strong background in SQL Server query optimization and Power BI visualization. Looking forward to driving analytics at DataVerve.",
                    ResumeFileName = "Priya_Nair_Resume.pdf",
                    ResumePath = "/uploads/resumes/sample_resume.pdf",
                    AppliedDate = DateTime.UtcNow.AddDays(-1),
                    Status = "Applied",
                    RecruiterNotes = "New application received; pending recruiter screening."
                }
            );
        }
    }
}
