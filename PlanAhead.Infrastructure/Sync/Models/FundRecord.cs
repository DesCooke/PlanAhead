using System;
using System.Collections.Generic;
using System.Text;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace PlanAhead.Infrastructure.Sync.Models
{
    [Table("funds")]
    public class FundRecord : BaseModel
    {
        [PrimaryKey("id")]
        public Guid Id { get; set; }

        [Column("account_id")]
        public Guid AccountId { get; set; }

        [Column("name")]
        public string Name { get; set; } = "";

        [Column("description")]
        public string Description { get; set; } = "";

        [Column("frequency")]
        public int Frequency { get; set; }

        [Column("status")]
        public int Status { get; set; }

        [Column("display_order")]
        public int DisplayOrder{ get; set; }

        [Column("notes")]
        public string Notes { get; set; } = "";

        [Column("icon_id")]
        public string IconId { get; set; } = "";

        [Column("created_utc")]
        public DateTime CreatedUtc { get; set; }

        [Column("updated_utc")]
        public DateTime UpdatedUtc { get; set; }

        [Column("deleted_utc")]
        public DateTime DeletedUtc { get; set; }

        [Column("user_id")]
        public Guid UserId { get; set; }

        [Column("deleted")]
        public bool Deleted { get; set; }

    }
}
