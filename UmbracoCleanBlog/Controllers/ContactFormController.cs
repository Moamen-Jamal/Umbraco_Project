using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;
using UmbracoCleanBlog.Models;
using UmbracoCleanBlog.Services;

namespace UmbracoCleanBlog.Controllers
{
    public class ContactFormController : SurfaceController
    {
        private readonly EmailService _EmailService;

        public ContactFormController(EmailService emailService,
            IUmbracoContextAccessor umbracoContextAccessor,
            IUmbracoDatabaseFactory databaseFactory,
            ServiceContext services,
            AppCaches appCaches,
            IProfilingLogger profilingLogger,
            IPublishedUrlProvider publishedUrlProvider)
            : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        { 
            _EmailService = emailService;
        }

        [HttpPost]
        public IActionResult Submit(ContactFormModel model)
        {
            if (!ModelState.IsValid)
            {
                //return View("ContactForm",model);
                return CurrentUmbracoPage();
            }

            // Work with form data here
            _EmailService.Send(model, CurrentPage.Id);
            TempData["ContactResult"] = "Has been sent....";
            return RedirectToCurrentUmbracoPage();
        }
    }
}
