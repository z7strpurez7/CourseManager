using CourseManagerAPI.Constants;
using CourseManagerAPI.DTOs.Auth;
using CourseManagerAPI.DTOs.General;
using CourseManagerAPI.DTOs.Request;
using CourseManagerAPI.Entities;
using CourseManagerAPI.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CourseManagerAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogService _logService;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<UserEntity> userManager, RoleManager<IdentityRole> roleManager, ILogService logService, IConfiguration configuration)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            _logService = logService;
            this._configuration = configuration;
        }
        public async Task<GeneralServiceResponseDto> SeedRolesAsync()
        {
            bool isOwnerRoleExist = await _roleManager.RoleExistsAsync(StaticUserRoles.OWNER);
            bool isAdminRoleExist = await _roleManager.RoleExistsAsync(StaticUserRoles.ADMIN);
            bool isLecturerRoleExist = await _roleManager.RoleExistsAsync(StaticUserRoles.LECTURER);
            bool isUserRoleExist = await _roleManager.RoleExistsAsync(StaticUserRoles.USER);

            if (isOwnerRoleExist && isAdminRoleExist && isLecturerRoleExist && isUserRoleExist)
            {
                return new GeneralServiceResponseDto
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Message = "Role Seeding is already done"
                };
            }
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.OWNER));
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.ADMIN));
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.LECTURER));
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.USER));
            return new GeneralServiceResponseDto
            {
                IsSuccess = true,
                StatusCode = 201,
                Message = "Seeding completed successfully"
            };
        }

        public async Task<GeneralServiceResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            var isExistingUser = await _userManager.FindByNameAsync(registerDto.UserName);
            if (isExistingUser is not null)
            {
                return new GeneralServiceResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 409,
                    Message = "UserName is already in use"
                };
            }
            var validRoles = StaticUserRoles.ADMINLECTURERUSER;

            if(!validRoles.Contains(registerDto.Role)){

                return new GeneralServiceResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Role not Accepted"
                };
            }
            UserEntity newUser = new UserEntity()
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                Address = registerDto.Address,
                Birthday = registerDto.Birthday,
                SecurityStamp = Guid.NewGuid().ToString(),
            };
            var createUserResult = await _userManager.CreateAsync(newUser, registerDto.Password.ToString());
            
            if (!createUserResult.Succeeded)
            {
                var errorString = "User Creation failed: ";
                foreach (var error in createUserResult.Errors)
                {
                    errorString += '#' + error.Description;
                }
                return new GeneralServiceResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = errorString
                };
            }
            //Add default user role to all users
            await _userManager.AddToRoleAsync(newUser, registerDto.Role);
            await _logService.SaveNewLog(newUser.UserName, "Registered");

            return new GeneralServiceResponseDto
            {
                IsSuccess = true,
                StatusCode = 201,
                Message = "User Registered"

            };
        }

        public async Task<LoginServiceResponseDto?> LoginAsync(LoginDto loginDto)
        {
            //finner bruker
            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user is null)
            {
                return null;
            }

            //check password
            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isPasswordCorrect)
            {
                return null;
            }


            //Helper Functions&tasks  defined in the bottom
            //If valid login, return token and userinfo
            var newToken = await GenerateJWTTokenAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var userInfo = GenerateUserInfoObject(user, roles);
            await _logService.SaveNewLog(user.UserName, "New Login");

            return new LoginServiceResponseDto()
            {
                NewToken = newToken,
                UserInfo = userInfo,
            };
        }

        public async Task<GeneralServiceResponseDto> UpdateRoleAsync(ClaimsPrincipal User, UpdateRoleDto updateRoleDto)
        {

            //check if user exists
            var user = await _userManager.FindByNameAsync(updateRoleDto.UserName);
            if(user is null)
            {
                return new GeneralServiceResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Invalid user name"
                };
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            //User is Admin
            
            //Legg merke til stor U
            if (User.IsInRole(StaticUserRoles.ADMIN))
            {
                //Just the owner and admin be updated
                if (updateRoleDto.NewRole == RoleType.USER || updateRoleDto.NewRole == RoleType.LECTURER)
                {
                    //Admin can change the role of everyone except owners and admins
                    if (userRoles.Any(q => q.Equals(StaticUserRoles.OWNER) || q.Equals(StaticUserRoles.ADMIN)))
                    {
                        return new GeneralServiceResponseDto()
                        {
                            IsSuccess = false,
                            StatusCode = 403,
                            Message = "You are not allowed to change the role of this user"
                        };
                    }
                    else
                    {
                        await _userManager.RemoveFromRolesAsync(user, userRoles);
                        await _userManager.AddToRoleAsync(user, updateRoleDto.NewRole.ToString());
                        await _logService.SaveNewLog(user.UserName, "User Roles Updated");
                        return new GeneralServiceResponseDto()
                        {
                            IsSuccess = true,
                            StatusCode = 200,
                            Message = "Role updated successfully"
                        };
                    }
                }
                else return new GeneralServiceResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 403,
                    Message = "You dont have access to change roles"
                };
            }
            else
            {
                // If the user passed in are a Owner
                if (userRoles.Any(q => q.Equals(StaticUserRoles.OWNER)))
                {
                    return new GeneralServiceResponseDto()
                    {
                        IsSuccess = false,
                        StatusCode = 403,
                        Message = "You are not allowed to change role of this user"
                    };
                }
                else
                {
                    await _userManager.RemoveFromRolesAsync(user, userRoles);
                    await _userManager.AddToRoleAsync(user, updateRoleDto.NewRole.ToString());
                    await _logService.SaveNewLog(user.UserName, "User Roles Updated");

                    return new GeneralServiceResponseDto()
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        Message = "Role updated successfully"
                    };
                }
            }
        }

        public async Task<LoginServiceResponseDto> MeAsync(MeDto meDto)
        {
            //tokenvalidation parameters er samme som i program
            //må bare bytte ut builder med _config for å få tilgang
            ClaimsPrincipal handler = new JwtSecurityTokenHandler().ValidateToken(meDto.Token, new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _configuration["JWT:ValidIssuer"],
                ValidAudience = _configuration["JWT:ValidAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]))
            }, out SecurityToken securityToken);

            string decodedUserName = handler.Claims.First(q => q.Type == ClaimTypes.Name).Value;

            //notValid  username
            if (decodedUserName is null)
            {
                return null;
            }

            var user = await _userManager.FindByNameAsync(decodedUserName);
            if (user == null)
            {
                return null;
            }
            //user exits
            //samme som i loginAsyc()
            var newToken = await GenerateJWTTokenAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var userInfo = GenerateUserInfoObject(user, roles);
            await _logService.SaveNewLog(user.UserName, "New Token Generated");

            return new LoginServiceResponseDto()
            {
                NewToken = newToken,
                UserInfo = userInfo
        };
        }

        public async Task<IEnumerable<UserInfoResult>> GetUserListAsync()
        {
            var users = await _userManager.Users.ToListAsync();

            //For å ikke returne for mye unødvendig info
            List<UserInfoResult> userInfoResults = new List<UserInfoResult>();

            //GeneratueUserInfoObject er definert nederst på siden
            foreach (var user in users) {
                var roles = await _userManager.GetRolesAsync(user);
                var userInfo= GenerateUserInfoObject(user, roles);
                userInfoResults.Add(userInfo);
            }

            return userInfoResults;
        }

        //kan finne userdetails ved hjelp av annet også
        public async Task<UserInfoResult?> GetUserDetailsByUserNameAsync(string UserName)
        {
            
           var user = await _userManager.FindByNameAsync(UserName);
            if(user is null)
            {
                return null;
            }

            var roles = await _userManager.GetRolesAsync(user);
            //definert nederst
            var userInfo = GenerateUserInfoObject(user, roles);
            return userInfo;
        }


        public async Task<IEnumerable<TeacherNameDto>> GetTeacherNamesListAsync()
        {
            var users = await _userManager.Users.ToListAsync();

            var teachers = new List<TeacherNameDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if(roles.Contains(StaticUserRoles.LECTURER))
                {
                    var TeacherNames = new TeacherNameDto
                    {
                        TeacherId = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName
                    };
                    teachers.Add(TeacherNames);
                }
            }

            return teachers.ToList();
        }




            // Assistant funcs
            //  GenerateJWTTokenAsync, claims
            private async Task<string> GenerateJWTTokenAsync(UserEntity user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
            };

            //Adding claims to user
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            //Same as in program.cs file
            //får tilgang til denne gjennom iConfiguration
            var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var signingCredentials = new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256);

            var tokenObject = new JwtSecurityToken(
               issuer: _configuration["JWT:ValidIssuer"],
               audience: _configuration["JWT:ValidAudience"],
               notBefore: DateTime.Now,
               expires: DateTime.Now.AddHours(3),
               claims: authClaims,
               signingCredentials: signingCredentials
               );

            string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);
            return token;
        }


        //GenerateUserInfoObject (in DTO/AUTH folder)
        private UserInfoResult GenerateUserInfoObject(UserEntity user, IEnumerable<string> Roles)
        {
            //Kan vurdere automapper packages
            return new UserInfoResult()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Birthday = user.Birthday,
                UserName = user.UserName,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                Roles = Roles
            };
        }
    }

}


