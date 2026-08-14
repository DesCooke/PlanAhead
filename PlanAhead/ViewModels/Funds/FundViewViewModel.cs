using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.Models.Enums;
using PlanAhead.Core.Services.Accounts;
using PlanAhead.Interfaces;
using PlanAhead.Views.Accounts;
using PlanAhead.Views.Funds;

namespace PlanAhead.ViewModels.Funds;

public partial class FundViewViewModel : BaseViewModel
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

    public FundViewViewModel(
        IFundService fundService,
        IAccountService accountService,
        INavigationService navigation,
        INavigationContext navigationContext,
        IDialogService dialogs)
        : base(navigation, dialogs)
    {
        _fundService = fundService;
        _navigationContext = navigationContext;

        Title = "Fund Details";
    }

    private void Load(Fund fund)
    {
        Id = fund.Id;
        AccountId = fund.AccountId;
        Name = fund.Name;
        Description = fund.Description;
        Frequency = fund.Frequency;
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

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (Id == Guid.Empty)
        {
            Id = _navigationContext.Get<Guid>();
            _navigationContext.Clear();
        }

        var fund = await _fundService.GetByIdAsync(Id);

        if (fund != null)
            Load(fund);
    }

    [RelayCommand]
    private async Task EditAsync()
    {
        _navigationContext.Set(Id);

        await Shell.Current.GoToAsync("FundEditPage");
    }

    [RelayCommand]
    private Task CancelAsync()
    {
        return Navigation.GoBackAsync();
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        var delete =
            await Dialogs.ConfirmAsync(
                "Delete Fund",
                $"Delete '{Name}'?");

        if (!delete)
            return;

        await _fundService.DeleteAsync(Id);

        await Navigation.GoBackAsync();
    }
}