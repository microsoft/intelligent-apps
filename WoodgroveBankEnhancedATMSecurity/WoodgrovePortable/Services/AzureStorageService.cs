
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

        public async Task<object> SignInAsync(string username, string PIN)
        {
            //Get reference to user table
            var table = await GetUserTableAsync();

            //Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<UserEntity>(AppSettings.defaultPersonGroupID, username.ToLower().Replace(" ", ""));

            //Execute the retrieve operation.
            TableResult retrievedResult = await table.ExecuteAsync(retrieveOperation);

            if (retrievedResult.Result != null)
            {
                //User found 
                var result = retrievedResult.Result as UserEntity;
                if (result.PIN == PIN)
                    return true;
                else
                    return "Sorry, you have entered an invalid PIN!";
                //Check for match
            }
            else
            {
                //User not found
                return "Sorry, user not found!";
            }
        }

        public async Task<object> uploadPhotoAsync(string personGroupID, string imageNameWithExtension, Stream ImageStream)
        {
            try
            {
                //TODO: add in code to upload the image to a blob container
                //and returns the URI that references the image
                
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        private async Task<CloudTable> GetFaceTableAsync()
        {
            //TODO: create a table

            return null;
        }

        public async Task<object> AddUserFaceAsync(string Username, string personId, string uri, string persistedFaceID)
        {
            //Get reference to face table
            var table = await GetFaceTableAsync();


            //TODO: create FaceEntity to be inserted into the table           


            //Execute the insert operation.
            try
            {
                //TODO: add code to execute the insert operation
            }
            catch (Exception ex)
            {
                return ex;
            }
            return true;
        }

        public async Task<List<FaceEntity>> LoadFacesAsync(string Username, string personId)
        {
            //TODO: load faces tied to a person


            return null;
        }
    }
}
