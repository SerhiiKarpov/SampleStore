namespace SampleStore.Services.Identity
{
    using System.Linq;

    using Microsoft.AspNetCore.Identity;

    using SampleStore.Data.Entities.Identity;

    /// <summary>
    /// Class encapsulating role store.
    /// </summary>
    /// <seealso cref="IQueryableRoleStore{Role}" />
    public partial class RoleStore : IQueryableRoleStore<Role>
    {
        #region Properties

        /// <summary>
        /// Returns an <see cref="T:System.Linq.IQueryable`1" /> collection of roles.
        /// </summary>
        /// <value>
        /// An <see cref="T:System.Linq.IQueryable`1" /> collection of roles.
        /// </value>
        public IQueryable<Role> Roles
        {
            get
            {
                return _unitOfWork.GetRepository<Role>().Query;
            }
        }

        #endregion Properties
    }
}