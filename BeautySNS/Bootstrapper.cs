using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc4;
using BeautySNS.Domain.DAO.Interfaces;
using BeautySNS.Domain.DAO;
using BeautySNS.Domain.Services.Interfaces;
using BeautySNS.Domain.Services;
using BeautySNS.Domain.Code;
using BeautySNS.Domain.Code.Interfaces;

namespace BeautySNS
{
  public static class Bootstrapper
  {
    public static IUnityContainer Initialise()
    {
      var container = BuildUnityContainer();

      DependencyResolver.SetResolver(new UnityDependencyResolver(container));

      return container;
    }

    private static IUnityContainer BuildUnityContainer()
    {
      var container = new UnityContainer();

      // register all your components with the container here
      // it is NOT necessary to register your controllers

      // e.g. container.RegisterType<ITestService, TestService>();    
      container.RegisterType<IAccountDAO, AccountDAO>();
      container.RegisterType<IAccountPermissionDAO, AccountPermissionDAO>();
      container.RegisterType<IJobDAO, JobDAO>();
      container.RegisterType<IFriendDAO, FriendDAO>();
      container.RegisterType<IFriendInvitationDAO, FriendInvitationDAO>();
      container.RegisterType<IProfileDAO, ProfileDAO>();
      container.RegisterType<IAccountService, AccountService>();
      container.RegisterType<ISessionWrapper, SessionWrapper>();
      container.RegisterType<IEmail, Email>();
      container.RegisterType<IAccountService, AccountService>();
      container.RegisterType<IConfiguration, Configuration>();
      container.RegisterType<IUserSession, UserSession>();
      container.RegisterType<IAlertService, AlertService>();
      container.RegisterType<IAlertDAO, AlertDAO>();
      container.RegisterType<IStatusUpdateDAO, StatusUpdateDAO>();

      RegisterTypes(container);

      return container;
    }

    public static void RegisterTypes(IUnityContainer container)
    {
    
    }
  }
}