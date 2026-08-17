using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Navigation;
using PlanAhead.Views;
using PlanAhead.Views.Accounts;
using PlanAhead.Views.Funds;
using PlanAhead.Views.Startup;


namespace PlanAhead
{
    public partial class AppShell : Shell
    {
        private readonly IApplicationStartupService _startup;

        public AppShell(IApplicationStartupService startup)
        {
            InitializeComponent();

            _startup = startup;

            /*
             * Any pages which are standard - go here, go there are defined here
             * Any pages which we need to be set as root - the first page are 
             * declared in App.xaml - and hidden - if necessary
             */
            Routing.RegisterRoute(nameof(AccountEditPage), typeof(AccountEditPage));
            Routing.RegisterRoute(nameof(AccountsPage), typeof(AccountsPage));
            Routing.RegisterRoute(nameof(AccountViewPage), typeof(AccountViewPage));
            Routing.RegisterRoute(nameof(FundEditPage), typeof(FundEditPage));
            Routing.RegisterRoute(nameof(FundsPage), typeof(FundsPage));
            Routing.RegisterRoute(nameof(FundViewPage), typeof(FundViewPage));
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await GoToAsync("//Splash");
        }
    }
}
