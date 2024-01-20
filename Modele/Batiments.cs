namespace CiteU.Modele
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Batiments
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Batiments()
        {
            Chambres = new HashSet<Chambres>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_Batiment { get; set; }

        [StringLength(100)]
        public string Nom_Batiment { get; set; }

        [StringLength(255)]
        public string Adresse_Batiment { get; set; }

        public string Description_Batiment { get; set; }

        public int? Nombre_Etages { get; set; }

        public int? Nombre_max_chambre { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Chambres> Chambres { get; set; }
    }
}
