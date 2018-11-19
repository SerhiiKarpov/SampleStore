namespace SampleStore.UI.Pages.Products
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using SampleStore.Common;
    using SampleStore.Data;
    using SampleStore.Data.Entities.Domain;

    /// <summary>
    /// Class encapsulating index model.
    /// </summary>
    /// <seealso cref="PageModel" />
    [AllowAnonymous]
    public class IndexModel : PageModelBase
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
        /// Initializes a new instance of the <see cref="IndexModel"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="queryMaterializer">The query materializer.</param>
        public IndexModel(IUnitOfWork unitOfWork, IQueryMaterializer queryMaterializer)
        {
            _unitOfWork = unitOfWork.ThrowIfArgumentIsNull(nameof(unitOfWork));
            _queryMaterializer = queryMaterializer.ThrowIfArgumentIsNull(nameof(queryMaterializer));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the products.
        /// </summary>
        public IList<Product> Products
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
                return "Products";
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Called when [get asynchronous].
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task OnGetAsync()
        {
            var productQuery = _unitOfWork.GetRepository<Product>().Query;
            Products = await _queryMaterializer.ToList(productQuery);
        }

        #endregion Methods
    }
}