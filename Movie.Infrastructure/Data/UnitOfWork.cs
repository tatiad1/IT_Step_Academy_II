using Movie.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movie.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MovieDbContext _movieDbContext;

        public UnitOfWork(MovieDbContext movieDbContext)
        {
            _movieDbContext = movieDbContext;
        }

        public async Task SaveChangesAsync()
        {
            await _movieDbContext.SaveChangesAsync();
        }
    }
}
