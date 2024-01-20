namespace CiteU.Modele
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Paiements
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_Paiement { get; set; }

        public int? ID_Etudiant { get; set; }

        public decimal? Montant { get; set; }

        public DateTime? Date_Paiement { get; set; }

        [StringLength(50)]
        public string Methode_Paiement { get; set; }

        public virtual Etudiants Etudiants { get; set; }
    }
}
