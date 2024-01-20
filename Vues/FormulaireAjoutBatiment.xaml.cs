using System;
using System.Windows;
using System.Windows.Controls;
using CiteU.Modele;
using System.Collections.Generic;

namespace CiteU.Vues
{
    public partial class FormulaireAjoutBatiment : UserControl
    {
        private Model1 dbContext; // Assurez-vous d'avoir une instance du contexte de base de données

        public FormulaireAjoutBatiment()
        {
            InitializeComponent();
            dbContext = new Model1(); // Initialisation du contexte de base de données
        }

        private async void AjouterBatiment_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer les valeurs du formulaire
            string nomBatiment = NomBatimentTextBox.Text;

            // Assurez-vous de gérer les conversions de manière appropriée pour les champs numériques
            int nombreEtages = Convert.ToInt32(NombreEtagesTextBox.Text);
            int nombreChambresParEtage = Convert.ToInt32(NombreChambresParEtageTextBox.Text);
            int nombreLitsParChambre = Convert.ToInt32(NombreLitsParChambreTextBox.Text);
            int prixChambre = Convert.ToInt32(PrixChambreTextBox.Text);

            // Création d'une nouvelle instance de Batiment avec les nouveaux champs
            BatimentsSet nouveauBatiment = new BatimentsSet
            {
                Nom_Batiment = nomBatiment,
                Nombre_etage = nombreEtages + 1,
                Nombre_Chambre_Par_Etage = nombreChambresParEtage,
                Nombre_Lits_Par_Chambre = nombreLitsParChambre,
                Prix_Chambre = prixChambre,
            };

            // Ajout du nouveau bâtiment à la base de données
            dbContext.BatimentsSet.Add(nouveauBatiment);

            try
            {
                // Enregistrement des modifications dans la base de données de manière asynchrone
                await dbContext.SaveChangesAsync();

                // Génération des chambres pour chaque étage
                for (int etage = 1; etage <= nouveauBatiment.Nombre_etage; etage++)
                {
                    for (int chambre = 1; chambre <= nouveauBatiment.Nombre_Chambre_Par_Etage; chambre++)
                    {
                        // Création d'une nouvelle instance de Chambre
                        ChambreSet nouvelleChambre = new ChambreSet
                        {
                            Niveau = $"Étage {etage}",
                            BatimentsId_batiments = nouveauBatiment.Id_batiments,
                            // Autres propriétés de la chambre si nécessaire
                        };

                        // Ajout de la nouvelle chambre à la base de données
                        dbContext.ChambreSet.Add(nouvelleChambre);
                    }
                }

                // Enregistrement des chambres dans la base de données
                await dbContext.SaveChangesAsync();

                // Affichage d'un message de confirmation
                MessageBox.Show("Le bâtiment et ses chambres ont été ajoutés avec succès!", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);

                // Redirection vers la page d'affichage des bâtiments
                (Application.Current.MainWindow as MainWindow)?.NavigateToMesBatiments();
            }
            catch (Exception ex)
            {
                // Gestion des erreurs d'enregistrement
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}