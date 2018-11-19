namespace SampleStore.UI.Pages.Products
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using SampleStore.Common;
    using SampleStore.Data;
    using SampleStore.Data.Entities.Domain;
    using SampleStore.Data.Extensions;

    /// <summary>
    /// Class encapsulating details model.
    /// </summary>
    /// <seealso cref="PageModel" />
    public class DetailsModel : PageModelBase
    {
        #region Fields

        /// <summary>
        /// The query materializer
        /// </summary>
        private readonly IQueryMaterializer _queryMaterializer;

        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DetailsModel"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="queryMaterializer">The query materializer.</param>
        public DetailsModel(IUnitOfWork unitOfWork, IQueryMaterializer queryMaterializer)
        {
            _unitOfWork = unitOfWork.ThrowIfArgumentIsNull(nameof(unitOfWork));
            _queryMaterializer = queryMaterializer.ThrowIfArgumentIsNull(nameof(queryMaterializer));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the product.
        /// </summary>
        public Product Product
        {
            get; set;
        }

        /// <summary>
        /// Gets the title.
        /// </summary>
        public override string Title
        {
            get
            {
                return "Details";
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Called when [get asynchronous].
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The <see cref="IActionResult"/>.</returns>
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

        #endregion Methods
    }
}