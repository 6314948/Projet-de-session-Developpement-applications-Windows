using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ProjetElectionsWinUI.ViewModels;
using System;

namespace ProjetElectionsWinUI.Views
{
    public sealed partial class DistrictsPage : Page
    {
        public DistrictsViewModel ViewModel { get; set; }

        public DistrictsPage()
        {
            this.InitializeComponent();

            // On crée et on stocke le ViewModel
            ViewModel = new DistrictsViewModel();

            // On lie le ViewModel à la page
            this.DataContext = ViewModel;
        }

        // Quand on clique sur le bouton Supprimer, On ouvre un dialogue de confirmation
        private async void Supprimer_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog
            {
                Title = "Confirmation",
                Content = "Voulez-vous vraiment supprimer ce district ?",
                PrimaryButtonText = "Supprimer",
                CloseButtonText = "Annuler",
                DefaultButton = ContentDialogButton.Close,
                XamlRoot = this.Content.XamlRoot
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                // Appel direct à la méthode de suppression du ViewModel
                ViewModel.DeleteDistrict();
            }
        }
    }
}