using CiteU.Modele;
using CiteUContext = CiteU.Modele.Model1;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;
using System.Windows.Threading;

namespace CiteU.Vues
{
    public partial class Mesbatiments : UserControl
    {
        public ObservableCollection<BatimentsSet> ListOfBatiments { get; set; }
        private CollectionViewSource batimentsCollectionViewSource;

        public Mesbatiments()
        {
            InitializeComponent();
            batimentsCollectionViewSource = FindResource("BatimentsCollectionViewSource") as CollectionViewSource;

            using (var context = new CiteUContext())
            {
                ListOfBatiments = new ObservableCollection<BatimentsSet>(context.BatimentsSet.ToList());
            }

            DataContext = this;
        }

        private void AjouterBatiment_Click(object sender, RoutedEventArgs e)
        {
            // Créer une instance du formulaire d'ajout de bâtiment
            FormulaireAjoutBatiment formulaireAjoutBatiment = new FormulaireAjoutBatiment();

            // Afficher le formulaire dans la fenêtre principale
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                // Utiliser la méthode de la fenêtre principale pour changer la page
                mainWindow.AfficherPage(formulaireAjoutBatiment);
            }
        }
    }
}
