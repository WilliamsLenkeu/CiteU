// FormulaireAjoutEtudiant.xaml.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CiteU.Modele;

namespace CiteU.Vues
{
    public partial class FormulaireAjoutEtudiant : UserControl
    {
        public FormulaireAjoutEtudiant()
        {
            InitializeComponent();
        }

        private void AjouterEtudiant_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Récupérer les informations saisies par l'utilisateur
                string nom = NomTextBox.Text;
                string prenom = PrenomTextBox.Text;
                DateTime? dateNaissance = DateNaissancePicker.SelectedDate;
                string sexe = (SexeComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();
                string telephone = TelephoneTextBox.Text;
                string email = EmailTextBox.Text;
                bool handicape = HandicapeCheckBox.IsChecked ?? false;

                // Valider les données si nécessaire

                // Créer une nouvelle instance d'étudiant avec les données saisies
                EtudiantsSet nouvelEtudiant = new EtudiantsSet
                {
                    Nom = nom,
                    Niveau = "TODO: Ajouter le niveau selon vos besoins",
                    Sexe = sexe,
                    Age = 0, // TODO: Ajouter l'âge selon vos besoins
                    Handicape = handicape,
                    // Ajoutez d'autres propriétés selon vos besoins
                };

                // Ajouter l'étudiant à la base de données
                using (var context = new Model1())
                {
                    context.EtudiantsSet.Add(nouvelEtudiant);

                    // Essayer de sauvegarder les changements
                    try
                    {
                        context.SaveChanges();
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                    {
                        foreach (var entityValidationError in ex.EntityValidationErrors)
                        {
                            foreach (var validationError in entityValidationError.ValidationErrors)
                            {
                                Console.WriteLine($"Validation Error: Property - {validationError.PropertyName}, Error - {validationError.ErrorMessage}");
                            }
                        }

                        // Gérer l'erreur de validation ici (vous pouvez afficher un message à l'utilisateur, logguer les erreurs, etc.)
                        MessageBox.Show("Erreur de validation. Veuillez vérifier les données saisies.");
                        return;
                    }

                    // Attribuer une chambre et gérer la réservation et le paiement
                    AttribuerChambreReservationPaiement(context, nouvelEtudiant);
                }

                // Rediriger vers la page d'affichage des étudiants
                if (Application.Current.MainWindow is MainWindow mainWindow)
                {
                    MessageBox.Show("L'ajout a réussi ");
                    if (mainWindow.Content is Frame mainFrame && mainFrame.Content is MesEtudiants mesEtudiantsPage)
                    {
                        mainWindow.AfficherPage(new MesEtudiants());
                    }
                }
            }
            catch (Exception ex)
            {
                // Gérer l'exception générale ici (vous pouvez afficher un message à l'utilisateur, logguer l'erreur, etc.)
                MessageBox.Show($"Erreur lors de l'ajout de l'étudiant : {ex.Message}");
            }
        }

        private void AttribuerChambreReservationPaiement(Model1 context, EtudiantsSet nouvelEtudiant)
        {
            // Utilisez la méthode AttribuerChambre que nous avons définie précédemment
            ChambreSet chambreAttribuee = AttribuerChambre(context, nouvelEtudiant);

            // Créez une nouvelle réservation
            ReservationSet nouvelleReservation = new ReservationSet
            {
                Date_Debut = DateTime.Now, // Utilisez la date appropriée
                Date_Fin = DateTime.Now.AddMonths(3), // Utilisez la date appropriée
                ChambreSet = chambreAttribuee
            };

            // Ajoutez la réservation à la base de données
            context.ReservationSet.Add(nouvelleReservation);
            context.SaveChanges();

            // Créez un paiement associé à la réservation
            PaimentSet nouveauPaiement = new PaimentSet
            {
                Montant = chambreAttribuee.BatimentsSet.Prix_Chambre, // Utilisez le montant approprié
                Lieu_Paiement = "TODO: Lieu de paiement selon vos besoins",
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
                Date_Fin = DateTime.Now.AddMonths(6) // Exemple : Réservation pour 6 mois
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
                var occupants = context.EtudiantsSet.Count(e => e.PaimentSet.Any(p => p.ChambreId_Chambre == chambre.Id_Chambre));

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