namespace SampleStore.Identity.Services
{
    using System.Linq;

    using Microsoft.AspNetCore.Identity;

    using SampleStore.Data.Entities.Identity;

    /// <summary>
    /// Class encapsulating user store.
    /// </summary>
    /// <seealso cref="IQueryableUserStore{User}" />
    public partial class UserStore : IQueryableUserStore<User>
    {
        #region Properties

        /// <summary>
        /// Returns an <see cref="T:System.Linq.IQueryable`1" /> collection of users.
        /// </summary>
        /// <value>
        /// An <see cref="T:System.Linq.IQueryable`1" /> collection of users.
        /// </value>
        public IQueryable<User> Users
        {
            get
            {
                return _unitOfWork.GetRepository<User>().Query;
            }
        }

        #endregion Properties
    }
}