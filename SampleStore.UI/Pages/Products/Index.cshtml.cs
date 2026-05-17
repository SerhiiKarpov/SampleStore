using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;

using SampleStore.Data;
using SampleStore.Data.Entities.Domain;

namespace SampleStore.UI.Pages.Products;

[AllowAnonymous]
public class IndexModel : PageModelBase
{
    private readonly IQueryMaterializer _queryMaterializer;
    private readonly IUnitOfWork _unitOfWork;

    public IndexModel(IUnitOfWork unitOfWork, IQueryMaterializer queryMaterializer)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _queryMaterializer = queryMaterializer ?? throw new ArgumentNullException(nameof(queryMaterializer));
    }

    public IList<Product> Products { get; set; } = [];

    public override string Title => "Products";

    public async Task OnGetAsync()
    {
        var productQuery = _unitOfWork.GetRepository<Product>().Query;
        Products = await _queryMaterializer.ToList(productQuery);
    }
}