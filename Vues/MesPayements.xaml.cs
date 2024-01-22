using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using CiteU.Modele;

namespace CiteU.Vues
{
    public partial class MesPayements : UserControl
    {
        public ObservableCollection<PaiementInfo> Paiements { get; set; }

        public MesPayements()
        {
            InitializeComponent();
            DataContext = this;

            using (var context = new Model1())
            {
                // Chargement des paiements avec les informations associées
                Paiements = new ObservableCollection<PaiementInfo>(
                    context.PaimentSet
                        .Include("EtudiantsSet")
                        .Include("ReservationSet.ChambreSet.BatimentsSet")
                        .ToList()
                        .Select(p => new PaiementInfo
                        {
                            EtudiantNom = p.EtudiantsSet.Nom,
                            Montant = p.Montant,
                            Lieu_Paiement = p.Lieu_Paiement,
                            Date_Paiement = p.ReservationSet.Date_Debut,
                            ChambreInfo = $"Chambre {p.ReservationSet.ChambreSet.Id_Chambre}, bâtiment {p.ReservationSet.ChambreSet.BatimentsSet.Nom_Batiment}",
                            Duree = (p.ReservationSet.Date_Fin - p.ReservationSet.Date_Debut).Days
                        })
                );
            }
        }
    }

    public class PaiementInfo
    {
        public string EtudiantNom { get; set; }
        public int Montant { get; set; }
        public string Lieu_Paiement { get; set; }
        public DateTime Date_Paiement { get; set; }
        public string ChambreInfo { get; set; }
        public int Duree { get; set; }
    }
}