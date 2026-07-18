using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanAhead.core.Interfaces;
using PlanAhead.core.Models.Domain;
using PlanAhead.Repositories;

namespace HomeBudget.ViewModels;

public partial class AccountDetailViewModel : BaseViewModel
{
    private readonly AccountRepository _repository;
    private Account? _editingAccount;
    private readonly INavigationContext _context;

    public AccountDetailViewModel(AccountRepository repository,
        INavigationService navigation,
        IDialogService dialogs,
        INavigationContext parameters): base(navigation, dialogs)
    {
        _repository = repository;
        _context = parameters;

        _editingAccount = _context.Get<Account>();
    }

    [ObservableProperty]
    private string accountName = "";

    [ObservableProperty]
    private decimal balance;

    [ObservableProperty]
    private bool includeInTotal = true;

    public async Task LoadAsync()
    {
        _editingAccount = _context.Get<Account>();

        if (_editingAccount == null)
        {
            // Add mode
            AccountName = "";
            Balance = 0;
            IncludeInTotal = true;
            return;
        }

        // Edit mode
//        AccountName = _editingAccount.Name;
  //      Balance = _editingAccount.Balance;
    //    IncludeInTotal = _editingAccount.IncludeInTotal;
    }

    [RelayCommand]
    private async Task Cancel()
    {
        await Navigation.GoBackAsync();
    }

    [RelayCommand]
    private async Task Save()
    {
        if (string.IsNullOrWhiteSpace(AccountName))
        {
            await Dialogs.ShowErrorAsync(
                "Please enter an account name.");

            return;
        }

        if (_editingAccount == null)
        {
            await _repository.AddAsync(
                new Account
                {
//                    Name = AccountName,
  //                  Balance = Balance,
    //                IncludeInTotal = IncludeInTotal
                });
        }
        else
        {
      //      _editingAccount.Name = AccountName;
        //    _editingAccount.Balance = Balance;
          //  _editingAccount.IncludeInTotal = IncludeInTotal;

            await _repository.UpdateAsync(_editingAccount);
        }

        await Navigation.GoBackAsync();
    }

}