using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bulky.Models.ViewModels;

public class ProductVM
{
    public Product Product { get; set; }
    public IEnumerable<SelectListItem> CategoryList { get; set; }
}
