using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ProjetElectionsWinUI.ViewModels;
using System;

namespace ProjetElectionsWinUI.Views
{
    public sealed partial class CandidatsPage : Page
    {
        /// <summary>
        /// ViewModel associé à la page.
        /// </summary>
        public CandidatsViewModel ViewModel { get; }

        // ======================================================
        //   Constructeur
        // ======================================================
        public CandidatsPage()
        {
            InitializeComponent();
            ViewModel = new CandidatsViewModel();
            DataContext = ViewModel;
        }

        // ======================================================
        //   Confirmation avant la suppression
        // ======================================================
        private async void Supprimer_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog
            {
                Title = "Confirmation",
                Content = "Voulez-vous vraiment supprimer ce candidat ?",
                PrimaryButtonText = "Supprimer",
                CloseButtonText = "Annuler",
                DefaultButton = ContentDialogButton.Close,
                XamlRoot = this.Content.XamlRoot
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                ViewModel.SupprimerCandidat();
            }
        }
    }
}
