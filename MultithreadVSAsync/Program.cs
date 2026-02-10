using MultithreadVSAsync.IViewModel;
using MultithreadVSAsync.ViewModel;

static class Program
{
    static void Main(string[] args)
    {
        // Correction : créer une instance concrète de IViewModel (par exemple ViewModel)
        IViewModel viewModel = new ViewModel();
        viewModel.Start();
    }
}