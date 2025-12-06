using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ProjetElectionsWinUI.Views;

namespace ProjetElectionsWinUI
{
    /// <summary>
    /// Fenêtre principale de l’application.
    /// Elle contient le menu (NavigationView) et charge les différentes pages.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Je redimensionne la fenêtre au démarrage pour avoir une taille plus confortable.
            // (Le projet ouvrait en petit au début. J’ai ajusté la taille pour qu'on voie tout d'un coup.)
            this.AppWindow.Resize(new Windows.Graphics.SizeInt32(1500, 850));

            // Page affichée par défaut quand l’application ouvre.
            ContentFrame.Navigate(typeof(DistrictsPage));
        }

        /// <summary>
        /// Gestion du menu de navigation.
        /// Lorsqu’on clique sur un item du NavigationView, on change la page dans le Frame.
        /// </summary>
        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItem is NavigationViewItem item)
            {
                switch (item.Tag)
                {
                    case "districts":
                        ContentFrame.Navigate(typeof(DistrictsPage));
                        break;

                    case "candidats":
                        ContentFrame.Navigate(typeof(CandidatsPage));
                        break;

                    case "electeurs":
                        ContentFrame.Navigate(typeof(ElecteursPage));
                        break;
                }
            }
        }
    }
}
