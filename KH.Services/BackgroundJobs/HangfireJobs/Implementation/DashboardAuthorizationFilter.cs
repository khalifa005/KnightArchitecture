using Hangfire.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KH.Services.BackgroundJobs.HangfireJobs;

public class MyAuthorizationFilter : IDashboardAuthorizationFilter
{
  public bool Authorize(DashboardContext context)
  {
    var httpContext = context.GetHttpContext();
    return true;
    // Allow all authenticated users to see the Dashboard (potentially dangerous).
    //return httpContext.User.Identity?.IsAuthenticated ?? false;
  }
}
