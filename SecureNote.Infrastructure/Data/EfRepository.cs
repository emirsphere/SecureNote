using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SecureNote.Application.Interfaces;
using SecureNote.Domain.Entities;

namespace SecureNote.Infrastructure.Data
{
    // <T> burada User olabilir, Note olabilir.
    public class EfRepository<T> : IAsyncRepository<T> where T : BaseEntity
    {
        // Veritabanı bağlamını (Context) içeri alıyoruz.
        protected readonly SecureNoteDbContext _dbContext;

        public EfRepository(SecureNoteDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<T?> GetByIdAsync(Guid id)
        {
            // Set<T>() -> Tabloyu dinamik seçer. T User ise Users tablosuna gider.
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync(); // Transaction (İşlem) sonlandırılır ve veritabanına yazılır.
            return entity;
        }


        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}