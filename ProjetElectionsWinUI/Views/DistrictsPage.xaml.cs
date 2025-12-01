using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ProjetElectionsWinUI.ViewModels;
using System;

namespace ProjetElectionsWinUI.Views
{
    public sealed partial class DistrictsPage : Page
    {
        /// <summary>
        /// ViewModel associé à la page Districts.
        /// </summary>
        public DistrictsViewModel ViewModel { get; }

        // ======================================================
        //   Constructeur
        // ======================================================
        public DistrictsPage()
        {
            InitializeComponent();

            ViewModel = new DistrictsViewModel();
            DataContext = ViewModel;
        }

        // ======================================================
        //   Confirmation de suppression
        // ======================================================
        private async void Supprimer_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog
            {
                Title = "Confirmation",
                Content = "Voulez-vous vraiment supprimer ce district ?",
                PrimaryButtonText = "Supprimer",
                CloseButtonText = "Annuler",
                DefaultButton = ContentDialogButton.Close,
                XamlRoot = Content.XamlRoot
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                ViewModel.DeleteDistrict();
            }
        }
    }
}
