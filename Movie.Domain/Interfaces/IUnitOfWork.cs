using System;
using System.Collections.Generic;
using System.Text;

namespace Movie.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
    }
}
