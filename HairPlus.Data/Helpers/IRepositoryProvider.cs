using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HairPlus.Contract;
using System.Data.Entity;

namespace HairPlus.Data.Helpers
{
    public interface IRepositoryProvider
    {
        DbContext _DbContext { get; set; }
        IRepository<T> GetRepositoryForEntityType<T>() where T : class;
        T GetRepository<T>(Func<DbContext, object> factory = null) where T : class;
        void SetRepository<T>(T repository);
    }
}
