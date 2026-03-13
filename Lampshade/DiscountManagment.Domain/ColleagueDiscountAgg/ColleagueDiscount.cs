using _0_Framework.Domain;

namespace DiscountManagement.Domain.ColleagueDiscountAgg;

public class ColleagueDiscount : EntityBase<long>
{
    public ColleagueDiscount(long productId, int discountRate)
    {
        ProductId = productId;
        DiscountRate = discountRate;
    }

    public long ProductId { get; private set; }
    public int DiscountRate { get; private set; }

    public void Edit(long productId, int discountRate)
    {
        ProductId = productId;
        DiscountRate = discountRate;
    }
}