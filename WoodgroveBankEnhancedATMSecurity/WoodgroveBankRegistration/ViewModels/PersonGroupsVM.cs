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

        
    }
}
