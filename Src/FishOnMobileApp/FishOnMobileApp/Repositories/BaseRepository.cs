using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishOn.Repositories
{
    public abstract class BaseRepository
    {
        protected IFishOnHttpRepository _httpRepository;
        private IFishOnDataContext _dataContext;

        public BaseRepository(IFishOnHttpRepository httpRepository = null, IFishOnDataContext dataContext = null)
        {
            _httpRepository = httpRepository ?? new FishOnHttpRepository();
            _dataContext = dataContext;

        }

        public async Task<IFishOnDataContext> GetDB()
        {
            if (_dataContext == null)
            {
                _dataContext = await FishOnDataContext.GetInstanceAsync();
            }

            return _dataContext;
        }
    }
}
