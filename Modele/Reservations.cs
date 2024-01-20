namespace CiteU.Modele
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Reservations
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_Reservation { get; set; }

        public int? ID_Etudiant { get; set; }

        public int? ID_Chambre { get; set; }

        public DateTime? Date_Debut { get; set; }

        public DateTime? Date_Fin { get; set; }

        [StringLength(20)]
        public string Statut_Paiement { get; set; }

        public virtual Chambres Chambres { get; set; }

        public virtual Etudiants Etudiants { get; set; }
    }
}
