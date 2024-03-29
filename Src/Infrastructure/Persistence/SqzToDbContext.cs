﻿using Domain.Common;
using Microsoft.EntityFrameworkCore;
using SqzTo.Application.Common.Interfaces;
using SqzTo.Domain.Entities;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SqzTo.Infrastructure.Persistence
{
    public class SqzToDbContext : DbContext, ISqzToDbContext
    {
        public SqzToDbContext(
            DbContextOptions<SqzToDbContext> options
            ) : base(options)
        {
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<IHasTracking>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = DateTime.UtcNow;
                        entry.Entity.CreatedBy = 0;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = DateTime.UtcNow;
                        entry.Entity.LastModifiedBy = 0;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }
    }
}
