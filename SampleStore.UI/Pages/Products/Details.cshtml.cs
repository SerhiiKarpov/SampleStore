using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using SampleStore.Data;
using SampleStore.Data.Entities.Domain;
using SampleStore.Data.Extensions;

namespace SampleStore.UI.Pages.Products;

public class DetailsModel : PageModelBase
{
    private readonly IQueryMaterializer _queryMaterializer;

    private readonly IUnitOfWork _unitOfWork;

    public DetailsModel(IUnitOfWork unitOfWork, IQueryMaterializer queryMaterializer)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _queryMaterializer = queryMaterializer ?? throw new ArgumentNullException(nameof(queryMaterializer));
    }

    public Product? Product { get; set; }

    public override string Title
    {
        get
        {
            return "Details";
        }
    }

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (!id.HasValue)
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
}