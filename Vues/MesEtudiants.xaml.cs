using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

        public ICommand AttribuerChambreCommand { get; private set; }

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

            // Initialiser la commande pour attribuer une chambre
            AttribuerChambreCommand = new RelayCommand(AttribuerChambreExecute, AttribuerChambreCanExecute);
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

        // Méthode pour exécuter l'attribution de la chambre
        private void AttribuerChambreExecute(object parameter)
        {
            if (parameter is EtudiantsSet etudiant)
            {
                using (var context = new Model1())
                {
                    string resultatAttribution = AttribuerChambreEtudiant(context, etudiant);
                    MessageBox.Show(resultatAttribution, "Résultat de l'attribution de chambre", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        // Méthode pour vérifier si l'attribution de chambre peut être exécutée
        private bool AttribuerChambreCanExecute(object parameter)
        {
            // Vérifier si la commande peut être exécutée
            return true;
        }

        // Modifier la signature de la méthode AttribuerChambreEtudiant pour renvoyer un string
        private string AttribuerChambreEtudiant(Model1 context, EtudiantsSet etudiant)
        {
            // Utilisez la méthode AttribuerChambrePourEtudiant que nous avons définie précédemment
            ChambreSet chambreAttribuee = AttribuerChambrePourEtudiant(context, etudiant);

            if (chambreAttribuee != null)
            {
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
                    Etudiants_Matricule = etudiant.Matricule
                };

                // Ajoutez le paiement à la base de données
                context.PaimentSet.Add(nouveauPaiement);
                context.SaveChanges();

                // Retournez un message de succès
                return "Chambre attribuée avec succès.";
            }
            else
            {
                // Retournez un message d'erreur
                return "Aucune chambre disponible pour l'étudiant.";
            }
        }

        // Méthode d'attribution de chambre pour un étudiant
        private ChambreSet AttribuerChambrePourEtudiant(Model1 context, EtudiantsSet nouvelEtudiant)
        {
            // Créer une nouvelle réservation
            ReservationSet nouvelleReservation = new ReservationSet
            {
                Date_Debut = DateTime.Now,
                Date_Fin = DateTime.Now.AddMonths(6)
            };

            // Attribuer une chambre à la nouvelle réservation
            nouvelleReservation.ChambreSet = TrouverChambreDisponiblePourEtudiant(context, nouvelEtudiant);

            // Ajouter la réservation à la base de données
            context.ReservationSet.Add(nouvelleReservation);
            context.SaveChanges();

            // Retourner la chambre attribuée
            return nouvelleReservation.ChambreSet;
        }

        // Méthode de recherche de chambre disponible pour un étudiant
        private ChambreSet TrouverChambreDisponiblePourEtudiant(Model1 context, EtudiantsSet nouvelEtudiant)
        {
            // Implémentez ici votre logique d'attribution de chambre similaire à celle dans FormulaireAjoutEtudiant

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

        // Implémentez l'interface INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public class RelayCommand : ICommand
        {
            private readonly Action<object> _execute;
            private readonly Predicate<object> _canExecute;

            public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
            {
                _execute = execute ?? throw new ArgumentNullException(nameof(execute));
                _canExecute = canExecute;
            }

            public bool CanExecute(object parameter)
            {
                return _canExecute == null || _canExecute(parameter);
            }

            public void Execute(object parameter)
            {
                _execute(parameter);
            }

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }
        }
    }
}