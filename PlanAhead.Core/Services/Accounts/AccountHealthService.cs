using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.Models.Domain;
using PlanAhead.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanAhead.Core.Services.Accounts
{
    public class AccountHealthService: IAccountHealthService
    {
        public async Task<Status> GetStatusAsync(Account account)
        {
            return
                    account.OpeningBalance switch
                    {
                        < 0 => Status.Red,
                        < 500 => Status.Amber,
                        _ => Status.Green
                    };
        }
    }
}
