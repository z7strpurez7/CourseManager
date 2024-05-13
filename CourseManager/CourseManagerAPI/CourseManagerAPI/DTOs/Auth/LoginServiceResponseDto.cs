namespace CourseManagerAPI.DTOs.Auth
{
    public class LoginServiceResponseDto
    {
        public string NewToken { get; set; }

        //skal returnes til gui
        public UserInfoResult UserInfo { get; set; }
    }
}
