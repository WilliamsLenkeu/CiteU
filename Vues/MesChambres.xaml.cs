using CiteU.Modele;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows.Controls;

namespace CiteU.Vues
{
    public partial class MesChambres : UserControl
    {
        private Model1 dbContext;
        public ObservableCollection<ChambreSet> Chambres { get; set; }

        public MesChambres()
        {
            InitializeComponent();
            Chambres = new ObservableCollection<ChambreSet>();
            listeChambres.ItemsSource = Chambres;

            dbContext = new Model1();

            var chambresQuery = dbContext.ChambreSet
                .Include("BatimentsSet")
                .Include("ReservationSet.PaimentSet.EtudiantsSet")
                .ToList();

            foreach (var chambre in chambresQuery)
            {
                Chambres.Add(chambre);
            }

            // Afficher les informations sur les étudiants
            AfficherInfosEtudiants();
        }

        private void TrierChambres()
        {
            string critereTri = (cmbTri.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (critereTri == "Niveau")
            {
                Chambres = new ObservableCollection<ChambreSet>(Chambres.OrderBy(chambre => chambre.Niveau));
            }
            else if (critereTri == "Prix")
            {
                Chambres = new ObservableCollection<ChambreSet>(Chambres.OrderByDescending(chambre => chambre.BatimentsSet.Prix_Chambre));
            }
            else if (critereTri == "Nom du Bâtiment")
            {
                Chambres = new ObservableCollection<ChambreSet>(Chambres.OrderBy(chambre => chambre.BatimentsSet.Nom_Batiment));
            }
            else if (critereTri == "Nombre de Lits par Chambre")
            {
                Chambres = new ObservableCollection<ChambreSet>(Chambres.OrderByDescending(chambre => chambre.BatimentsSet.Nombre_Lits_Par_Chambre));
            }

            listeChambres.ItemsSource = Chambres;

            // Afficher les informations sur les étudiants après le tri
            AfficherInfosEtudiants();
        }

        private void cmbTri_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TrierChambres();
        }

        private void AfficherInfosEtudiants()
        {
            foreach (var chambre in Chambres)
            {
                Console.WriteLine($"Chambre {chambre.Id_Chambre} - Niveau {chambre.Niveau}");

                if (chambre.ReservationSet.Any())
                {
                    foreach (var reservation in chambre.ReservationSet)
                    {
                        foreach (var paiement in reservation.PaimentSet)
                        {
                            var etudiant = paiement.EtudiantsSet;
                            Console.WriteLine($"- Etudiant : {etudiant.Nom}, Matricule : {etudiant.Matricule}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"- Chambre vide");
                }
            }
        }
    }
}