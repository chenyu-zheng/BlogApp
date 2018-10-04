namespace BlogApp.Migrations
{
    using BlogApp.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Configuration;

    internal sealed class Configuration : DbMigrationsConfiguration<BlogApp.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BlogApp.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            if (!context.Roles.Any(r => r.Name == "Moderator"))
            {
                roleManager.Create(new IdentityRole { Name = "Moderator" });
            }


            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            ApplicationUser adminUser = null;
            var adminUserName = WebConfigurationManager.AppSettings["admin-username"];
            if (!context.Users.Any(p => p.UserName == adminUserName))
            {
                adminUser = new ApplicationUser();
                adminUser.UserName = adminUserName;
                adminUser.Email = adminUserName;
                adminUser.FirstName = WebConfigurationManager.AppSettings["admin-firstname"];
                adminUser.LastName = WebConfigurationManager.AppSettings["admin-lastname"];
                adminUser.DisplayName = WebConfigurationManager.AppSettings["admin-displayname"];

                userManager.Create(adminUser, WebConfigurationManager.AppSettings["admin-password"]);
            }
            else
            {
                adminUser = context.Users.Where(p => p.UserName == adminUserName)
                    .FirstOrDefault();
            }

            if (!userManager.IsInRole(adminUser.Id, "Admin"))
            {
                userManager.AddToRole(adminUser.Id, "Admin");
            }

            ApplicationUser moderatorUser = null;
            var modUserName = WebConfigurationManager.AppSettings["moderator-username"];
            if (!context.Users.Any(p => p.UserName == modUserName))
            {
                moderatorUser = new ApplicationUser();
                moderatorUser.UserName = modUserName;
                moderatorUser.Email = modUserName;
                moderatorUser.FirstName = WebConfigurationManager.AppSettings["moderator-firstname"];
                moderatorUser.LastName = WebConfigurationManager.AppSettings["moderator-lastname"];
                moderatorUser.DisplayName = WebConfigurationManager.AppSettings["moderator-displayname"];

                userManager.Create(moderatorUser, WebConfigurationManager.AppSettings["moderator-password"]);
            }
            else
            {
                moderatorUser = context.Users.Where(p => p.UserName == modUserName)
                    .FirstOrDefault();
            }

            if (!userManager.IsInRole(moderatorUser.Id, "Moderator"))
            {
                userManager.AddToRole(moderatorUser.Id, "Moderator");
            }
        }
    }
}
