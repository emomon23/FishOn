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

        public BaseRepository(IFishOnHttpRepository httpRepository = null)
        {
            _httpRepository = httpRepository == null ? new FishOnHttpRepository() : httpRepository;
        }
    }
}
