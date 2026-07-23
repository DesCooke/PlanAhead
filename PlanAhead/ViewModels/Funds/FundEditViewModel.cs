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

    private Guid _fundId;
    private Guid _accountId;

    public IEnumerable<Frequency> Frequencies =>
        Enum.GetValues<Frequency>();

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
        IDialogService dialogs)
        : base(navigation, dialogs)
    {
        _fundService = fundService;
        _accountService = accountService;

        Title = "New Fund";
    }

    public async Task InitialiseAsync(Guid? fundId = null)
    {
        if (fundId == null)
        {
            //
            // New Fund
            //

            var account =
                (await _accountService.GetAllAsync())
                .OrderBy(a => a.DisplayOrder)
                .FirstOrDefault();

            if (account != null)
                _accountId = account.Id;

            _fundId = Guid.Empty;
            Title = "New Fund";

            return;
        }

        //
        // Existing Fund
        //

        var fund =
            await _fundService.GetByIdAsync(fundId.Value);

        if (fund == null)
            return;

        _fundId = fund.Id;
        _accountId = fund.AccountId;

        Name = fund.Name;
        Description = fund.Description;
        Frequency = fund.Frequency;
        Notes = fund.Notes;

        Title = "Edit Fund";
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            await Dialogs.ShowMessageAsync(
                "Validation",
                "Please enter a fund name.");

            return;
        }

        try
        {
            var fund = new Fund
            {
                Id = _fundId == Guid.Empty
                    ? Guid.NewGuid()
                    : _fundId,

                AccountId = _accountId,

                Name = Name.Trim(),

                Description = Description.Trim(),

                Frequency = Frequency,

                Notes = Notes.Trim()
            };

            if (_fundId == Guid.Empty)
                await _fundService.AddAsync(fund);
            else
                await _fundService.UpdateAsync(fund);

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