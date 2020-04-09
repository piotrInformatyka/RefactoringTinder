using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tinder.API.Data
{
    public class GenericRepository : IGenericRepository
    {
        private readonly DataContext _contex;
        public GenericRepository(DataContext contex)
        {
            _contex = contex;
        }
        public void Add<T>(T entity) where T : class
        {
            _contex.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _contex.Remove(entity);
        }

        public async Task<bool> SaveAll()
        {
            return await _contex.SaveChangesAsync() > 0;
        }
    }
}
