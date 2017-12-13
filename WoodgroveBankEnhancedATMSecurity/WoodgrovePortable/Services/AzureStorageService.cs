
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using WoodgrovePortable.Common;
using WoodgrovePortable.Models;

namespace WoodgrovePortable.Services
{
    public class AzureStorageService
    {
        private async Task<object> GetUserTableAsync()
        {
            //TODO: Create table in Azure

            return null;
        }

        public async Task<object> RegisterUserAsync(string Name, string PIN, string personGroupId)
        {
            //Get reference to user table
            var table = await GetUserTableAsync();


            //TODO: Create a new user entity


            //TODO: Create the TableOperation object that inserts the login entity

            return true;
        }
    }
}
