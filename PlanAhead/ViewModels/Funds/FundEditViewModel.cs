using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.Models.Domain;
using PlanAhead.Core.Models.Enums;
using PlanAhead.Interfaces;

namespace PlanAhead.ViewModels.Funds;

public partial class FundEditViewModel : BaseViewModel
{
    private readonly IFundService _fundService;
    private readonly IAccountService _accountService;
    private readonly INavigationContext _navigationContext;

    public IEnumerable<Frequency> Frequencies =>
        Enum.GetValues<Frequency>();


    [ObservableProperty]
    private Guid id;

    [ObservableProperty]
    private Guid accountId;

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string description = string.Empty;

    [ObservableProperty]
    private Frequency frequency = Frequency.Monthly;

    [ObservableProperty]
    private string notes = string.Empty;

    public FundEditViewModel(
        IFundService fundService,
        IAccountService accountService,
        INavigationService navigation,
        INavigationContext navigationContext,
        IDialogService dialogs)
        : base(navigation, dialogs)
    {
        _fundService = fundService;
        _accountService = accountService;
        _navigationContext = navigationContext;

        Title = "New Fund";
    }

    public async Task InitialiseAsync()
    {
        var fund = _navigationContext.Get<Fund>();

        if (fund == null)
        {
            //
            // New fund
            //
            return;
        }

        //
        // Existing fund
        //

        Load(fund);

        _navigationContext.Clear();
    }

    private void Load(Fund fund)
    {
        Id = fund.Id;
        Name = fund.Name;
        AccountId = fund.AccountId;
        Notes = fund.Notes;
    }

    private Fund Build()
    {
        return new Fund
        {
            Id = Id,

            AccountId = AccountId,

            Name = Name.Trim(),

            Description = Description.Trim(),

            Frequency = Frequency,

            Notes = Notes.Trim()
        };
    }

    string? Validate() 
    {
        if (string.IsNullOrWhiteSpace(Name))
            return("Please enter a fund name.");

        return null;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        var error = Validate();
        if (error!=null)
        {
            await Dialogs.ShowMessageAsync(
                "Validation",
                error);

            return;
        }

        try
        {
            if (Id == Guid.Empty)
                await _fundService.AddAsync(Build());
            else
                await _fundService.UpdateAsync(Build());

            await Navigation.GoBackAsync();
        }
        catch (Exception ex)
        {
            await Dialogs.ShowErrorAsync(ex.Message);
        }
    }
    [RelayCommand]
    private Task CancelAsync()
    {
        return Navigation.GoBackAsync();
    }
}