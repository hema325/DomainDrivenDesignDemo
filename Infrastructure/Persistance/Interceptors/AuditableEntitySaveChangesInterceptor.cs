﻿using Application.Common.Interfaces.Identity;
using Application.Common.Interfaces.Services;
using Domain.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Interceptors
{
    internal class AuditableEntitySaveChangesInterceptor: SaveChangesInterceptor
    {
        private readonly ICurrentUser _user;
        private readonly IDateTime _dateTime;

        public AuditableEntitySaveChangesInterceptor(ICurrentUser user, IDateTime dateTime)
        {
            _user = user;
            _dateTime = dateTime;
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            var changeTracker = eventData.Context.ChangeTracker;

            MangeAddedEntities(changeTracker);
            MangeModifiedEntities(changeTracker);
            MangeDeletedEntities(changeTracker);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void MangeAddedEntities(ChangeTracker changeTracker)
        {
            var addedEntries = changeTracker.Entries<IAuditableEntity>().Where(e => e.State == EntityState.Added);

            foreach (var entry in addedEntries)
            {
                entry.Entity.CreatedOn = _dateTime.Now;
                entry.Entity.CreatedBy = _user.Id;
                entry.Entity.ModifiedOn = _dateTime.Now;
                entry.Entity.ModifiedBy = _user.Id;
            }
        }

        private void MangeModifiedEntities(ChangeTracker changeTracker)
        {
            var modifiedEntries = changeTracker.Entries<IAuditableEntity>().Where(e => e.State == EntityState.Modified);

            foreach (var entry in modifiedEntries)
            {
                entry.Entity.ModifiedOn = _dateTime.Now;
                entry.Entity.ModifiedBy = _user.Id;
            }
        }

        private void MangeDeletedEntities(ChangeTracker changeTracker)
        {
            var modifiedEntries = changeTracker.Entries<ISoftDeletableEntity>().Where(e => e.State == EntityState.Deleted);

            foreach (var entry in modifiedEntries)
            {
                entry.Entity.DeletedOn = _dateTime.Now;
                entry.Entity.DeletedBy = _user.Id;
                entry.State = EntityState.Modified;
            }
        }
    }
}
