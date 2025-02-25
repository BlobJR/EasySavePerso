using EasySave.Core.Models;
using EasySave.Desktop.ViewModels;
using System.Windows;


namespace EasySave.Desktop.Views
{
    public partial class ModifyPage : Window
    {
        /// <summary>
        /// Interaction logic for ModifyPage.xaml
        /// </summary>
        /// <param name="selectedJob">the job selected in the UI</param>
        public ModifyPage(Job selectedJob)
        {
            InitializeComponent();
            var viewModel = new ModifyViewModel(selectedJob);
            DataContext = viewModel;
            viewModel.CloseAction = () => this.Close(); // to close the window when the buttom is clicked
        }
    }
}