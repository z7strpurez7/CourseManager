namespace CourseManagerAPI.Constants
{
    public static class StaticUserRoles
    {
        public const string OWNER = "OWNER";
        public const string ADMIN = "ADMIN";
        public const string LECTURER = "LECTURER";
        public const string USER = "USER";


        public const string OWNERADMIN = "OWNER,ADMIN";
        public const string OWNERADMINLECTURER = "OWNER,ADMIN,LECTURER";
        public const string OWNERADMINLECTURERUSER = "OWNER,ADMIN,LECTURER,USER";
        public const string LECTURERUSER = "LECTURER,USER";
        public const string ADMINLECTURERUSER= "ADMIN, LECTURER, USER";
    }
}
