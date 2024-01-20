namespace CiteU.Modele
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChambreSet")]
    public partial class ChambreSet
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ChambreSet()
        {
            ReservationSet = new HashSet<ReservationSet>();
        }

        [Key]
        public int Id_Chambre { get; set; }

        [Required]
        public string Niveau { get; set; }

        public int BatimentsId_batiments { get; set; }

        public virtual BatimentsSet BatimentsSet { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReservationSet> ReservationSet { get; set; }
    }
}
