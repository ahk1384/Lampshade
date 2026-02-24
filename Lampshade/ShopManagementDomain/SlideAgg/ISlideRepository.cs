using _0_Framework.Infrastructure;
using ShopManagement.Application.Contracts.SlideAgg;

namespace ShopManagementDomain.SlideAgg;

public interface ISlideRepository : IRepository<long, Slide>
{
    EditSlide GetDetails(long id);
    List<SlideViewModel> GetList();
}