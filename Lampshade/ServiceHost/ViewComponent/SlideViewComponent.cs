using _01_LampshadeQuery.Contracts.Slide;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.ViewComponent;

public class SlideViewComponent : Microsoft.AspNetCore.Mvc.ViewComponent
{
    private readonly ISlideQuery _slideQuery;

    public SlideViewComponent(ISlideQuery slideQuery)
    {
        _slideQuery = slideQuery;
    }

    public IViewComponentResult Invoke()
    {
        var slides = _slideQuery.GetSlides();
        return View(slides);
    }
}