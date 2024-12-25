namespace KH.BuildingBlocks.Apis.Extentions;

public static class AssemblyExtensions
{
  public static string GetCurrentVersion()
  {
    var thisApp = Assembly.GetExecutingAssembly();
    AssemblyName name = new AssemblyName(thisApp.FullName);
    var versionNumber = "v" + name.Version.Major + "." + name.Version.Minor;

    return versionNumber;
  }

  public static string GetCurrentVersion(this Assembly assembly)
  {
    var currentAssembly = assembly ?? Assembly.GetExecutingAssembly();
    AssemblyName name = new AssemblyName(currentAssembly.FullName);
    return $"v{name.Version.Major}.{name.Version.Minor}";
  }


}

