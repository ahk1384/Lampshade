using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.ViewComponent;

public class FooterViewComponent : Microsoft.AspNetCore.Mvc.ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}