[assembly: WebActivator.PostApplicationStartMethod(typeof(GeekQuiz.App_Start.SimpleInjectorInitializer), "Initialize")]

namespace GeekQuiz.App_Start
{
    using System.Web.Http;
    using SimpleInjector;
    using SimpleInjector.Integration.WebApi;
	using GeekQuiz.Controllers;
	using Microsoft.Owin.Security;
	using SimpleInjector.Advanced;
	using Microsoft.Owin;
	using System.Web;
	using System.Collections.Generic;
	using Microsoft.AspNet.Identity.Owin;
	using Microsoft.AspNet.Identity;
	using GeekQuiz.Models;
	using Microsoft.AspNet.Identity.EntityFramework;
	using SimpleInjector.Integration.Web;
	using System.Web.Mvc;
	using SimpleInjector.Integration.Web.Mvc;
	using System.Reflection;
	using SimpleInjector.Lifestyles;

	public static class SimpleInjectorInitializer
    {
        /// <summary>Initialize the container and register it as Web API Dependency Resolver.</summary>
        public static void Initialize()
        {
			var container = new Container();
			container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

			// Register your types, for instance using the scoped lifestyle:
			InitializeContainer(container);

			// This is an extension method from the integration package.
			
			container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
			container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

			container.Verify();

			DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
			GlobalConfiguration.Configuration.DependencyResolver =
				new SimpleInjectorWebApiDependencyResolver(container);
		}
     
        private static void InitializeContainer(Container container)
        {
			container.Register<ApplicationSignInManager>(Lifestyle.Scoped);
			container.Register<ApplicationUserManager>(Lifestyle.Scoped);
			
			container.Register<ApplicationDbContext>(Lifestyle.Scoped);
			container.Register<IOwinContext>( () => AdvancedExtensions.IsVerifying(container)
													? new OwinContext(new Dictionary<string, object>())
													: HttpContext.Current.GetOwinContext(), Lifestyle.Scoped);

			container.Register<IAuthenticationManager>(() => container.GetInstance<IOwinContext>().Authentication, Lifestyle.Scoped);
			container.Register<IUserStore<ApplicationUser>>(() => new UserStore<ApplicationUser>(container.GetInstance<ApplicationDbContext>()), Lifestyle.Scoped);


			container.Register<ITriviaContext, TriviaContext>(Lifestyle.Scoped);
			container.Register<TriviaQuestion>(Lifestyle.Scoped);
			container.Register<TriviaAnswer>(Lifestyle.Scoped);
		}
	}
}