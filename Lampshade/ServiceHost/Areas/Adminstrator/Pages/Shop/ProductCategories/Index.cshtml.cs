using _0_Framework.Application;
using _0_Framework.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopManagement.Application.Contracts.ProductCategoryAgg;
using ShopManagement.Application.Contracts.ProductCategoryAgg;
using System.Collections.Generic;

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
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => string.IsNullOrWhiteSpace(e.ErrorMessage) ? e.Exception?.Message : e.ErrorMessage)
                    .Where(m => !string.IsNullOrWhiteSpace(m))
                    .ToList();

                var message = errors.Any()
                    ? string.Join(" | ", errors)
                    : "Validation failed";

                return new JsonResult(new OperationResult().Fail(message));
            }
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
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => string.IsNullOrWhiteSpace(e.ErrorMessage) ? e.Exception?.Message : e.ErrorMessage)
                    .Where(m => !string.IsNullOrWhiteSpace(m))
                    .ToList();

                var message = errors.Any()
                    ? string.Join(" | ", errors)
                    : "Validation failed";

                return new JsonResult(new OperationResult().Fail(message));
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