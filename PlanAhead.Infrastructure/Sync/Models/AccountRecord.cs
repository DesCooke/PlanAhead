using System;
using System.Collections.Generic;
using System.Text;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace PlanAhead.Infrastructure.Sync.Models
{
    [Table("accounts")]
    public class AccountRecord : BaseModel
    {
        [PrimaryKey("id")]
        public Guid Id { get; set; }

        [Column("name")]
        public string Name { get; set; } = "";

        [Column("description")]
        public string Description { get; set; } = "";

        [Column("opening_balance")]
        public decimal OpeningBalance { get; set; }

        [Column("display_order")]
        public int DisplayOrder { get; set; }

        [Column("notes")]
        public string Notes { get; set; } = "";

        [Column("archived")]
        public bool Archived { get; set; }

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
    }
}