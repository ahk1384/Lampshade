namespace ShopManagement.Application.Contracts.SlideAgg;

public class SlideViewModel
{
    public long Id { get; set; }
    public string Picture { get; set; }
    public string Heading { get; set; }
    public string Title { get; set; }
    public bool IsDeleted { get; set; }
    public string CreationDate { get; set; }
}