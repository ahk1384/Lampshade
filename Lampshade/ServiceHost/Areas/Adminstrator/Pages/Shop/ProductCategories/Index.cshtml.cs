using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopManagement.Application.Contracts.ProductCategoryAgg;
using System.Collections.Generic;
using _0_Framework.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopManagement.Application.Contracts.ProductCategoryAgg;

namespace ServiceHost.Areas.Adminstrator.Pages.Shop.ProductCategories
{
    public class Index1Model : PageModel
    {
        public List<ProductCategoryViewModel> ProductCategories { get; set; }
        private readonly IProductCategoryApplication _productCategoryApplication;
        public ProductCategorySearchModel SearchModel;
        public static bool WatchDeleted = false;
        public bool watch = false;
        public Index1Model(IProductCategoryApplication productCategoryApplication)
        {
            _productCategoryApplication = productCategoryApplication;
        }


        public void OnGet(ProductCategorySearchModel? searchModel)
        {
            ProductCategories = _productCategoryApplication.Search(searchModel, WatchDeleted);
            watch = WatchDeleted;
        }

        
        public IActionResult OnGetCreate()
        {
            return Partial("./Create", new CreateProductCategory());
        }

        public RedirectToPageResult OnGetDeleted()
        {
            WatchDeleted = true;
            return RedirectToPage();
        }
        public RedirectToPageResult OnGetActive()
        {
            WatchDeleted = false;
            return RedirectToPage();
        }

        public JsonResult OnPostCreate(CreateProductCategory command)
        {
            var result = _productCategoryApplication.Add(command);
            return new JsonResult(result);
        }

        public IActionResult OnGetEdit(long id)
        {
            var productCategory = _productCategoryApplication.GetDetails(id);
            return Partial("Edit", productCategory);
        }

        public JsonResult OnPostEdit(EditProductCategory command)
        {
            if (ModelState.IsValid)
            {
            }

            var result = _productCategoryApplication.Edit(command);
            return new JsonResult(result);
        }

        public RedirectToPageResult OnGetRemove(long id)
        {
            _productCategoryApplication.Remove(id);
            return RedirectToPage();
        }
        public RedirectToPageResult OnGetRestore(long id)
        {
            _productCategoryApplication.Restore(id);
            return RedirectToPage("./Index",OnGetDeleted());
        }

        
    }
}