using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ProjetElectionsWinUI.ViewModels;
using System;

namespace ProjetElectionsWinUI.Views
{
    public sealed partial class CandidatsPage : Page
    {
        public CandidatsViewModel ViewModel { get; }

        public CandidatsPage()
        {
            this.InitializeComponent();
            ViewModel = new CandidatsViewModel();
            this.DataContext = ViewModel;
        }

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
