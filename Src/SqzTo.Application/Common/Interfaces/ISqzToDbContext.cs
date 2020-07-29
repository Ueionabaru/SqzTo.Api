﻿using Microsoft.EntityFrameworkCore;
using SqzTo.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace SqzTo.Application.Common.Interfaces
{
    public interface ISqzToDbContext
    {
        DbSet<SqzLink> ShortUrls { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
