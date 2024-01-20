namespace CiteU.Modele
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PaimentSet")]
    public partial class PaimentSet
    {
        [Key]
        public int Id_Paiement { get; set; }

        public int Montant { get; set; }

        [Required]
        public string Lieu_Paiement { get; set; }

        public int ChambreId_Chambre { get; set; }

        public int ReservationId_Reservation { get; set; }

        [Required]
        [StringLength(50)]
        public string Etudiants_Matricule { get; set; }

        public virtual EtudiantsSet EtudiantsSet { get; set; }

        public virtual ReservationSet ReservationSet { get; set; }
    }
}
