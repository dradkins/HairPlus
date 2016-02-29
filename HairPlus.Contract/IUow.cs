using HairPlus.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairPlus.Contract
{
    public interface IUow : IDisposable
    {
        #region Methods

        Task CommitAsync();

        #endregion

        #region Repositories

        IRepository<Customer> _Customer { get; }
        IRepository<CustomerPhoto> _CustomerPhoto { get; }
        IRepository<Expense> _Expense { get; }
        IRepository<HairLossSolution> _HairLossSolution { get; }
        IRepository<Income> _Income { get; }
        IRepository<Invoice> _Invoice { get; }
        IRepository<NonSurgicalPatient> _NonSurgicalPatient { get; }
        IRepository<Patient> _Patient { get; }
        IRepository<Reminder> _Reminder { get; }
        IRepository<SurgicalPatient> _SurgicalPatient { get; }
        IRepository<CutomerHairLossSolution> _CutomerHairLossSolution { get; }

        #endregion
    }
}
