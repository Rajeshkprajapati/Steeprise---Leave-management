using System;
using LMS.Business.Handlers.Admin;
using LMS.Business.Handlers.Auth;
using LMS.Business.Handlers.Employer.JobPost;
using LMS.Business.Handlers.Employer.Profile;
using LMS.Business.Handlers.Employer.SearchResume;
using LMS.Business.Handlers.Home;
using LMS.Business.Handlers.Jobseeker;
using LMS.Business.Handlers.Shared;
using LMS.Business.Handlers.TrainingPartner;
using LMS.Business.Interfaces.Admin;
using LMS.Business.Interfaces.Auth;
using LMS.Business.Interfaces.Employer.JobPost;
using LMS.Business.Interfaces.Employer.Profile;
using LMS.Business.Interfaces.Employer.SearchResume;
using LMS.Business.Interfaces.Home;
using LMS.Business.Interfaces.Jobseeker;
using LMS.Business.Interfaces.Shared;
using LMS.Business.Interfaces.TrainingPartner;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LMS.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Utility.Helpers.ConfigurationHelper.Config = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.Add
                (new ServiceDescriptor(typeof(IAuthHandler), typeof(AuthHandler), ServiceLifetime.Scoped));
            services.Add
               (new ServiceDescriptor(typeof(IJobPostHandler), typeof(JobPostHandler), ServiceLifetime.Scoped));
            services.Add
              (new ServiceDescriptor(typeof(IHomeHandler), typeof(HomeHandler), ServiceLifetime.Scoped));
            services.Add
             (new ServiceDescriptor(typeof(ISearchJobHandler), typeof(SearchJobHandler), ServiceLifetime.Scoped));
            services.Add
           (new ServiceDescriptor(typeof(IUserProfileHandler), typeof(UserProfileHandler), ServiceLifetime.Scoped));
            services.Add
              (new ServiceDescriptor(typeof(ISearchResumeHandler), typeof(SearchResumeHandler), ServiceLifetime.Scoped));
            services.Add
              (new ServiceDescriptor(typeof(IManageUsersHandler), typeof(ManageUserHandler), ServiceLifetime.Scoped));
            services.Add
            (new ServiceDescriptor(typeof(IEmpProfileHandler), typeof(EmpProfileHandler), ServiceLifetime.Scoped));
            services.Add
            (new ServiceDescriptor(typeof(IDesignationHandler), typeof(DesignationHandler), ServiceLifetime.Scoped));
            services.Add
            (new ServiceDescriptor(typeof(IJobIndustryAreaHandler), typeof(JobIndustryAreaHandler), ServiceLifetime.Scoped));
            services.Add
            (new ServiceDescriptor(typeof(IJobTitleHandler), typeof(JobTitleHandler), ServiceLifetime.Scoped));
            services.Add
            (new ServiceDescriptor(typeof(IResumeBuilderHandler), typeof(ResumeBuilderHandler), ServiceLifetime.Scoped));
            services.Add
            (new ServiceDescriptor(typeof(IFileHandler), typeof(FileHandler), ServiceLifetime.Scoped));
            services.Add
            (new ServiceDescriptor(typeof(ISuccessStoryVideoHandler), typeof(SuccessStoryVideoHandler), ServiceLifetime.Scoped));
            services.Add
            (new ServiceDescriptor(typeof(IBulkJobPostHandler), typeof(BulkJobPostHandler), ServiceLifetime.Scoped));
            services.Add
            (new ServiceDescriptor(typeof(Business.Interfaces.Admin.IDashboardHandler), typeof(Business.Handlers.Admin.DashboardHandler), ServiceLifetime.Scoped));
            services.Add
            (new ServiceDescriptor(typeof(Business.Interfaces.Employer.IDashboardHandler), typeof(Business.Handlers.Employer.DashboardHandler), ServiceLifetime.Scoped));
            services.Add
           (new ServiceDescriptor(typeof(IEMailHandler), typeof(EMailHandler), ServiceLifetime.Scoped));
            services.Add
            (new ServiceDescriptor(typeof(IUsersReviewsHandler), typeof(UsersReviewsHandler), ServiceLifetime.Scoped));
            services.Add
            (new ServiceDescriptor(typeof(IBulkJobSeekerUploadHandler), typeof(BulkJobSeekerUploadHandler), ServiceLifetime.Scoped));
            services.Add
            (new ServiceDescriptor(typeof(IManageJobsHandler), typeof(ManageJobsHandler), ServiceLifetime.Scoped));
            services.Add
            (new ServiceDescriptor(typeof(ITrainingPartnerProfileHandler), typeof(TrainingPartnerProfileHandler), ServiceLifetime.Scoped));
            services.Add
            (new ServiceDescriptor(typeof(Business.Interfaces.TrainingPartner.IDashboardHandler), typeof(Business.Handlers.TrainingPartner.DashboardHandler), ServiceLifetime.Scoped));
            services.Add
            (new ServiceDescriptor(typeof(IManageCityStateHandler), typeof(ManageCityStateHandler), ServiceLifetime.Scoped));
            services.Add
            (new ServiceDescriptor(typeof(INotificationHandler), typeof(NotificationHandler), ServiceLifetime.Scoped));
            services.Add
            (new ServiceDescriptor(typeof(IPlacedCandidateHandler), typeof(PlacedCandidateHandler), ServiceLifetime.Scoped));


            services.AddHttpContextAccessor();
            services.AddHttpClient();
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_2);
            services.AddSession(opt => opt.IdleTimeout = TimeSpan.FromMinutes(25));
            services.AddDistributedMemoryCache();//To Store session in Memory, This is default implementation of IDistributedCache 



            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
            

            //services.AddAuthentication()
            //    .AddGoogle(goptions =>
            //    {
            //        goptions.ClientId = "341141713204-ggssrf3ma50hdtgcs1jbmsrrj0e9hdgq.apps.googleusercontent.com";
            //        goptions.ClientSecret = "5DCCTVKg0Qhx_BhSHrW46f2-";
            //        goptions.SignInScheme = IdentityConstants.ExternalScheme;
            //    })
            //    .AddFacebook(foptions =>
            //    {
            //        foptions.ClientId = "725641638278516";
            //        foptions.ClientSecret = "438fc906b2bfa5dcf8fd92600aea18a7";
            //        foptions.SignInScheme = IdentityConstants.ExternalScheme;
            //    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();

            app.UseMvc(router =>
            {
                router.MapRoute(
                    name: "areaRoutes",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{Id?}"
                    );

                router.MapRoute(
                   name: "default",
                   defaults: new { Controller = "Home", Action = "Index" },
                   template: "{controller}/{action}/{id?}"
                   );
            });
        }
    }
}
