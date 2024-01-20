namespace CiteU.Modele
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BatimentsSet")]
    public partial class BatimentsSet
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BatimentsSet()
        {
            ChambreSet = new HashSet<ChambreSet>();
        }

        [Key]
        public int Id_batiments { get; set; }

        [Required]
        public string Nom_Batiment { get; set; }

        public int Nombre_etage { get; set; }

        public int Nombre_Chambre_Par_Etage { get; set; }

        public int Nombre_Lits_Par_Chambre { get; set; }

        public int Prix_Chambre { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChambreSet> ChambreSet { get; set; }
    }
}
