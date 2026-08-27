using PlanAhead.Core.Interfaces.Repositories;
using PlanAhead.Core.Models.Domain;
using PlanAhead.Infrastructure.Repositories;
using PlanAhead.Infrastructure.Sync.Models;
using Supabase;
using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using PlanAhead.Infrastructure.Authentication;
using System.Diagnostics;
using PlanAhead.Core.Interfaces.Services;


namespace PlanAhead.Infrastructure.DB
{
    public class AccountSynchroniser : EntitySynchroniser<Account>, IEntitySynchroniser
    {
        private readonly IAccountRepository _repository;
        private readonly ILogService _logService;

        public string EntityName => "Account";

        public AccountSynchroniser(
            Client client,
            IAccountRepository repository,
            ILogService logService)
            : base(client)
        {
            _repository = repository;
            _logService = logService;
        }

        public override async Task UploadAsync(Account account)
        {
            await UploadRecordAsync(ToRecord(account));
        }

        public override async Task UploadPendingAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var pending = await _repository.GetPendingSyncAsync();

            // Get the logged in user once
            foreach (var account in pending)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (account.UserId == Guid.Empty)
                    account.UserId = userId;

                await UploadRecordAsync(ToRecord(account));

                account.NeedsSync = false;

                await _repository.UpdateAsync(account);
            }
        }

        public override async Task DownloadChangesAsync(
            DateTime sinceUtc,
            CancellationToken cancellationToken = default)
        {
            var response = await Supabase
                .From<AccountRecord>()
                .Where(x => x.UpdatedUtc > sinceUtc)
                .Get();


            foreach (var record in response.Models)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var remote = ToDomain(record);

                await _logService.LogAsync($"Record {record.Name}");
                var local = await _repository.GetByIdAsync(remote.Id);

                if (local == null)
                {
                    await _logService.LogAsync(" -> Adding");
                    remote.NeedsSync = false;

                    await _repository.AddAsync(remote);

                    continue;
                }

                if (remote.UpdatedUtc > local.UpdatedUtc)
                {
                    await _logService.LogAsync(" -> Updating");
                    remote.NeedsSync = false;

                    await _repository.UpdateAsync(remote);
                }
            }
        }

        private static AccountRecord ToRecord(Account account)
        {
            return new AccountRecord
            {
                Id = account.Id,
                
                UserId = account.UserId,
                Name = account.Name,
                Description = account.Description,
                OpeningBalance = account.OpeningBalance,
                DisplayOrder = account.DisplayOrder,
                Notes = account.Notes,
                Archived = account.Archived,
                IconId = account.IconId,
                CreatedUtc = account.CreatedUtc,
                UpdatedUtc = account.UpdatedUtc,
                DeletedUtc = account.DeletedUtc,
                Deleted = account.Deleted
            };
        }

        private static Account ToDomain(AccountRecord record)
        {
            return new Account
            {
                Id = record.Id,
                UserId = record.UserId,
                Name = record.Name,
                Description = record.Description,
                OpeningBalance = record.OpeningBalance,
                DisplayOrder = record.DisplayOrder,
                Notes = record.Notes,
                Archived = record.Archived,
                IconId = record.IconId,
                CreatedUtc = record.CreatedUtc,
                UpdatedUtc = record.UpdatedUtc,
                DeletedUtc = record.DeletedUtc,
                Deleted = record.Deleted
            };
        }

    }




}
