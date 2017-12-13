
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
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
        StorageCredentials storageCredentials = new StorageCredentials(AppSettings.StorageAccountName, AppSettings.StorageAccountAccessKey);

        private async Task<CloudTable> GetUserTableAsync()
        {
            StorageCredentials storageCredentials = new StorageCredentials(AppSettings.StorageAccountName, AppSettings.StorageAccountAccessKey);
            CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(storageCredentials, useHttps: true);
            // Create the table client.
            CloudTableClient tableClient = cloudStorageAccount.CreateCloudTableClient();
            // Retrieve a reference to the table.
            CloudTable table = tableClient.GetTableReference("userTable");
            // Create the table if it doesn't exist.
            await table.CreateIfNotExistsAsync();
            return table;
        }
        public async Task<object> RegisterUserAsync(string Name, string PIN, string personGroupId)
        {
            //Get reference to user table
            var table = await GetUserTableAsync();

            // Create the user entity
            UserEntity user = new UserEntity();
            user.UserName = Name.ToLower().Replace(" ", "");
            user.Name = Name;
            user.PIN = PIN;
            user.PersonGroupId = personGroupId;
            user.Id = DateTime.Now.ToString("MM-dd-yyyy hh:mm:fff tt").ToString();
            user.PartitionKey = user.PersonGroupId;
            user.RowKey = user.UserName;

            // Create the TableOperation object that inserts the login entity.
            TableOperation insertOperation = TableOperation.Insert(user);

            // Execute the insert operation.
            try
            {
                await table.ExecuteAsync(insertOperation);
            }
            catch (Exception ex)
            {
                return ex;
            }
            return true;
        }

    }
}
