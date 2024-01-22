using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CiteU.Modele;

namespace CiteU.Vues
{
    public partial class FormulaireAjoutBatiment : UserControl
    {
        private Model1 dbContext; // Assurez-vous d'avoir une instance du contexte de base de données

        public FormulaireAjoutBatiment()
        {
            InitializeComponent();
            dbContext = new Model1(); // Initialisation du contexte de base de données
            WireUpValidationHandlers();
        }

        private void WireUpValidationHandlers()
        {
            // Ajouter des gestionnaires pour la validation des champs numériques
            NombreEtagesTextBox.PreviewTextInput += NumberValidationTextBox;
            NombreChambresParEtageTextBox.PreviewTextInput += NumberValidationTextBox;
            NombreLitsParChambreTextBox.PreviewTextInput += NumberValidationTextBox;
            PrixChambreTextBox.PreviewTextInput += NumberValidationTextBox;

            // Ajouter un gestionnaire pour la validation des champs vides
            foreach (var textBox in FindVisualChildren<TextBox>(this))
            {
                textBox.LostFocus += TextBox_LostFocus;
            }
        }

        // Gestionnaire d'événement pour valider les champs numériques
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            e.Handled = !IsTextAllowed(e.Text);
        }

        // Méthode pour vérifier si le texte est un nombre
        private static bool IsTextAllowed(string text)
        {
            return int.TryParse(text, out _);
        }

        // Gestionnaire d'événement pour valider les champs vides
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                MessageBox.Show("Veuillez remplir tous les champs du formulaire.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                textBox.Focus();
            }
        }

        private async void AjouterBatiment_Click(object sender, RoutedEventArgs e)
        {
            // Vérifier si tous les champs sont remplis
            if (!AreAllFieldsFilled())
            {
                MessageBox.Show("Veuillez remplir tous les champs du formulaire.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Récupérer les valeurs du formulaire
            string nomBatiment = NomBatimentTextBox.Text;

            // Assurez-vous de gérer les conversions de manière appropriée pour les champs numériques
            int nombreEtages = Convert.ToInt32(NombreEtagesTextBox.Text);

            // Validation du nombre d'étages (maximum 3)
            if (nombreEtages > 3)
            {
                MessageBox.Show("Un bâtiment ne peut avoir plus de 2 étages.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

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

        // Méthode pour vérifier si tous les champs sont remplis
        private bool AreAllFieldsFilled()
        {
            foreach (var textBox in FindVisualChildren<TextBox>(this))
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    return false;
                }
            }
            return true;
        }

        // Méthode pour trouver tous les enfants de type spécifié dans le visuel
        private IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}