using Microsoft.Maui.Storage;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.MethodLogging;
using PlanAhead.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanAhead.Services
{

    [MethodLogging]
    public class SecureStorageService : ISecureStorageService
    {
        public Task SetAsync(string key, string value) =>
            SecureStorage.Default.SetAsync(key, value);

        public Task<string?> GetAsync(string key) =>
            SecureStorage.Default.GetAsync(key);

        public void Remove(string key) =>
            SecureStorage.Default.Remove(key);
    }
}
