
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

        //TODO: Add in FaceClient function that returns an HttpClient


        //PUT Create person group
        public async Task<HttpResponseMessage> CreatePersonGroupAsync(string GroupID, string GroupName, string GroupDescription)
        {
            //TODO: using the FaceClient function, add in code
            //to create a new PersonGroup

            return null;
        }

        //POST Train person group
        public async Task<object> TrainPersonGroupAsync(string GroupID)
        {
            try
            {
                //TODO: train the person group based of the GroupID

            }
            catch (Exception ex)
            {
                return ex;
            }
            return null;
        }

        //GET all person groups
        public async Task<object> ListAllPersonGroupsAsync()
        {
            List<PersonGroupDetails> plist = new List<PersonGroupDetails>();
            try
            {
                //TODO: get the list of the person groups

            }
            catch (Exception ex)
            {
                return ex;
            }
            return null;
        }

        //GET all persons in a person group
        public async Task<object> ListPersonsAsync(string GroupID)
        {
            List<PersonDetails> plist = new List<PersonDetails>();
            try
            {
                //TODO: list all the persons contained within the person group tied to the GroupID

            }
            catch (Exception ex)
            {
                return ex;
            }
            return plist;
        }

        //POST create person in person group
        public async Task<HttpResponseMessage> CreatePersonAsync(string GroupID, string PersonName)
        {
            //TODO: add in code that will create a new person

            return null;
        }
    }
}
