
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
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
                //Reference to the storage account
                CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(storageCredentials, useHttps: true);

                //Create a blob client
                CloudBlobClient client = cloudStorageAccount.CreateCloudBlobClient();

                //Define a container reference to the Person Group ID
                CloudBlobContainer container = client.GetContainerReference(personGroupID);

                //If a container with the name of the Person Group ID does not exist, then create a new container
                await container.CreateIfNotExistsAsync();

                //Set container permissions
                await container.SetPermissionsAsync(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });

                //The image will be uploaded as a block blob
                CloudBlockBlob cblob = container.GetBlockBlobReference(imageNameWithExtension);

                //Upload to the block blob from the image stream
                await cblob.UploadFromStreamAsync(ImageStream);

                //Upload process will return a Uri that references the image
                return cblob.StorageUri.PrimaryUri.ToString();
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        private async Task<CloudTable> GetFaceTableAsync()
        {
            CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(storageCredentials, useHttps: true);

            // Create the table client.
            CloudTableClient tableClient = cloudStorageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            CloudTable table = tableClient.GetTableReference("faceTable");

            // Create the table if it doesn't exist.
            await table.CreateIfNotExistsAsync();

            return table;
        }

        public async Task<object> AddUserFaceAsync(string Username, string personId, string uri, string persistedFaceID)
        {
            //Get reference to face table
            var table = await GetFaceTableAsync();

            //Create face entity
            FaceEntity face = new FaceEntity();
            face.UserName = Username;
            face.ImageUrl = uri;
            face.Id = persistedFaceID;
            face.PersonId = personId;
            face.RowKey = face.Id;
            face.PartitionKey = face.UserName;

            //Create the TableOperation object that inserts the login entity.
            TableOperation insertOperation = TableOperation.Insert(face);

            //Execute the insert operation.
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

        public async Task<List<FaceEntity>> LoadFacesAsync(string Username, string personId)
        {
            //Get reference to face table
            var table = await GetFaceTableAsync();
            TableContinuationToken token = null;

            //Get all entities
            var entities = new List<FaceEntity>();
            do
            {
                var queryResult = await table.ExecuteQuerySegmentedAsync(new TableQuery<FaceEntity>(), token);
                entities.AddRange(queryResult.Results);
                token = queryResult.ContinuationToken;
            } while (token != null);

            //Find all registered faces for the user
            var faceList = new List<FaceEntity>();
            foreach (var item in entities)
            {
                if ((item.PersonId == personId) && (item.UserName == Username))
                    faceList.Add(item);
            }

            //Return list of faces
            return faceList;
        }

        public async Task<object> DeleteUserAsync(string personGroupID, string username)
        {
            try
            {
                //Get reference to user table
                var table = await GetUserTableAsync();

                //TODO: delete the user from the table


                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public async Task<object> DeleteFaceAsync(string username, string persistedFaceId)
        {
            try
            {
                //Get reference to face table
                var table = await GetFaceTableAsync();

                //TODO: delete the face from the table


                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public async Task<object> DeleteImageAsync(string personGroupID, string fileName)
        {
            try
            {
                //TODO: delete image from Azure blob storage

                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}
