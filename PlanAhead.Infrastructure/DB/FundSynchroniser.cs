using PlanAhead.Core.Interfaces.Repositories;
using PlanAhead.Core.Models.Domain;
using PlanAhead.Core.Models.Enums;
using PlanAhead.Infrastructure.Authentication;
using PlanAhead.Infrastructure.Repositories;
using PlanAhead.Infrastructure.Sync.Models;
using Supabase;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;


namespace PlanAhead.Infrastructure.DB
{
    public class FundSynchroniser : EntitySynchroniser<Fund>,
    IEntitySynchroniser
    {
        private readonly IFundRepository _repository;
        public string EntityName => "Fund";
        public FundSynchroniser(
            Client client,
            IFundRepository repository)
            : base(client)
        {
            _repository = repository;
        }

        public override async Task UploadAsync(Fund fund)
        {
            await UploadRecordAsync(ToRecord(fund));
        }

        public override async Task DownloadChangesAsync(
            DateTime sinceUtc,
            CancellationToken cancellationToken = default)
        {
            var response = await Supabase
                .From<FundRecord>()
                .Where(x => x.UpdatedUtc > sinceUtc)
                .Get();

            foreach (var record in response.Models)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var remote = ToDomain(record);

                var local = await _repository.GetByIdAsync(remote.Id);

                if (local == null)
                {
                    await _repository.AddAsync(remote);

                    continue;
                }

                if (remote.UpdatedUtc > local.UpdatedUtc)
                {
                    await _repository.UpdateAsync(remote);
                }
            }
        }

        public override async Task UploadPendingAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var pending = await _repository.GetPendingSyncAsync();

            foreach (var fund in pending)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (fund.UserId == Guid.Empty)
                    fund.UserId = userId;

                //
                // Deletions are just updates - we just set the delete flag and update
                // we never actually delete the record
                //
                await UploadRecordAsync(ToRecord(fund));

                fund.NeedsSync = false;

                await _repository.UpdateAsync(fund);
            }
        }


        private static FundRecord ToRecord(Fund fund)
        {
            return new FundRecord
            {
                Id = fund.Id,
                AccountId = fund.AccountId,
                Name = fund.Name,
                Description = fund.Description,
                Frequency = (int)fund.Frequency,
                Status = (int)fund.Status,
                DisplayOrder = fund.DisplayOrder,
                Notes = fund.Notes,
                IconId = fund.IconId,
                CreatedUtc = fund.CreatedUtc,
                UpdatedUtc = fund.UpdatedUtc,
                DeletedUtc = fund.DeletedUtc,
                UserId = fund.UserId
            };
        }

        private static Fund ToDomain(FundRecord record)
        {
            return new Fund
            {
                Id = record.Id,
                AccountId = record.AccountId,
                Name = record.Name,
                Description = record.Description,
                Frequency = (Frequency)record.Frequency,
                Status = (FundStatus)record.Status,
                DisplayOrder = record.DisplayOrder,
                Notes = record.Notes,
                IconId = record.IconId,
                CreatedUtc = record.CreatedUtc,
                UpdatedUtc = record.UpdatedUtc,
                DeletedUtc = record.DeletedUtc,
                UserId = record.UserId
            };
        }

    }




}
