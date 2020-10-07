namespace WSHospital
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SetService")]
    public partial class SetService
    {
        public int ID { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        public int? IDService { get; set; }

        public virtual LabServices LabServices { get; set; }
    }
}
