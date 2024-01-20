using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace CiteU.Modele
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model12")
        {
        }

        public virtual DbSet<BatimentsSet> BatimentsSet { get; set; }
        public virtual DbSet<ChambreSet> ChambreSet { get; set; }
        public virtual DbSet<EtudiantsSet> EtudiantsSet { get; set; }
        public virtual DbSet<PaimentSet> PaimentSet { get; set; }
        public virtual DbSet<ReservationSet> ReservationSet { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BatimentsSet>()
                .HasMany(e => e.ChambreSet)
                .WithRequired(e => e.BatimentsSet)
                .HasForeignKey(e => e.BatimentsId_batiments)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ChambreSet>()
                .HasMany(e => e.ReservationSet)
                .WithRequired(e => e.ChambreSet)
                .HasForeignKey(e => e.ChambreId_Chambre)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EtudiantsSet>()
                .HasMany(e => e.PaimentSet)
                .WithRequired(e => e.EtudiantsSet)
                .HasForeignKey(e => e.Etudiants_Matricule)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReservationSet>()
                .HasMany(e => e.PaimentSet)
                .WithRequired(e => e.ReservationSet)
                .HasForeignKey(e => e.ReservationId_Reservation)
                .WillCascadeOnDelete(false);
        }
    }
}
