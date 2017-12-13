using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using WoodgrovePortable.Common;
using WoodgrovePortable.Helpers;
using WoodgrovePortable.Models;
using WoodgrovePortable.Services;

namespace WoodgroveBankRegistration.ViewModels
{
    public class PersonGroupsVM : BindableBase
    {
        MessageDialog msg = new MessageDialog("");
        FaceAPIService faceService = new FaceAPIService();

        private List<PersonGroupDetails> persongroupList;
        public List<PersonGroupDetails> PersonGroupList
        {
            get { return persongroupList; }
            set { SetProperty(ref persongroupList, value); }
        }

        public PersonGroupsVM()
        {
            PersonGroupList = new List<PersonGroupDetails>();
        }


        public async Task<bool> InitializePersonGroupsAsync()
        {
            //TODO: uncomment code
            /*//Get all person groups
            var response = await faceService.ListAllPersonGroupsAsync();
            if (response is Exception)
            {
                //Could not retrieve the list of person groups
                msg.Title = "Unable to retrieve person groups";
                msg.Content = (response as Exception).Message;
                await msg.ShowAsync();
                return false;
            }
            else
            {
                var result = response as HttpResponseMessage;
                var responseMessage = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    //Deserialize the response to a list of person groups
                    PersonGroupList = JsonConvert.DeserializeObject<List<PersonGroupDetails>>(responseMessage);

                    //Check if the default person group specified in AppSettings.cs exists
                    foreach (var item in PersonGroupList)
                    {
                        if (item.personGroupId == AppSettings.defaultPersonGroupID)
                            return true;
                    }

                    //Create the default person group if it does not exist
                    var createPersonGroup = await faceService.CreatePersonGroupAsync(AppSettings.defaultPersonGroupID, AppSettings.defaultPersonGroupName, "All woodgrove ATM users");

                    if (!createPersonGroup.IsSuccessStatusCode)
                    {
                        var errorMessage = await createPersonGroup.Content.ReadAsStringAsync();
                        msg.Title = "Unable to create person group";
                        msg.Content = errorMessage;
                        await msg.ShowAsync();
                        return false;
                    }
                    else
                        //Train person group
                        await faceService.TrainPersonGroupAsync(AppSettings.defaultPersonGroupID);

                }
                else
                {
                    //Could not retrieve the list of person groups
                    msg.Title = "Unable to retrieve person groups";
                    msg.Content = responseMessage;
                    await msg.ShowAsync();
                    return false;
                }
                return true;
            }*/

            //TODO: remove this returm since there already exists a
            //return element in both paths of the if statement so this will neve be hit
            return false;
        }
    }
}
