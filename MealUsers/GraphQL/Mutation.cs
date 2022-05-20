using HotChocolate.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using UserService.Models;

namespace UserService.GraphQL
{
    public class Mutation
    {

        public async Task<UserData> RegisterUserAsync(
           RegisterUser input,
           [Service] MealDeliveryContext context)
        {
            var user = context.Users.Where(o => o.Username == input.UserName).FirstOrDefault();
            if (user != null)
            {
                return await Task.FromResult(new UserData());
            }
            var newUser = new User
            {
                FullName = input.FullName,
                Email = input.Email,
                Username = input.UserName,
                Password = BCrypt.Net.BCrypt.HashPassword(input.Password) // encrypt password
            };

            // EF
            var ret = context.Users.Add(newUser);
            await context.SaveChangesAsync();

            return await Task.FromResult(new UserData
            {
                Id = newUser.Id,
                Username = newUser.Username,
                Email = newUser.Email,
                FullName = newUser.FullName
            });
        }

        public async Task<UserToken> LoginAsync(
            LoginUser input,
            [Service] IOptions<TokenSettings> tokenSettings, // setting token
            [Service] MealDeliveryContext context) // EF
        {
            var user = context.Users.Where(o => o.Username == input.Username).FirstOrDefault();
            if (user == null)
            {
                return await Task.FromResult(new UserToken(null, null, "Username or password was invalid"));
            }
            bool valid = BCrypt.Net.BCrypt.Verify(input.Password, user.Password);
            if (valid)
            {
                // generate jwt token
                var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettings.Value.Key));
                var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

                // jwt payload
                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, user.Username));

                var userRoles = context.UserRoles.Where(o => o.Id == user.Id).ToList();
                foreach (var userRole in userRoles)
                {
                    var role = context.Roles.Where(o => o.Id == userRole.RoleId).FirstOrDefault();
                    if (role != null)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role.Name));
                    }
                }

                var expired = DateTime.Now.AddHours(3);
                var jwtToken = new JwtSecurityToken(
                    issuer: tokenSettings.Value.Issuer,
                    audience: tokenSettings.Value.Audience,
                    expires: expired,
                    claims: claims, // jwt payload
                    signingCredentials: credentials // signature
                );

                return await Task.FromResult(
                    new UserToken(new JwtSecurityTokenHandler().WriteToken(jwtToken),
                    expired.ToString(), null));
                //return new JwtSecurityTokenHandler().WriteToken(jwtToken);
            }

            return await Task.FromResult(new UserToken(null, null, Message: "Username or password was invalid"));
        }

        [Authorize(Roles = new[] { "ADMIN" })]
        public async Task<User> UpdatePasswordAsync(
            ChangePassword input,
            [Service] MealDeliveryContext context)
        {
            var user = context.Users.Where(o => o.Id == input.Id).FirstOrDefault();
            if (user != null)
            {
                user.Username = input.username;
                user.Password = BCrypt.Net.BCrypt.HashPassword(input.password);


                context.Users.Update(user);
                await context.SaveChangesAsync();
            }


            return await Task.FromResult(user);
        }

        [Authorize]
        public async Task<ProfileData> AddProfileAsync(
            ProfileInput input,
            [Service] MealDeliveryContext context)
        {
            var profile = context.Profiles.Where(o => o.UserId == input.UserId).FirstOrDefault();
            if (profile != null)
            {
                return await Task.FromResult(new ProfileData());
            }

            var newProfile = new Profile
            {
                UserId = input.UserId,
                Name = input.Name,
                Address = input.Address,
                City = input.City,
                Phone = input.Phone,
            };

            var ret = context.Profiles.Add(newProfile);
            await context.SaveChangesAsync();

            return await Task.FromResult(new ProfileData
            {
                UserId = newProfile.UserId,
                Name = newProfile.Name,
                Address = newProfile.Address,
                City = newProfile.City,
                Phone = newProfile.Phone
            });





        }

        [Authorize(Roles = new[] { "ADMIN" })]
        public async Task<User> DeleteUserByIdAsync(
           int id,
           [Service] MealDeliveryContext context)
        {
            var product = context.Users.Where(o => o.Id == id).Include(s => s.UserRoles).Include(a => a.Profiles).Include(s => s.Orders).FirstOrDefault();
            if (product != null)
            {
                context.Users.Remove(product);
                await context.SaveChangesAsync();
            }


            return await Task.FromResult(product);
        }

        [Authorize(Roles = new[] { "ADMIN" })]
        public async Task<UserRoleData> AddUserRoleAsync(
           UserRoleInput input,
           [Service] MealDeliveryContext context)
        {
            var userrole = context.UserRoles.Where(o => o.Id == input.Id).FirstOrDefault();
            if (userrole != null)
            {
                return await Task.FromResult(new UserRoleData());
            }

            var newUserRole = new UserRole
            {
                UserId = input.UserId,
                RoleId = input.RoleId

            };

            var ret = context.UserRoles.Add(newUserRole);
            await context.SaveChangesAsync();

            return await Task.FromResult(new UserRoleData
            {
                Id = newUserRole.Id,
                UserId = newUserRole.UserId,
                RoleId = newUserRole.RoleId
            });





        }

        public async Task<CourierData> AddCourierAsync(
          CourierInput input,
          [Service] MealDeliveryContext context)
        {
            var courier = context.Couriers.Where(o => o.Name == input.Name).FirstOrDefault();
            if (courier != null)
            {
                return await Task.FromResult(new CourierData());
            }
            var newCourier = new Courier
            {
                Name = input.Name,
                Phone = input.Phone,
                UserId = input.UserId

            };

            // EF
            var ret = context.Couriers.Add(newCourier);
            await context.SaveChangesAsync();

            return await Task.FromResult(new CourierData
            {
                Id = newCourier.Id,
                Name = newCourier.Name,
                Phone = newCourier.Phone,
                UserId = newCourier.UserId

            });
        }




    }
}
