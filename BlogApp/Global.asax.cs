﻿using BlogApp.Migrations;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BlogApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Update database
            var configuration = new Configuration();
            var migrator = new DbMigrator(configuration);
            migrator.Update();

            // Create Uploads folder
            System.IO.Directory.CreateDirectory(Server.MapPath("~/Uploads"));
        }
    }
}
