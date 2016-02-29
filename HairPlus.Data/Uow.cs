using HairPlus.Contract;
using HairPlus.EF;
using HairPlus.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairPlus.Data
{
    public class Uow : IUow
    {
        #region Class Members

        protected IRepositoryProvider _RepositoryProvider { get; set; }

        private HairPlusDBEntities _DbContext { get; set; }


        #endregion

        #region Constructor

        public Uow(IRepositoryProvider repositoryProvider)
        {
            CreateDbContext();

            repositoryProvider._DbContext = _DbContext;
            _RepositoryProvider = repositoryProvider;
        }

        #endregion

        #region Repositories

        #region Generic repositories

        public IRepository<Customer> _Customer { get { return GetStandardRepo<Customer>(); } }
        public IRepository<CustomerPhoto> _CustomerPhoto { get { return GetStandardRepo<CustomerPhoto>(); } }
        public IRepository<Expense> _Expense { get { return GetStandardRepo<Expense>(); } }
        public IRepository<HairLossSolution> _HairLossSolution { get { return GetStandardRepo<HairLossSolution>(); } }
        public IRepository<Income> _Income { get { return GetStandardRepo<Income>(); } }
        public IRepository<Invoice> _Invoice { get { return GetStandardRepo<Invoice>(); } }
        public IRepository<NonSurgicalPatient> _NonSurgicalPatient { get { return GetStandardRepo<NonSurgicalPatient>(); } }
        public IRepository<Patient> _Patient { get { return GetStandardRepo<Patient>(); } }
        public IRepository<Reminder> _Reminder { get { return GetStandardRepo<Reminder>(); } }
        public IRepository<SurgicalPatient> _SurgicalPatient { get { return GetStandardRepo<SurgicalPatient>(); } }
        public IRepository<CutomerHairLossSolution> _CutomerHairLossSolution { get { return GetStandardRepo<CutomerHairLossSolution>(); } }

        #endregion

        #region Specific Repositories

        //public IOrderRepository _Orders { get { return GetRepo<IOrderRepository>(); } }

        #endregion

        #endregion

        #region Class Methods

        private void CreateDbContext()
        {
            this._DbContext = new HairPlusDBEntities();

            // Do NOT enable proxied entities, else serialization fails
            _DbContext.Configuration.ProxyCreationEnabled = false;

            // Load navigation properties explicitly (avoid serialization trouble)
            _DbContext.Configuration.LazyLoadingEnabled = false;

            // Because We perform validation, we don't need/want EF to do so
            _DbContext.Configuration.ValidateOnSaveEnabled = false;
        }

        private IRepository<T> GetStandardRepo<T>() where T : class
        {
            return _RepositoryProvider.GetRepositoryForEntityType<T>();
        }

        private T GetRepo<T>() where T : class
        {
            return _RepositoryProvider.GetRepository<T>();
        }

        #endregion

        #region Interface Implementation

        public async Task CommitAsync()
        {
            await _DbContext.SaveChangesAsync();
        }

        #region Garbage Collector

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_DbContext != null)
                {
                    _DbContext.Dispose();
                }
            }
        }
        #endregion

        #endregion
    }
}
