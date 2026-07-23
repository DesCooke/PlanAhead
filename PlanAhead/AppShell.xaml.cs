using PlanAhead.Navigation;
using PlanAhead.Views;
using PlanAhead.Views.Accounts;
using PlanAhead.Views.Funds;


namespace PlanAhead
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            RouteRegistry.RegisterRoutes();
        }
    }
}
