using ContactMicroService.Entities.Model;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactMicroService.Test
{
    public class MockContactInfos
    {
        private readonly List<ContactInfo> ContactInfoList;
        public MockContactInfos()
        {

            ContactInfoList = new List<ContactInfo>
            {
            new ContactInfo
            {
                ContactInfoId=1,
                ContactType=Entities.Enums.ContactType.Location,
                Information="Hatay",

            },
            new ContactInfo
            {
                ContactInfoId = 2,
                ContactType=Entities.Enums.ContactType.Location,
                Information="Hatay",
            },
            new ContactInfo
            {
                ContactInfoId=3,
                ContactType=Entities.Enums.ContactType.Location,
                Information="Hatay",
            }
            };
        }

        public List<ContactInfo> GetContactInfos()
        {
            return ContactInfoList;
        }

    }
}
