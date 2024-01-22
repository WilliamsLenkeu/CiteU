// FormulaireAjoutEtudiant.xaml.cs

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using CiteU.Modele;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace CiteU.Vues
{
    public partial class FormulaireAjoutEtudiant : UserControl
    {
        public FormulaireAjoutEtudiant()
        {
            InitializeComponent();
        }

        public List<EtudiantsSet> etudiantsFromJson;

        private void ImporterEtudiants_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Fichiers JSON (*.json)|*.json|Tous les fichiers (*.*)|*.*",
                    Title = "Sélectionnez un fichier JSON"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    string jsonContent = File.ReadAllText(openFileDialog.FileName);

                    etudiantsFromJson = JsonConvert.DeserializeObject<List<EtudiantsSet>>(jsonContent);

                    // Afficher un message de succès
                    MessageBox.Show("Importation réussie. Les étudiants seront ajoutés lors de la prochaine opération.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'importation des étudiants : {ex.Message}");
            }
        }

        private void AjouterEtudiant_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new Model1())
                {
                    // Ajouter les étudiants à la base de données
                    foreach (var nouvelEtudiant in etudiantsFromJson)
                    {
                        context.EtudiantsSet.Add(nouvelEtudiant);

                        // Attribuer une chambre et gérer la réservation et le paiement
                        AttribuerChambreReservationPaiement(context, nouvelEtudiant);
                    }

                    // Essayer de sauvegarder les changements
                    context.SaveChanges();

                    // Afficher un message de succès
                    MessageBox.Show("Ajout des étudiants réussi");

                    // Rediriger vers la page d'affichage des étudiants
                    if (Application.Current.MainWindow is MainWindow mainWindow)
                    {
                        if (mainWindow.Content is Frame mainFrame && mainFrame.Content is MesEtudiants mesEtudiantsPage)
                        {
                            mainWindow.AfficherPage(new MesEtudiants());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ajout des étudiants : {ex.Message}");
            }
        }

        private void AttribuerChambreReservationPaiement(Model1 context, EtudiantsSet nouvelEtudiant)
        {
            // Utilisez la méthode AttribuerChambre que nous avons définie précédemment
            ChambreSet chambreAttribuee = AttribuerChambre(context, nouvelEtudiant);

            // Créez une nouvelle réservation
            ReservationSet nouvelleReservation = new ReservationSet
            {
                Date_Debut = DateTime.Now,
                Date_Fin = DateTime.Now.AddMonths(3),
                ChambreId_Chambre = chambreAttribuee.Id_Chambre
            };

            // Ajoutez la réservation à la base de données
            context.ReservationSet.Add(nouvelleReservation);
            context.SaveChanges();

            // Créez un paiement associé à la réservation
            PaimentSet nouveauPaiement = new PaimentSet
            {
                Montant = chambreAttribuee.BatimentsSet.Prix_Chambre,
                Lieu_Paiement = "Societe Generale",
                ChambreId_Chambre = chambreAttribuee.Id_Chambre,
                ReservationId_Reservation = nouvelleReservation.Id_Reservation,
                Etudiants_Matricule = nouvelEtudiant.Matricule
            };

            // Ajoutez le paiement à la base de données
            context.PaimentSet.Add(nouveauPaiement);
            context.SaveChanges();
        }

        private ChambreSet AttribuerChambre(Model1 context, EtudiantsSet nouvelEtudiant)
        {
            // Créer une nouvelle réservation
            ReservationSet nouvelleReservation = new ReservationSet
            {
                Date_Debut = DateTime.Now,
                Date_Fin = DateTime.Now.AddMonths(6)
            };

            // Attribuer une chambre à la nouvelle réservation
            nouvelleReservation.ChambreSet = TrouverChambreDisponible(context, nouvelEtudiant);

            // Ajouter la réservation à la base de données
            context.ReservationSet.Add(nouvelleReservation);
            context.SaveChanges();

            // Retourner la chambre attribuée
            return nouvelleReservation.ChambreSet;
        }

        private ChambreSet TrouverChambreDisponible(Model1 context, EtudiantsSet nouvelEtudiant)
        {
            ChambreSet chambreAttribuee = null;

            // Recherche des chambres disponibles
            var chambresDisponibles = context.ChambreSet
                .Include("BatimentsSet")
                .Where(c => c.BatimentsSet.Nombre_etage > 0 && c.BatimentsSet.Nombre_Lits_Par_Chambre > 0)
                .OrderBy(c => c.BatimentsSet.Nombre_etage)
                .ToList();

            // Parcourir les chambres disponibles pour attribuer au nouvel étudiant
            foreach (var chambre in chambresDisponibles)
            {
                // Vérifier si la capacité restante de la chambre permet d'ajouter le nouvel étudiant
                var occupants = context.PaimentSet.Count(p => p.ChambreId_Chambre == chambre.Id_Chambre);

                if (occupants < chambre.BatimentsSet.Nombre_Lits_Par_Chambre)
                {
                    // Trouver le nombre d'étages du bâtiment
                    int nombreEtagesDuBatiment = chambre.BatimentsSet.Nombre_etage;

                    // Vérifier si l'étudiant peut être attribué à l'étage approprié
                    if ((nouvelEtudiant.Handicape && nombreEtagesDuBatiment >= 1) ||
                        (nouvelEtudiant.Sexe == "F" && nombreEtagesDuBatiment >= 2) ||
                        (nouvelEtudiant.Sexe == "M" && nombreEtagesDuBatiment >= 3))
                    {
                        chambreAttribuee = chambre;
                        break;
                    }
                }
            }

            // Si aucune chambre n'est trouvée, afficher un message et gérer la suppression de la réservation et du paiement
            if (chambreAttribuee == null)
            {
                MessageBox.Show("Aucune chambre disponible pour l'étudiant.");
                var reservationsToDelete = context.ReservationSet.Where(r => r.ChambreSet == null).ToList();
                var paiementsToDelete = context.PaimentSet.Where(p => p.ChambreId_Chambre == null).ToList();

                context.ReservationSet.RemoveRange(reservationsToDelete);
                context.PaimentSet.RemoveRange(paiementsToDelete);
                context.SaveChanges();

                throw new Exception("Aucune chambre disponible pour l'étudiant.");
            }

            return chambreAttribuee;
        }
    }
}