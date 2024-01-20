using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using CiteU.Modele;

namespace CiteU.Vues
{
    public partial class MesEtudiants : UserControl, INotifyPropertyChanged
    {
        private ObservableCollection<EtudiantsSet> _etudiantsList;

        public ObservableCollection<EtudiantsSet> EtudiantsList
        {
            get { return _etudiantsList; }
            set
            {
                if (_etudiantsList != value)
                {
                    _etudiantsList = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<BatimentsSet> _batimentsList;

        public ObservableCollection<BatimentsSet> BatimentsList
        {
            get { return _batimentsList; }
            set
            {
                _batimentsList = value;
                OnPropertyChanged();
            }
        }

        public string Message { get; set; } // Ajout d'une propriété pour le message

        public MesEtudiants()
        {
            InitializeComponent();
            // Initialiser la liste d'étudiants
            EtudiantsList = new ObservableCollection<EtudiantsSet>(GetEtudiantsFromDatabase());
            // Initialiser la liste des bâtiments
            BatimentsList = new ObservableCollection<BatimentsSet>(GetBatimentsFromDatabase());
            // Définir le DataContext
            DataContext = this;

            // Désactiver le bouton AjouterEtudiant si la liste des bâtiments est vide
            if (BatimentsList.Count == 0)
            {
                AjouterEtudiantButton.IsEnabled = false;
            }
        }

        public List<EtudiantsSet> GetEtudiantsFromDatabase()
        {
            // À implémenter : Récupérer la liste des étudiants depuis la base de données
            using (var context = new Model1())
            {
                return context.EtudiantsSet.ToList();
            }
        }

        public List<BatimentsSet> GetBatimentsFromDatabase()
        {
            // À implémenter : Récupérer la liste des bâtiments depuis la base de données
            using (var context = new Model1())
            {
                return context.BatimentsSet.ToList();
            }
        }

        private void AjouterEtudiant_Click(object sender, RoutedEventArgs e)
        {
            // Vérifier s'il y a des bâtiments disponibles
            if (BatimentsList.Count > 0)
            {
                // Créer une instance du FormulaireAjoutEtudiant
                FormulaireAjoutEtudiant formulaireAjoutEtudiant = new FormulaireAjoutEtudiant();

                // Afficher le formulaire dans la fenêtre principale
                if (Application.Current.MainWindow is MainWindow mainWindow)
                {
                    // Utiliser la méthode de la fenêtre principale pour changer la page
                    mainWindow.AfficherPage(formulaireAjoutEtudiant);
                }
                }
                else
                {
                MessageBox.Show("Aucun bâtiment n'est disponible. Veuillez ajouter des bâtiments avant d'ajouter des étudiants.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Implémentez l'interface INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
