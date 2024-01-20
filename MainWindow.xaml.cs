using CiteU.Vues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using CiteU.Vues;

namespace CiteU
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Afficher initialement le contrôle MesEtudiants
            AfficherPage(new Mesbatiments());
        }

        public void NavigateToMesBatiments()
        {
            MainFrame.Navigate(new Mesbatiments());
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            IconMesEtudiants.Foreground = new SolidColorBrush(Color.FromRgb(0x3C, 0x40, 0x48));
            IconMesPayements.Foreground = new SolidColorBrush(Color.FromRgb(0x3C, 0x40, 0x48));
            IconMesChambres.Foreground = new SolidColorBrush(Color.FromRgb(0x3C, 0x40, 0x48));
            IconHome.Foreground = new SolidColorBrush(Colors.White);
            ChangerCouleurBordure(btnHomeBorder);
            AfficherPage(new Mesbatiments());
        }

        private void btnMesEtudiants_Click(object sender, RoutedEventArgs e)
        {
            ChangerCouleurBordure(btnMesEtudiantsBorder);
            AfficherPage(new MesEtudiants());
        }

        private void btnCreditCard_Click(object sender, RoutedEventArgs e)
        {
            IconHome.Foreground = new SolidColorBrush(Color.FromRgb(0x3C, 0x40, 0x48));
            IconMesEtudiants.Foreground = new SolidColorBrush(Color.FromRgb(0x3C, 0x40, 0x48));
            IconMesChambres.Foreground = new SolidColorBrush(Color.FromRgb(0x3C, 0x40, 0x48));
            IconHome.Foreground = new SolidColorBrush(Color.FromRgb(0x3C, 0x40, 0x48));
            IconMesPayements.Foreground = new SolidColorBrush(Colors.White);
            ChangerCouleurBordure(btnCreditCardBorder);
            AfficherPage(new MesPayements());
        }

        private void btnChambre_Click(object sender, RoutedEventArgs e)
        {
            IconHome.Foreground = new SolidColorBrush(Color.FromRgb(0x3C, 0x40, 0x48));
            IconMesPayements.Foreground = new SolidColorBrush(Color.FromRgb(0x3C, 0x40, 0x48));
            IconMesEtudiants.Foreground = new SolidColorBrush(Color.FromRgb(0x3C, 0x40, 0x48));
            IconHome.Foreground = new SolidColorBrush(Color.FromRgb(0x3C, 0x40, 0x48));
            IconMesChambres.Foreground = new SolidColorBrush(Colors.White);
            ChangerCouleurBordure(btnChambreBorder);
            AfficherPage(new MesChambres());
        }

        public void AfficherPage(UserControl page)
        {
            // Remplacez la page actuelle dans le Frame
            MainFrame.Content = page;
        }

        private void ChangerCouleurBordure(Border border)
        {
            // Mettez à jour les couleurs des bordures pour indiquer l'élément actif
            btnCreditCardBorder.Background = new SolidColorBrush(Colors.Transparent);
            btnMesEtudiantsBorder.Background = new SolidColorBrush(Colors.Transparent);
            btnHomeBorder.Background = new SolidColorBrush(Colors.Transparent);
            btnChambreBorder.Background = new SolidColorBrush(Colors.Transparent);

            border.Background = new SolidColorBrush(Color.FromRgb(0x3C, 0x40, 0x48));
        }

    }
}
