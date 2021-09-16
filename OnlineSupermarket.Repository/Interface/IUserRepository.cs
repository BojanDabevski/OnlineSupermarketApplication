using OnlineSupermarket.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSupermarket.Repository.Interface
{
    public interface IUserRepository
    {
        IEnumerable<ApplicationUser> GetAll();
        ApplicationUser Get(string id);
        void Insert(ApplicationUser entity);
        void Update(ApplicationUser entity);
        void Delete(ApplicationUser entity);
    }
}
