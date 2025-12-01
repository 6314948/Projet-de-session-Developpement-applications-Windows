using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ProjetElectionsWinUI.ViewModels;
using System;

namespace ProjetElectionsWinUI.Views
{
    public sealed partial class ElecteursPage : Page
    {
        /// <summary>
        /// ViewModel principal de la page.
        /// </summary>
        public ElecteursViewModel ViewModel { get; }

        // ======================================================
        //   Constructeur
        // ======================================================
        public ElecteursPage()
        {
            InitializeComponent();
            ViewModel = new ElecteursViewModel();
            DataContext = ViewModel;
        }

        // ======================================================
        //   Confirmation avant suppression
        // ======================================================
        private async void Supprimer_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog
            {
                Title = "Confirmation",
                Content = "Voulez-vous vraiment supprimer cet électeur ?",
                PrimaryButtonText = "Supprimer",
                CloseButtonText = "Annuler",
                DefaultButton = ContentDialogButton.Close,
                XamlRoot = Content.XamlRoot
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                ViewModel.SupprimerElecteur();
            }
        }
    }
}
