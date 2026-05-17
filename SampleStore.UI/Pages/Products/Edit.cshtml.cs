using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using SampleStore.Common.Extensions;
using SampleStore.Data;
using SampleStore.Data.Entities.Domain;
using SampleStore.Data.Extensions;

namespace SampleStore.UI.Pages.Products;

public class EditModel : PageModelBase
{
    private readonly IQueryMaterializer _queryMaterializer;

    private readonly IUnitOfWork _unitOfWork;

    public EditModel(IUnitOfWork unitOfWork, IQueryMaterializer queryMaterializer)
    {
        _unitOfWork = unitOfWork.ThrowIfArgumentIsNull(nameof(unitOfWork));
        _queryMaterializer = queryMaterializer.ThrowIfArgumentIsNull(nameof(queryMaterializer));
    }

    [BindProperty]
    public Product? Product { get; set; }

    public override string Title
    {
        get
        {
            return "Edit";
        }
    }

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Product = await _unitOfWork.GetRepository<Product>().FindById(id.Value, _queryMaterializer);

        if (Product == null)
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        await _unitOfWork.GetRepository<Product>().Update(Product!, _queryMaterializer);

        try
        {
            await _unitOfWork.SaveChanges();
        }
        catch (ConcurrencyException)
        {
            if (!await ProductExists(Product!.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToPage("./Index");
    }

    private async Task<bool> ProductExists(Guid id)
    {
        var productQuery = _unitOfWork.GetRepository<Product>().Query.Where(p => p.Id == id);
        return await _queryMaterializer.Any(productQuery);
    }
}