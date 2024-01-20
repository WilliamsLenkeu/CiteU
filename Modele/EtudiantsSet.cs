namespace CiteU.Modele
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EtudiantsSet")]
    public partial class EtudiantsSet
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EtudiantsSet()
        {
            PaimentSet = new HashSet<PaimentSet>();
        }

        [Key]
        [StringLength(50)]
        public string Matricule { get; set; }

        [Required]
        public string Nom { get; set; }

        [Required]
        [StringLength(1)]
        public string Sexe { get; set; }

        [Required]
        public string Niveau { get; set; }

        public bool Handicape { get; set; }

        public int Age { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PaimentSet> PaimentSet { get; set; }
    }
}
