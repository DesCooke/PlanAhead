using HomeBudget.Views;

namespace HomeBudget
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(
                nameof(AccountDetailPage),
                typeof(AccountDetailPage));
        }
    }
}
