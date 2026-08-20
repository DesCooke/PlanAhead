using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.Models.Enums;
using PlanAhead.Core.Services.Accounts;
using PlanAhead.Interfaces;

namespace PlanAhead.ViewModels.Funds;

public partial class FundEditViewModel : BaseViewModel
{
    private readonly IFundService _fundService;
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

    [ObservableProperty]
    private string iconId = string.Empty;

    public FundEditViewModel(
        IFundService fundService,
        IAccountService accountService,
        INavigationService navigation,
        INavigationContext navigationContext,
        IDialogService dialogs)
        : base(navigation, dialogs)
    {
        _fundService = fundService;
        _navigationContext = navigationContext;

        Title = "New Fund";
    }

    public async Task InitialiseAsync()
    {
        try
        {
            // has a guid - it is the Account Id - it is a new fund
            if (_navigationContext.Has<Guid>())
            {
                AccountId = _navigationContext.Get<Guid>();

                Title = "New Fund";

                Id = Guid.Empty;
                Name = "";
                Description = "";
                Notes = "";
                IconId = "";

                return;
            }

            Title = "Change Fund";

            //
            // Existing Fund
            //

            var fund = _navigationContext.Get<Fund>();
            if(fund!=null)
                Load(fund);

            _navigationContext.Clear();
        }
        catch (Exception ex)
        {
            var msg = $"Error in FundEditViewModel:InitialiseAsync:{ex.Message}";
            await Dialogs.ShowErrorAsync(msg);
        }

    }

    private void Load(Fund fund)
    {
        Id = fund.Id;
        Name = fund.Name;
        AccountId = fund.AccountId;
        Description = fund.Description;
        Notes = fund.Notes;
        IconId = fund.IconId;
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

            Notes = Notes.Trim(),

            IconId = IconId.Trim()
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
        try
        {
            var error = Validate();
            if (error != null)
            {
                await Dialogs.ShowMessageAsync(
                    "Validation",
                    error);

                return;
            }

            if (Id == Guid.Empty)
                await _fundService.AddAsync(Build());
            else
                await _fundService.UpdateAsync(Build());

            await Navigation.GoBackAsync();
        }
        catch (Exception ex)
        {
            var msg = $"Error in FundEditViewModel:SaveAsync:{ex.Message}";
            await Dialogs.ShowErrorAsync(msg);
        }
    }

    [RelayCommand]
    private Task CancelAsync()
    {
        try
        {
            return Navigation.GoBackAsync();
        }
        catch (Exception ex)
        {
            var msg = $"Error in FundEditViewModel:CancelAsync:{ex.Message}";
            Dialogs.ShowErrorAsync(msg);
        }
        return Task.CompletedTask;
            
    }
}