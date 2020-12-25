using Lenceas.Core.IServices;
using Lenceas.Core.Model;
using Lenceas.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Lenceas.Core.Services
{
    public class BaseServices<T> : IBaseServices<T> where T : class, new()
    {
        #region 构造函数
        public IBaseRepository<T> BaseDal;
        public BaseServices(IBaseRepository<T> baseDal)
        {
            BaseDal = baseDal;
        }
        #endregion

        #region 是否存在
        public async Task<bool> IsExist(long id)
        {
            return await BaseDal.IsExist(id);
        }
        public async Task<bool> IsExist(Expression<Func<T, bool>> whereLambda)
        {
            return await BaseDal.IsExist(whereLambda);
        }
        #endregion

        #region 分页查询
        public async Task<List<T>> GetPage(int pageIndex, int pageSize)
        {
            return await BaseDal.GetPage(pageIndex, pageSize);
        }

        public async Task<List<T>> GetPage(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLambda)
        {
            return await BaseDal.GetPage(pageIndex, pageSize, whereLambda);
        }
        #endregion

        #region 查询
        public async Task<T> GetById(long id)
        {
            return await BaseDal.GetById(id);
        }

        public async Task<List<T>> GetByIds(List<long> ids)
        {
            return await BaseDal.GetByIds(ids);
        }

        public async Task<T> GetEntity(Expression<Func<T, bool>> whereLambda)
        {
            return await BaseDal.GetEntity(whereLambda);
        }

        public async Task<List<T>> GetList()
        {
            return await BaseDal.GetList();
        }

        public async Task<List<T>> GetList(Expression<Func<T, bool>> whereLambda)
        {
            return await BaseDal.GetList(whereLambda);
        }
        #endregion

        #region 增加
        public async Task<int> AddAsync(T entity)
        {
            return await BaseDal.AddAsync(entity);
        }
        public async Task<int> AddBulkAsync(List<T> entities)
        {
            return await BaseDal.AddBulkAsync(entities);
        }
        #endregion

        #region 修改
        public async Task<int> UpdateAsync(Expression<Func<T, bool>> whereLambda, Expression<Func<T, T>> entity)
        {
            return await BaseDal.UpdateAsync(whereLambda, entity);
        }
        public async Task<int> UpdateBulkAsync(List<T> entities)
        {
            return await BaseDal.UpdateBulkAsync(entities);
        }
        #endregion

        #region 删除
        public async Task<int> DeleteById(long id)
        {
            return await BaseDal.DeleteById(id);
        }
        public async Task<int> DeleteByIds(List<long> ids)
        {
            return await BaseDal.DeleteByIds(ids);
        }
        public async Task<int> DeleteAsync(T entity)
        {
            return await BaseDal.DeleteAsync(entity);
        }
        public async Task<int> DeletesAsync(List<T> entities)
        {
            return await BaseDal.DeletesAsync(entities);
        }
        public async Task<int> DeleteAsync(Expression<Func<T, bool>> whereLambda)
        {
            return await BaseDal.DeleteAsync(whereLambda);
        }
        #endregion
    }
}
