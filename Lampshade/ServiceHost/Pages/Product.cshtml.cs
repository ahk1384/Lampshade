using _01_LampshadeQuery.Contracts.Product;
using CommentManagement.Application.Contracts.Comment;
using CommnetManagement.Infrastructure.EFCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Runtime.InteropServices;

namespace ServiceHost.Pages
{
    public class ProductModel : PageModel
    {
        private readonly ICommentApplication _commentApplication;
        private readonly IProductQuery _productQuery;
        public ProductQueryModel Product;

        public ProductModel(IProductQuery product, IProductQuery productQuery, ICommentApplication commentApplication)
        {
            _productQuery = productQuery;
            _commentApplication = commentApplication;
            _productQuery = product;
        }



        public void OnGet(string id)
        {
            Product = _productQuery.GetProductDetails(id);
        }
        public IActionResult OnPost(AddComment command, string productSlug)
        {
            command.Type = CommentType.Product;
            var result = _commentApplication.Add(command);
            return RedirectToPage("/Product", new { Id = productSlug });
        }
    }
}
