using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SampleStore.Data;
using SampleStore.Services.Identity.Constants;
using SampleStore.Data.Entities.Domain;
using SampleStore.Data.Extensions;

namespace SampleStore.UI.Pages.Products;

[Authorize(Roles = Roles.Admin)]
public class DeleteModel : PageModelBase
{
    private readonly IQueryMaterializer _queryMaterializer;

    private readonly IUnitOfWork _unitOfWork;

    public DeleteModel(IUnitOfWork unitOfWork, IQueryMaterializer queryMaterializer)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _queryMaterializer = queryMaterializer ?? throw new ArgumentNullException(nameof(queryMaterializer));
    }

    [BindProperty]
    public Product? Product { get; set; }

    public override string Title => "Delete";

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var productQuery = _unitOfWork.GetRepository<Product>().Query.Where(m => m.Id == id);
        Product = await _queryMaterializer.FirstOrDefault(productQuery);

        if (Product == null)
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        if (!id.HasValue)
        {
            return NotFound();
        }

        var repository = _unitOfWork.GetRepository<Product>();
        Product = await repository.FindById(id.Value, _queryMaterializer);

        if (Product != null)
        {
            repository.Remove(Product);
            await _unitOfWork.SaveChanges();
        }

        return RedirectToPage("./Index");
    }
}