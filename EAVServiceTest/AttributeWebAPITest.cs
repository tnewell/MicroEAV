using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace EAVServiceTest
{
    public partial class EAVWebServiceTestHarness
    {

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Retrieve")]
        [TestCategory("Attribute")]
        public void RetrieveNonExistentAttribute()
        {
            HttpResponseMessage response = WebClient.GetAsync(String.Format("api/meta/attributes/{0}", -1)).Result;
            if (response.IsSuccessStatusCode)
            {
                var attribute = response.Content.ReadAsAsync<EAVStoreLibrary.StoreAttribute>().Result;

                Assert.IsNull(attribute, "Unexpected attribute object retrieved.");
            }
            else
            {
                Assert.Fail(response.Content.ReadAsStringAsync().Result);
            }
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Retrieve")]
        [TestCategory("Attribute")]
        public void RetrieveRandomAttribute()
        {
            var dbAttribute = SelectRandomItem(this.DbContext.Attributes);

            if (dbAttribute != null)
            {
                HttpResponseMessage response = WebClient.GetAsync(String.Format("api/meta/attributes/{0}", dbAttribute.Attribute_ID)).Result;
                if (response.IsSuccessStatusCode)
                {
                    var attribute = response.Content.ReadAsAsync<EAVStoreLibrary.StoreAttribute>().Result;

                    Assert.IsNotNull(attribute, "Failed to retrieve attribute {0}.", dbAttribute.Attribute_ID);
                    Assert.AreEqual(dbAttribute.Attribute_ID, attribute.AttributeID, "Attribute ID values do not match.");
                }
                else
                {
                    Assert.Fail(response.Content.ReadAsStringAsync().Result);
                }
            }
            else
            {
                Assert.Inconclusive("No attributes were found in the database.");
            }
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Delete")]
        [TestCategory("Attribute")]
        public void DeleteAttribute()
        {
            var dbContainer = SelectRandomItem(this.DbContext.Containers);
            EAVStoreClient.Attribute dbAttributeIn = CreateAttribute(dbContainer.Container_ID, Guid.NewGuid().ToString(), EAV.EAVDataType.String, rng.Next(), true);

            HttpResponseMessage response = WebClient.DeleteAsync(String.Format("api/meta/attributes/{0}", dbAttributeIn.Attribute_ID)).Result;
            if (response.IsSuccessStatusCode)
            {
                ResetDatabaseContext();

                EAVStoreClient.Attribute dbAttributeOut = this.DbContext.Attributes.SingleOrDefault(it => it.Attribute_ID == dbAttributeIn.Attribute_ID);

                Assert.IsNull(dbAttributeOut, "Failed to delete attribute ID {0} from the database.", dbAttributeIn.Attribute_ID);
            }
            else
            {
                Assert.Fail(response.Content.ReadAsStringAsync().Result);
            }
        }
    }
}
