using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.FileProviders;

namespace KH.BuildingBlocks.Apis.Services;

public interface IRazorRendererService
{
  string RenderViewToString<TModel>(string viewName, TModel model);
}


public class RazorRendererService : IRazorRendererService
{
  private readonly IServiceProvider _serviceProvider;
  private readonly ICompositeViewEngine _viewEngine;

  public RazorRendererService(ICompositeViewEngine viewEngine, IServiceProvider serviceProvider)
  {
    _viewEngine = viewEngine;
    _serviceProvider = serviceProvider;
  }

  public string RenderViewToString<TModel>(string viewName, TModel model)
  {
    using var stringWriter = new StringWriter();

    var viewResult = _viewEngine.FindView(null, viewName, false);
    if (!viewResult.Success)
    {
      throw new FileNotFoundException($"View {viewName} not found.");
    }

    var viewContext = new ViewContext
    {
      View = viewResult.View,
      ViewData = new ViewDataDictionary<TModel>(
            new EmptyModelMetadataProvider(),
            new ModelStateDictionary())
      {
        Model = model
      },
      Writer = stringWriter,
      HttpContext = new DefaultHttpContext { RequestServices = _serviceProvider }
    };

    viewResult.View.RenderAsync(viewContext).Wait();
    return stringWriter.ToString();
  }


}
