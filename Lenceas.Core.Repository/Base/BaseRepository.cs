using Lenceas.Core.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace Lenceas.Core.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        #region 构造函数
        protected IUnitOfWork unitOfWork;
        private readonly MySqlContext _context;
        private readonly DbSet<T> _dbSet;
        public BaseRepository(MySqlContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _dbSet = _context.Set<T>();
            this.unitOfWork = unitOfWork;
        }
        #endregion

        #region 是否存在
        public async Task<bool> IsExist(long id)
        {
            return await this.GetById(id) != null;
        }
        public async Task<bool> IsExist(Expression<Func<T, bool>> whereLambda)
        {
            return await _dbSet.AnyAsync(whereLambda);
        }
        #endregion

        #region 分页查询
        public async Task<List<T>> GetPage(int pageIndex, int pageSize)
        {
            var entities = await this.GetList();
            return entities.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        public async Task<List<T>> GetPage(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLambda)
        {
            var entities = await this.GetList(whereLambda);
            return entities.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        #endregion

        #region 查询
        public async Task<T> GetById(long id)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<T>> GetByIds(List<long> ids)
        {
            return await _dbSet.AsNoTracking().WhereBulkContains(ids).ToListAsync();
        }

        public async Task<T> GetEntity(Expression<Func<T, bool>> whereLambda)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(whereLambda);
        }

        public async Task<List<T>> GetList()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<List<T>> GetList(Expression<Func<T, bool>> whereLambda)
        {
            return await _dbSet.Where(whereLambda).ToListAsync();
        }
        #endregion

        #region 增加
        public async Task<int> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return await unitOfWork.SaveChangesAsync();
        }
        public async Task<int> AddBulkAsync(List<T> entities)
        {
            await _dbSet.BulkInsertAsync(entities);
            return await unitOfWork.SaveChangesAsync();
        }
        #endregion

        #region 修改
        public async Task<int> UpdateAsync(Expression<Func<T, bool>> whereLambda, Expression<Func<T, T>> entity)
        {
            await _dbSet.Where(whereLambda).UpdateAsync(entity);
            return await unitOfWork.SaveChangesAsync();
        }
        public async Task<int> UpdateBulkAsync(List<T> entities)
        {
            await _dbSet.BulkUpdateAsync(entities);
            return await unitOfWork.SaveChangesAsync();
        }
        #endregion

        #region 删除
        public async Task<int> DeleteById(long id)
        {
            await _dbSet.Where(t => t.Id == id).DeleteAsync();
            await unitOfWork.SaveChangesAsync();
            return await this.IsExist(id) ? 0 : 1;
        }
        public async Task<int> DeleteByIds(List<long> ids)
        {
            var eneities = await this.GetByIds(ids);
            await this.DeletesAsync(eneities);
            await unitOfWork.SaveChangesAsync();
            eneities = await this.GetByIds(ids);
            return eneities.Count == 0 ? ids.Count : 0;
        }
        public async Task<int> DeleteAsync(T entity)
        {
            await _dbSet.DeleteAsync();
            await unitOfWork.SaveChangesAsync();
            return await this.IsExist(entity.Id) ? 0 : 1;
        }
        public async Task<int> DeletesAsync(List<T> entities)
        {
            await _dbSet.BulkDeleteAsync(entities);
            await unitOfWork.SaveChangesAsync();
            var newEntities = await this.GetByIds(entities.Select(t => t.Id).ToList());
            return newEntities.Count == 0 ? entities.Count : 0;
        }
        public async Task<int> DeleteAsync(Expression<Func<T, bool>> whereLambda)
        {
            await _dbSet.Where(whereLambda).DeleteAsync();
            await unitOfWork.SaveChangesAsync();
            var entities = await this.GetList(whereLambda);
            return entities.Count == 0 ? 1 : 0;
        }
        #endregion
    }
}
