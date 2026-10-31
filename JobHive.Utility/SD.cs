namespace JobHive.Utility
{
    public static class SD
    {
        // Roles
        public const string Role_Admin = "Admin";
        public const string Role_Employer = "Employer";
        public const string Role_Applicant = "Applicant";

        // Application Statuses
        public const string Status_Applied = "Applied";
        public const string Status_UnderReview = "Under Review";
        public const string Status_Shortlisted = "Shortlisted";
        public const string Status_Interview = "Interview";
        public const string Status_Offered = "Offered";
        public const string Status_Rejected = "Rejected";

        // Job Types
        public const string JobType_FullTime = "Full-Time";
        public const string JobType_PartTime = "Part-Time";
        public const string JobType_Remote = "Remote";
        public const string JobType_Contract = "Contract";
        public const string JobType_Internship = "Internship";

        // Experience Levels
        public const string Exp_Entry = "Entry Level";
        public const string Exp_Mid = "Mid Level";
        public const string Exp_Senior = "Senior Level";
        public const string Exp_Lead = "Lead / Manager";

        // Session keys
        public const string Session_Role = "CurrentRole";
        public const string Session_UserName = "CurrentUserName";
        public const string Session_UserEmail = "CurrentUserEmail";
    }
}
