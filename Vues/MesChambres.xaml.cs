using CiteU.Modele;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

namespace CiteU.Vues
{
    public partial class MesChambres : UserControl
    {
        private Model1 dbContext; // Assurez-vous que le contexte de la base de données est accessible ici.

        public ObservableCollection<ChambreSet> Chambres { get; set; }

        public MesChambres()
        {
            InitializeComponent();
            Chambres = new ObservableCollection<ChambreSet>();
            listeChambres.ItemsSource = Chambres;

            // Initialisez votre contexte de base de données ici
            dbContext = new Model1();

            // Chargez les chambres depuis la base de données
            var chambresQuery = dbContext.ChambreSet.Include("BatimentsSet").ToList();
            foreach (var chambre in chambresQuery)
            {
                Chambres.Add(chambre);
            }
        }

        private void TrierChambres()
        {
            // Obtenez le critère de tri sélectionné
            string critereTri = (cmbTri.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (critereTri == "Niveau")
            {
                Chambres = new ObservableCollection<ChambreSet>(Chambres.OrderBy(chambre => chambre.Niveau));
            }
            else if (critereTri == "Prix")
            {
                Chambres = new ObservableCollection<ChambreSet>(Chambres.OrderBy(chambre => chambre.BatimentsSet.Prix_Chambre));
            }
            else if (critereTri == "Nom du Bâtiment")
            {
                Chambres = new ObservableCollection<ChambreSet>(Chambres.OrderBy(chambre => chambre.BatimentsSet.Nom_Batiment));
            }
            else if (critereTri == "Nombre de Lits par Chambre")
            {
                Chambres = new ObservableCollection<ChambreSet>(Chambres.OrderBy(chambre => chambre.BatimentsSet.Nombre_Lits_Par_Chambre));
            }

            // Mettez à jour la source de liaison pour refléter le nouveau tri
            listeChambres.ItemsSource = Chambres;
        }

        private void cmbTri_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Lorsque la sélection du ComboBox change, triez les chambres
            TrierChambres();
        }
    }
}
