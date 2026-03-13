using _0_Framework.Application;
using _0_Framework.Infrastructure;
using ShopManagement.Application.Contracts.SlideAgg;
using ShopManagementDomain.SlideAgg;
using SM.Infrastructure.EFCore;

namespace ShopManagement.Infrastructure.EFCore.Repository;

public class SlideRepository : BaseRepository<long, Slide>, ISlideRepository
{
    private readonly ShopContext _context;

    public SlideRepository(ShopContext context) : base(context)
    {
        _context = context;
    }

    public EditSlide GetDetails(long id)
    {
        return _context.Slides.Select(x => new EditSlide
        {
            Id = x.Id,
            BtnText = x.BtnText,
            Heading = x.Heading,
            PictureAlt = x.PictureAlt,
            PictureTitle = x.PictureTitle,
            Text = x.Text,
            Link = x.Link,
            Title = x.Title
        }).FirstOrDefault(x => x.Id == id);
    }

    public List<SlideViewModel> GetList()
    {
        return _context.Slides.Select(x => new SlideViewModel
        {
            Id = x.Id,
            Heading = x.Heading,
            Picture = x.Picture,
            Title = x.Title,
            CreationDate = x.CreationDate.ToFarsi(),
            IsDeleted = x.IsDeleted
        }).OrderByDescending(x => x.Id).ToList();
    }
}