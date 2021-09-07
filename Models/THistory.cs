using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("t_history", Schema = "logging")]
    public partial class THistory
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("tstamp")]
        public DateTime? Tstamp { get; set; }
        [Column("schemaname")]
        public string Schemaname { get; set; }
        [Column("tabname")]
        public string Tabname { get; set; }
        [Column("operation")]
        public string Operation { get; set; }
        [Column("who")]
        public string Who { get; set; }
        [Column("new_val", TypeName = "json")]
        public string NewVal { get; set; }
        [Column("old_val", TypeName = "json")]
        public string OldVal { get; set; }
    }
}
