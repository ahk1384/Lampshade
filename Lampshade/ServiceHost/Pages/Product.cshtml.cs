using System.Runtime.InteropServices;
using _01_LampshadeQuery.Contracts.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages
{
    public class ProductModel : PageModel
    {
        private readonly IProductQuery _product;

        public ProductModel(IProductQuery product)
        {
            _product = product;
        }

        public ProductQueryModel Product { get; set; }


        public void OnGet(string id)
        {
            Product = _product.GetProductDetails(id);
        }
    }
}
