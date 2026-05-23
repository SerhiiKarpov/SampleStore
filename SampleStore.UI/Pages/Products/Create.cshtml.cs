using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using SampleStore.Common.Extensions;
using SampleStore.Data;
using SampleStore.Data.Entities.Domain;
using SampleStore.Data.Extensions;

namespace SampleStore.UI.Pages.Products;

public class CreateModel : PageModelBase
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork)) ;
    }

    [BindProperty]
    public Product? Product { get; set; }

    public override string Title => "Create";

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _unitOfWork.GetRepository<Product>().Add(Product!);
        await _unitOfWork.SaveChanges();

        return RedirectToPage("./Index");
    }
}