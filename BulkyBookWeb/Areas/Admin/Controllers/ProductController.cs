using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = SD.Role_Admin)]
public class ProductController(IUnitOfWork unit, IWebHostEnvironment webHost) : Controller
{
    public IActionResult Index()
    {
        var products = unit.Product.GetAll(includeProperties: "Category");

        return View(products);
    }

    public IActionResult Upsert(int? id)
    {
        IEnumerable<SelectListItem> CategoryList = unit.Category.GetAll().Select(u => new SelectListItem
        {
            Text = u.Name,
            Value = u.Id.ToString()
        });

        ProductVM productvm = new()
        {
            Product = new Product(),
            CategoryList = CategoryList,
        };

        if (id == null || id == 0)
        {
            return View(productvm);
        }
        else
        {
            productvm.Product = unit.Product.Get(u => u.Id == id);
            return View(productvm);
        }
    }

    [HttpPost]
    public IActionResult Upsert([Bind(Prefix = "Product")] Product product, IFormFile? file)
    {
        if (ModelState.IsValid)
        {
            string wwwRootPath = webHost.WebRootPath;
            //IFormFile file = product.ImageFile;
            if ( file != null)
            {
                string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string productPath = Path.Combine(wwwRootPath, @"images\product");

                if(!string.IsNullOrEmpty(product.ImageUrl))
                {
                    //delete the old image
                    var oldImagePath = Path.Combine(wwwRootPath, product.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                    using var fileStream = new FileStream(Path.Combine(productPath, filename), FileMode.Create);
                    file.CopyTo(fileStream);
                    product.ImageUrl = @$"\images\product\{filename}";

            }

            if (product.Id == 0)
            {
                unit.Product.Add(product);
                TempData["success"] = "Product created Successfully";
            }

            else
            {
                unit.Product.Update(product);
                TempData["success"] = "Product updated Successfully";

            }

            unit.Save();
            return RedirectToAction("Index");
        }
        else
        {
            if(product.Id == 0)
            {
                TempData["error"] = "Error creating product";
            }
            else
            {
                TempData["error"] = "Error updating product";
            }

            IEnumerable<SelectListItem> CategoryList = unit.Category.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

            ProductVM productvm = new()
            {
                Product = new Product(),
                CategoryList = CategoryList,
            };
            return View(productvm);

        }
    }

    //public IActionResult Edit(int? id)
    //{
    //    if (id == null || id == 0)
    //    {
    //        return NotFound();
    //    }

    //    var product = unit.Product.Get(u => u.Id == id);
    //    if (product == null)
    //    {
    //        return NotFound();
    //    }
    //    return View(product);
    //}

    //[HttpPost]
    //public IActionResult Edit(Product product)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        unit.Product.Update(product);
    //        unit.Save();
    //        TempData["success"] = "Product updated successfully";
    //        return RedirectToAction("Index");
    //    }

    //    TempData["error"] = "Error updating Product";
    //    return View();
    //}

    //public IActionResult Delete(int? id)
    //{
    //    if (id == null || id == 0)
    //    {
    //        return NotFound();
    //    }

    //    var product = unit.Product.Get(u => u.Id == id);
    //    if (product == null)
    //    {
    //        return NotFound();
    //    }

    //    return View(product);
    //}

    //[HttpPost, ActionName("Delete")]
    //public IActionResult DeletePOST(int? id)
    //{
    //    string wwwRootPath = webHost.WebRootPath;
    //    var product = unit.Product.Get(u => u.Id == id);
    //    if (product == null)
    //    {
    //        return NotFound();
    //    }
    //    if (!string.IsNullOrEmpty(product.ImageUrl))
    //    {
    //        //delete the old image
    //        var oldImagePath = Path.Combine(wwwRootPath, product.ImageUrl.TrimStart('\\'));

    //        if (System.IO.File.Exists(oldImagePath))
    //        {
    //            System.IO.File.Delete(oldImagePath);
    //        }
    //    }

    //    unit.Product.Remove(product);
    //    unit.Save();
    //    TempData["success"] = "Product deleted successfully";
    //    return RedirectToAction("Index");
    //}

    #region APICALLS
    [HttpGet]
    public IActionResult GetAll()
    {
        var products = unit.Product.GetAll(includeProperties: "Category");
        return Json(new { data = products });
    }


    [HttpDelete]
    public IActionResult Delete(int? id)
    {
        var product = unit.Product.Get(u => u.Id == id);
        if(product == null)
        {
            return Json(new { success = false, message = "Error while deleting" });
        }

        var oldImagePath = Path.Combine(webHost.WebRootPath,
            product.ImageUrl.TrimStart('\\'));

        if (System.IO.File.Exists(oldImagePath))
        {
            System.IO.File.Delete(oldImagePath);
        }

        unit.Product.Remove(product);
        unit.Save();

        return Json(new { success = true, message = "Deleted Successfully" });

    }
    #endregion
}
