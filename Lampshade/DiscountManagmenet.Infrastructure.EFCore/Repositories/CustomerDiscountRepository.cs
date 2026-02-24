using _0_Framework.Application;
using _0_Framework.Infrastructure;
using DiscountManagemenet.Infrastructure.EFCore;
using DiscountManagmenet.Application.Contracts.CustomerDiscount;
using DiscountManagment.Domain.CustomerDiscountAgg;
using SM.Infrastructure.EFCore;

namespace DiscountManagmenet.Infrastructure.EFCore.Repositories;

public class CustomerDiscountRepository : BaseRepository<long, CustomerDiscount> ,ICustomerDiscountRepository
{
    private readonly DiscountContext _context;
    private readonly ShopContext _shopContext;
    public CustomerDiscountRepository(DiscountContext context, ShopContext shopContext) : base(context)
    {
        _context = context;
        _shopContext = shopContext;
    }

    public EditCustomerDiscount GetDetails(long id)
    {
        return _context.CustomerDiscounts.Select(x => new EditCustomerDiscount()
        {
            Id = x.Id,
            DiscountRate = x.DiscountRate,
            StartDate = x.StartDate.ToFarsi(),
            EndDate = x.EndDate.ToFarsi(),
            ProductId = x.ProductId,
            Reason = x.Reason
        }).FirstOrDefault(x => x.Id == id);
    }

    public List<CustomerDiscountViewModel> Search(CustomerDiscountSearchModel searchModel, bool watchDeleted)
    {
        var res = watchDeleted
            ? _context.CustomerDiscounts.Where(x => x.IsDeleted)
            : _context.CustomerDiscounts.Where(x => !x.IsDeleted);
        var products = _shopContext.Products.Select(x => new {x.Id,x.Name});
        var query = res.Select(x => new CustomerDiscountViewModel
        {
            Id = x.Id,
            DiscountRate = x.DiscountRate,
            EndDate = x.EndDate.ToFarsi(),
            EndDateGr = x.EndDate,
            StartDate = x.StartDate.ToFarsi(),
            StartDateGr = x.StartDate,
            ProductId = x.ProductId,
            Reason = x.Reason,
            CreationDate = x.CreationDate.ToFarsi()
        });
        if (searchModel.ProductId > 0)
            query = query.Where(x => x.ProductId == searchModel.ProductId);
        if (!string.IsNullOrWhiteSpace(searchModel.StartDate))
        {
            query = query.Where(x => x.StartDateGr <= searchModel.StartDate.ToGeorgianDateTime());
        }
        if (!string.IsNullOrWhiteSpace(searchModel.EndDate))
        {
            query = query.Where(x => x.EndDateGr >= searchModel.EndDate.ToGeorgianDateTime());
        }
        if (!string.IsNullOrWhiteSpace(searchModel.Reason))
        {
            query = query.Where(x => x.Reason.Contains(searchModel.Reason));

        }

        return query.OrderByDescending(x => x.Id).ToList();
    }
}