using _0_Framework.Application;
using _0_Framework.Infrastructure;
using DiscountManagement.Application.Contracts.ColleagueDiscount;
using DiscountManagement.Domain.ColleagueDiscountAgg;
using SM.Infrastructure.EFCore;

namespace DiscountManagement.Infrastructure.EFCore.Repositories;

public class ColleagueDiscountRepository : BaseRepository<long, ColleagueDiscount>, IColleagueDiscountRepository
{
    private readonly DiscountContext _discountContext;
    private readonly ShopContext _shopContext;

    public ColleagueDiscountRepository(DiscountContext discountContext, ShopContext shopContext) : base(discountContext)
    {
        _discountContext = discountContext;
        _shopContext = shopContext;
    }

    public EditColleagueDiscount GetDetails(long id)
    {
        return _discountContext.CustomerDiscounts.Select(x => new EditColleagueDiscount
        {
            Id = x.Id,
            DiscountRate = x.DiscountRate,
            ProductId = x.ProductId
        }).FirstOrDefault(x => x.Id == id);
    }

    public List<ColleagueDiscountViewModel> Search(ColleagueDiscountSearchModel searchModel, bool watchDeleted)
    {
        var res = watchDeleted
            ? _discountContext.ColleagueDiscounts.Where(x => x.IsDeleted)
            : _discountContext.ColleagueDiscounts.Where(x => !x.IsDeleted);
        var products = _shopContext.Products.Select(x => new { x.Id, x.Name });
        var query = res.Select(x => new ColleagueDiscountViewModel
        {
            Id = x.Id,
            DiscountRate = x.DiscountRate,
            ProductId = x.ProductId,
            CreationDate = x.CreationDate.ToFarsi()
        });
        if (searchModel.ProductId > 0)
            query = query.Where(x => x.ProductId.ToString().Contains(searchModel.ProductId.ToString()));

        var results = query.ToList();
        foreach (var discount in results)
            discount.Product = products.FirstOrDefault(c => c.Id == discount.ProductId)?.Name;
        if (!string.IsNullOrWhiteSpace(searchModel.Product))
            results = results.Where(x => x.Product.Contains(searchModel.Product)).ToList();

        return results.OrderByDescending(x => x.Id).ToList();
    }
}