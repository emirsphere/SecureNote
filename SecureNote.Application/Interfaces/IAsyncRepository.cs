using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecureNote.Domain.Entities;

namespace SecureNote.Application.Interfaces
{

    public interface IAsyncRepository<T> where T : BaseEntity
    {
        // ID'ye göre tek bir kayıt getir
        Task<T?> GetByIdAsync(Guid id);

        // Hepsini getir (Sadece okuma amaçlı listeler için IReadOnlyList daha hafiftir)
        Task<IReadOnlyList<T>> ListAllAsync();

        // Yeni kayıt ekle -> Geriye eklenen kaydı döner
        Task<T> AddAsync(T entity);

        // Güncelle
        Task UpdateAsync(T entity);

        // Sil
        Task DeleteAsync(T entity);
    }
}