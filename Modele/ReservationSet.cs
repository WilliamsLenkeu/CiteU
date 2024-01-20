namespace CiteU.Modele
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ReservationSet")]
    public partial class ReservationSet
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ReservationSet()
        {
            PaimentSet = new HashSet<PaimentSet>();
        }

        [Key]
        public int Id_Reservation { get; set; }

        public DateTime Date_Debut { get; set; }

        public DateTime Date_Fin { get; set; }

        public int ChambreId_Chambre { get; set; }

        public virtual ChambreSet ChambreSet { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PaimentSet> PaimentSet { get; set; }
    }
}
