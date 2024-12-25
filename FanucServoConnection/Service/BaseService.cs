using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanucServoConnection.Service
{
    /// <summary>
    /// 数据库连接基类
    /// </summary>
    public class BaseService<T> where T : class,new()
    {
        /// <summary>
        /// 数据库操作对象
        /// </summary>
        public static IFreeSql Fsql {  get; set; }

        static BaseService()
        {
            Fsql=new FreeSqlBuilder()
                .UseConnectionString(DataType.Sqlite, @"Data Source=data\fanuc.db; Pooling=true;Min Pool Size=1")
                .UseAutoSyncStructure(true)
                .Build();
        }

        public BaseService()
        {
            Selector = Fsql.Select<T>();
        }
        /// <summary>
        /// 查询器
        /// </summary>
        public ISelect<T> Selector { get; set; }

        public int Update(T entity)
        {
            var rows = Fsql.Update<T>().SetSource(entity).ExecuteAffrows();
            return rows;
        }
        public void UpdateAsync(T entity)
        {
            Fsql.Update<T>().SetSource(entity).ExecuteAffrowsAsync();
        }

        public virtual int Update(List<T> entities)
        {
            var rows = Fsql.Update<T>().SetSource(entities).ExecuteAffrows();
            return rows;
        }
        public void UpdateAsync(List<T> entities)
        {
            Fsql.Update<T>().SetSource(entities).ExecuteAffrowsAsync();
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entities">数据项</param>
        /// <returns>返回插入后的数据项,带主键</returns>
        public virtual List<T> Insert(List<T> entities)
        {
            var inserted = Fsql.GetRepository<T>().Insert(entities);
            return inserted;
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entities">数据项</param>
        /// <returns>返回插入后的数据项,带主键</returns>
        public virtual void InsertAsync(List<T> entities)
        {
            Fsql.GetRepository<T>().InsertAsync(entities);
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据项</param>
        /// <returns>返回插入后的数据项,带主键</returns>
        public virtual T Insert(T entity)
        {
            var inserted = Fsql.GetRepository<T>().Insert(entity);
            return inserted;
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据项</param>
        /// <returns>返回插入后的数据项,带主键</returns>
        public virtual void InsertAsync(T entity)
        {
            var inserted = Fsql.GetRepository<T>().InsertAsync(entity);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="entity">数据项</param>
        /// <returns>返回删除的数据项数</returns>
        public virtual int Delete(T entity)
        {
            var rows = Fsql.GetRepository<T>().Delete(entity);
            return rows;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="entity">数据项</param>
        /// <returns>返回删除的数据项数</returns>
        public virtual void DeleteAsync(T entity)
        {
            var rows = Fsql.GetRepository<T>().DeleteAsync(entity);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="entities">数据项</param>
        /// <returns>返回删除的数据项数</returns>
        public virtual void Delete(List<T> entities)
        {
            var rows = Fsql.GetRepository<T>().DeleteAsync(entities);
        }


    }
}
