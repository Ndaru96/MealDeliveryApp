﻿using HotChocolate.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UserService.Models;

namespace UserService.GraphQL
{
    public class Query
    {
        [Authorize] // dapat diakses kalau sudah login
        public IQueryable<User> GetUsers([Service] MealAppContext context, ClaimsPrincipal claimsPrincipal)
        {
            var userName = claimsPrincipal.Identity.Name;

            // check admin role ?
            var adminRole = claimsPrincipal.Claims.Where(o => o.Type == ClaimTypes.Role).FirstOrDefault();
            var user = context.Users.Where(o => o.Username == userName).FirstOrDefault();
            if (user != null)
            {
                if (adminRole.Value == "ADMIN")
                {
                    return context.Users.Include(p => p.Profiles);
                }
                var orders = context.Users.Where(o => o.Id == user.Id).Include(s => s.Profiles);
                return orders.AsQueryable();

            }


            return new List<User>().AsQueryable();
        }

        [Authorize] // dapat diakses kalau sudah login
        public IQueryable<Role> GetRole([Service] MealAppContext context, ClaimsPrincipal claimsPrincipal)
        {
            var userName = claimsPrincipal.Identity.Name;

            // check admin role ?
            var adminRole = claimsPrincipal.Claims.Where(o => o.Type == ClaimTypes.Role).FirstOrDefault();
            var user = context.Users.Where(o => o.Username == userName).FirstOrDefault();
            if (user != null)
            {
                if (adminRole.Value == "ADMIN")
                {
                    return context.Roles;
                }
               
            }


            return new List<Role>().AsQueryable();
        }

        [Authorize] // dapat diakses kalau sudah login
        public IQueryable<UserRole> GetUserRole([Service] MealAppContext context, ClaimsPrincipal claimsPrincipal)
        {
            var userName = claimsPrincipal.Identity.Name;

            // check admin role ?
            var adminRole = claimsPrincipal.Claims.Where(o => o.Type == ClaimTypes.Role).FirstOrDefault();
            var user = context.Users.Where(o => o.Username == userName).FirstOrDefault();
            if (user != null)
            {
                if (adminRole.Value == "ADMIN")
                {
                    return context.UserRoles;
                }

            }


            return new List<UserRole>().AsQueryable();
        }

        [Authorize(Roles = new[] { "ADMIN" })]// dapat diakses kalau sudah login
        public IQueryable<UserRole> GetUserRoleByRoleId(
            int id, [Service] MealAppContext context)
        {
         // check admin role ?
            
            var user = context.UserRoles.Where(o => o.RoleId == id).FirstOrDefault();
            if (user != null)

            { 
                return context.UserRoles.Where(o => o.RoleId == id);

            }
            


            return new List<UserRole>().AsQueryable();
        }
    }
}
