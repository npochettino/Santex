using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.SeedWork
{
    public interface IRepository<T>
    {
        public IUnitOfWork UnitOfWork { get; }
    }
}
