using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WoodgrovePortable.Common;
using WoodgrovePortable.Models;

namespace WoodgrovePortable.Services
{
    public class FaceAPIService
    {
        HttpContent content;
        HttpResponseMessage responseMessage;

        private HttpClient FaceClient()
        {
            HttpClient faceClient = new HttpClient();
            faceClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", AppSettings.FaceAPIKey);
            return faceClient;
        }

        #region Person Groups
        //PUT create person group
        public async Task<HttpResponseMessage> CreatePersonGroupAsync(string GroupID, string GroupName, string GroupDescription)
        {
            try
            {
                using (var client = FaceClient())
                {
                    string uri = AppSettings.baseuri + "/persongroups/" + GroupID;

                    // Request body
                    PersonGroup pg = new PersonGroup() { name = GroupName, userData = GroupDescription };
                    string jsonRequest = JsonConvert.SerializeObject(pg);

                    content = new StringContent(jsonRequest);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    responseMessage = await client.PutAsync(uri, content);
                }
            }
            catch (Exception ex)
            {
            }
            return responseMessage;
        }

        //GET all person groups
        public async Task<object> ListAllPersonGroupsAsync()
        {
            List<PersonGroupDetails> plist = new List<PersonGroupDetails>();
            try
            {
                using (var client = FaceClient())
                {
                    // Request body
                    var uri = AppSettings.baseuri + "/persongroups";
                    responseMessage = await client.GetAsync(uri);
                }
            }
            catch (Exception ex)
            {
               return ex;
            }
            return responseMessage;
        }

        //DELETE person group
        public async Task<HttpResponseMessage> DeletePersonGroupAsync(string GroupID)
        {
            try
            {
                using (var client = FaceClient())
                {
                    // Request body
                    var uri = AppSettings.baseuri + "/persongroups/" + GroupID;
                    responseMessage = await client.DeleteAsync(uri);
                }
            }
            catch (Exception ex)
            { }
            return responseMessage;
        }

        //POST Train person group
        public async Task<object> TrainPersonGroupAsync(string GroupID)
        {
            try
            {
                using (var client = FaceClient())
                {
                    string uri = AppSettings.baseuri + "/persongroups/" + GroupID + "/train";
                    responseMessage = await client.PostAsync(uri, content);
                }
            }
            catch (Exception ex)
            {
                return ex;
            }
            return responseMessage;
        }

        //GET Training status of person group
        public async Task<HttpResponseMessage> GetPersonGroupTrainingStatus(string GroupID)
        {
            try
            {
                using (var client = FaceClient())
                {
                    // Request body
                    var uri = AppSettings.baseuri + "/persongroups/" + GroupID + "/training";
                    responseMessage = await client.GetAsync(uri);
                }
            }
            catch (Exception ex)
            {
            }
            return responseMessage;
        }

        #endregion

        #region Persons
        //POST create person in person group
        public async Task<HttpResponseMessage> CreatePersonAsync(string GroupID, string PersonName)
        {
            using (var client = FaceClient())
            {
                //Form the request URL
                string uri = AppSettings.baseuri + "/persongroups/" + GroupID + "/persons?";
                
                //Create a username using the person's name
                string username = PersonName.ToLower().Replace(" ", "");

                // Request body
                string jsonRequest = "{\"name\":\"" + PersonName + "\",\"userData\":\"" + username + "\"}";
                byte[] byteData = Encoding.UTF8.GetBytes(jsonRequest);

                content = new ByteArrayContent(byteData);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                responseMessage = await client.PostAsync(uri, content);
                return responseMessage;
            }
        }

        //GET all persons in a person group
        public async Task<object> ListPersonsAsync(string GroupID)
        {
            List<PersonDetails> plist = new List<PersonDetails>();
            try
            {
                using (var client = FaceClient())
                {
                    // Request body
                    var uri = AppSettings.baseuri + "/persongroups/" + GroupID + "/persons?start=0&top=1000";

                    responseMessage = await client.GetAsync(uri);
                    var responseContent = await responseMessage.Content.ReadAsStringAsync();
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        plist = JsonConvert.DeserializeObject<List<PersonDetails>>(responseContent);
                    }
                    else return false;
                }
            }
            catch(Exception ex)
            {
                return ex;
            }
            return plist;
        }

        //GET person using person ID
        public async Task<HttpResponseMessage> GetPersonAsync(string GroupID, string PersonID)
        {
            using (var client = new HttpClient())
            {
                // Request body
                var uri = AppSettings.baseuri + "/persongroups/" + GroupID + "/persons/" + PersonID;

                responseMessage = await client.GetAsync(uri);
                return responseMessage;
            }
        }

        //DELETE Person using person ID
        public async Task<HttpResponseMessage> DeletePersonAsync(string GroupID, string PersonID)
        {
            using (var client = FaceClient())
            {
                // Request body
                var uri = AppSettings.baseuri + "/persongroups/" + GroupID + "/persons/" + PersonID;
                var response = await client.DeleteAsync(uri);
                return response;
            }
        }
            #endregion

        #region Faces

        //POST Create a person face
        public async Task<HttpResponseMessage> CreatePersonFaceAsync(string GroupID, string PersonID, string FaceUrl)
        {
            using (var client = FaceClient())
            {
                string uri = AppSettings.baseuri + "/persongroups/" + GroupID + "/persons/" + PersonID + "/persistedFaces?";

                // Request body
                string jsonRequest = "{\"url\":\"" + FaceUrl + "\"}";
                byte[] byteData = Encoding.UTF8.GetBytes(jsonRequest);

                content = new ByteArrayContent(byteData);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                responseMessage = await client.PostAsync(uri, content);
                return responseMessage;
            }
        }

        //POST Detect face
        public async Task<HttpResponseMessage> DetectFaceAsync(string ImageUrl)
        {
            using (var client = FaceClient())
            {
                string uri = AppSettings.baseuri + "/detect";

                // Request body
                string jsonRequest = "{\"url\":\"" + ImageUrl + "\"}";
                byte[] byteData = Encoding.UTF8.GetBytes(jsonRequest);

                content = new ByteArrayContent(byteData);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                responseMessage = await client.PostAsync(uri, content);
                return responseMessage;
            }
        }

        //POST Verify face
        public async Task<HttpResponseMessage> IdentifyFaceAsync(float confidenceThreshold, string[] faceIds, int maxNumOfCandidatesReturned, string personGroupId)
        {
            using (var client = FaceClient())
            {
                //Request Headers
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", AppSettings.APIKEY);

                //Request Url
                string uri = AppSettings.baseuri + "/identify";

                var request = new IdenfityFaceRequestModel() { confidenceThreshold = confidenceThreshold, faceIds = faceIds, maxNumOfCandidatesReturned = maxNumOfCandidatesReturned, personGroupId = personGroupId };

                // Request body
                string jsonRequest = JsonConvert.SerializeObject(request);
                byte[] byteData = Encoding.UTF8.GetBytes(jsonRequest);

                content = new ByteArrayContent(byteData);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                responseMessage = await client.PostAsync(uri, content);
                return responseMessage;
            }
        }

        //DELETE a person face
        public async Task<object> DeleteFaceAsync(string GroupID, string PersonID, string persistedFaceID)
        {
            try
            {
                using (var client = FaceClient())
                {
                    // Request body
                    var uri = AppSettings.baseuri + "/persongroups/" + GroupID + "/persons/" + PersonID + "/persistedFaces/" + persistedFaceID;
                    var response = await client.DeleteAsync(uri);
                    return response;
                }
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
        #endregion
    }
}
