using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;


namespace EAVSandbox
{
    public partial class EAVSandboxForm : Form
    {
        public EAVSandboxForm()
        {
            InitializeComponent();
        }

        private Model.EAVContext BuildContext()
        {
            Queue<EAV.Model.EAVDataType> types = new Queue<EAV.Model.EAVDataType>(Enum.GetValues(typeof(EAV.Model.EAVDataType)).OfType<EAV.Model.EAVDataType>());

            Model.EAVContext context = new Model.EAVContext()
            {
                Name = "Test",
                DataName = "TEST",
                DisplayText = "Test Context",
            };

            Model.EAVRootContainer rootContainer;
            Model.EAVChildContainer childContainer;

            rootContainer = new Model.EAVRootContainer()
            {
                Name = "Container 1",
                DataName = "CONTAINER_1",
                DisplayText = "Root Container 1",
                IsRepeating = false,
            };

            context.Containers.Add(rootContainer);

            for (int i = 1; i <= 3; ++i)
            {
                rootContainer.Attributes.Add(new Model.EAVAttribute()
                {
                    Name = String.Format("Attribute 1{0}", i),
                    DataName = String.Format("ATTRIBUTE_1{0}", i),
                    DisplayText = String.Format("Attribute 1{0}", i),
                    IsKey = i == 1,
                    DataType = types.Peek(),
                });

                types.Enqueue(types.Dequeue());
            }

            childContainer = new Model.EAVChildContainer()
            {
                Name = "Container 11",
                DataName = "CONTAINER_11",
                DisplayText = "Child Container 1-1",
                IsRepeating = false,
            };

            rootContainer.ChildContainers.Add(childContainer);

            for (int i = 1; i <= 3; ++i)
            {
                childContainer.Attributes.Add(new Model.EAVAttribute()
                {
                    Name = String.Format("Attribute 11{0}", i),
                    DataName = String.Format("ATTRIBUTE_11{0}", i),
                    DisplayText = String.Format("Attribute 11{0}", i),
                    IsKey = i == 1,
                    DataType = types.Peek(),
                });

                types.Enqueue(types.Dequeue());
            }

            childContainer = new Model.EAVChildContainer()
            {
                Name = "Container 12",
                DataName = "CONTAINER_12",
                DisplayText = "Child Container 1-2",
                IsRepeating = true,
            };

            rootContainer.ChildContainers.Add(childContainer);

            for (int i = 1; i <= 3; ++i)
            {
                childContainer.Attributes.Add(new Model.EAVAttribute()
                {
                    Name = String.Format("Attribute 12{0}", i),
                    DataName = String.Format("ATTRIBUTE_12{0}", i),
                    DisplayText = String.Format("Attribute 12{0}", i),
                    IsKey = i == 1,
                    DataType = types.Peek(),
                });

                types.Enqueue(types.Dequeue());
            }

            rootContainer = new Model.EAVRootContainer()
            {
                Name = "Container 2",
                DataName = "CONTAINER_2",
                DisplayText = "Root Container 2",
                IsRepeating = false,
            };

            context.Containers.Add(rootContainer);

            for (int i = 1; i <= 3; ++i)
            {
                rootContainer.Attributes.Add(new Model.EAVAttribute()
                {
                    Name = String.Format("Attribute 2{0}", i),
                    DataName = String.Format("ATTRIBUTE_2{0}", i),
                    DisplayText = String.Format("Attribute 2{0}", i),
                    IsKey = i == 1,
                    DataType = types.Peek(),
                });

                types.Enqueue(types.Dequeue());
            }

            childContainer = new Model.EAVChildContainer()
            {
                Name = "Container 21",
                DataName = "CONTAINER_21",
                DisplayText = "Child Container 2-1",
                IsRepeating = true,
            };

            rootContainer.ChildContainers.Add(childContainer);

            for (int i = 1; i <= 3; ++i)
            {
                childContainer.Attributes.Add(new Model.EAVAttribute()
                {
                    Name = String.Format("Attribute 21{0}", i),
                    DataName = String.Format("ATTRIBUTE_21{0}", i),
                    DisplayText = String.Format("Attribute 21{0}", i),
                    IsKey = i == 1,
                    DataType = types.Peek(),
                });

                types.Enqueue(types.Dequeue());
            }

            return (context);
        }

        private void ctlGoButton_Click(object sender, EventArgs e)
        {
            ctlResults.Text = null;

            var contexts = Util.LoadContexts();

            ctlResults.Text += String.Format("Save Started: {0:HH:mm:ss.fff}\r\n", DateTime.Now);

            var context1 = BuildContext();

            Util.SaveContext(context1);

            ctlResults.Text += String.Format("Create Complete: {0:HH:mm:ss.fff}\r\n", DateTime.Now);

            var context2 = Util.LoadContext(context1.Name);

            context2.MarkDeleted();

            Util.SaveContext(context2);

            ctlResults.Text += String.Format("Delete Complete: {0:HH:mm:ss.fff}\r\n", DateTime.Now);

            ctlResults.Text += "\r\n\r\nDone.\r\n";
        }
    }

    public class Util
    {
        private static HttpClient client;

        static Util()
        {
            client = new HttpClient() { BaseAddress = new Uri("http://localhost:10240") };
        }

        #region Load
        private static void LoadAttributes(Model.EAVContainer container)
        {
            HttpResponseMessage response = client.GetAsync(String.Format("api/meta/containers/{0}/attributes", container.ContainerID)).Result;
            if (response.IsSuccessStatusCode)
            {
                var attributes = response.Content.ReadAsAsync<IEnumerable<Model.EAVAttribute>>().Result;

                foreach (Model.EAVAttribute attribute in attributes)
                {
                    attribute.MarkCreated(attribute);

                    container.Attributes.Add(attribute);
                }
            }
            else
            {
                throw (new ApplicationException("Attempt to get attributes failed."));
            }
        }

        private static void LoadChildContainers(Model.EAVContainer parentContainer)
        {
            HttpResponseMessage response = client.GetAsync(String.Format("api/meta/containers/{0}/containers", parentContainer.ContainerID)).Result;
            if (response.IsSuccessStatusCode)
            {
                var childContainers = response.Content.ReadAsAsync<IEnumerable<Model.EAVChildContainer>>().Result;

                foreach (Model.EAVChildContainer childContainer in childContainers)
                {
                    childContainer.MarkCreated(childContainer);

                    LoadAttributes(childContainer);
                    LoadChildContainers(childContainer);

                    parentContainer.ChildContainers.Add(childContainer);
                }
            }
            else
            {
                throw (new ApplicationException("Attempt to get root containers failed."));
            }
        }

        private static void LoadRootContainers(Model.EAVContext context)
        {
            HttpResponseMessage response = client.GetAsync(String.Format("api/meta/contexts/{0}/containers", context.ContextID)).Result;
            if (response.IsSuccessStatusCode)
            {
                var rootContainers = response.Content.ReadAsAsync<IEnumerable<Model.EAVRootContainer>>().Result;

                foreach (Model.EAVRootContainer rootContainer in rootContainers)
                {
                    rootContainer.MarkCreated(rootContainer);

                    LoadAttributes(rootContainer);
                    LoadChildContainers(rootContainer);

                    context.Containers.Add(rootContainer);
                }
            }
            else
            {
                throw (new ApplicationException("Attempt to get root containers failed."));
            }
        }

        public static Model.EAVContext LoadContext(string name)
        {
            HttpResponseMessage response = client.GetAsync(String.Format("api/meta/contexts/{0}", Uri.EscapeUriString(name))).Result;
            if (response.IsSuccessStatusCode)
            {
                var context = response.Content.ReadAsAsync<Model.EAVContext>().Result;

                context.MarkCreated(context);

                LoadRootContainers(context);

                return (context);
            }
            else
            {
                throw (new ApplicationException("Attempt to get context failed."));
            }
        }

        public static IEnumerable<Model.EAVContext> LoadContexts()
        {
            HttpResponseMessage response = client.GetAsync("api/meta/contexts").Result;
            if (response.IsSuccessStatusCode)
            {
                return (response.Content.ReadAsAsync<IEnumerable<Model.EAVContext>>().Result);
            }
            else
            {
                throw (new ApplicationException("Attempt to get contexts failed."));
            }
        }
        #endregion

        #region Save
        private static void SaveAttribute(Model.EAVAttribute attribute)
        {
            HttpResponseMessage response;

            if (attribute.ObjectState == Model.ObjectState.New)
            {
                response = client.PostAsJsonAsync<EAV.Model.IEAVAttribute>(String.Format("api/meta/containers/{0}/attributes", attribute.Container.ContainerID), attribute).Result;
                if (response.IsSuccessStatusCode)
                {
                    Model.EAVAttribute newAttribute = response.Content.ReadAsAsync<Model.EAVAttribute>().Result;

                    attribute.MarkCreated(newAttribute);
                }
                else
                {
                    throw (new ApplicationException("Attempt to create attribute failed."));
                }
            }
            else if (attribute.ObjectState == Model.ObjectState.Modified)
            {
                response = client.PatchAsJsonAsync<EAV.Model.IEAVAttribute>("api/meta/attributes", attribute).Result;
                if (response.IsSuccessStatusCode)
                {
                }
                else
                {
                    throw (new ApplicationException("Attempt to update attribute failed."));
                }
            }

            if (attribute.ObjectState == Model.ObjectState.Deleted)
            {
                response = client.DeleteAsync(String.Format("api/meta/attributes/{0}", attribute.AttributeID)).Result;
                if (response.IsSuccessStatusCode)
                {
                }
                else
                {
                    throw (new ApplicationException("Attempt to delete attribute failed."));
                }
            }
        }

        private static void SaveChildContainer(Model.EAVChildContainer container)
        {
            HttpResponseMessage response;

            if (container.ObjectState == Model.ObjectState.New)
            {
                response = client.PostAsJsonAsync<EAV.Model.IEAVContainer>(String.Format("api/meta/containers/{0}/containers", container.ParentContainer.ContainerID), container).Result;
                if (response.IsSuccessStatusCode)
                {
                    Model.EAVContainer newContainer = response.Content.ReadAsAsync<Model.EAVChildContainer>().Result;

                    container.MarkCreated(newContainer);
                }
                else
                {
                    throw (new ApplicationException("Attempt to create child container failed."));
                }
            }
            else if (container.ObjectState == Model.ObjectState.Modified)
            {
                response = client.PatchAsJsonAsync<EAV.Model.IEAVContainer>("api/meta/containers", container).Result;
                if (response.IsSuccessStatusCode)
                {
                }
                else
                {
                    throw (new ApplicationException("Attempt to update child container failed."));
                }
            }

            foreach (Model.EAVAttribute attribute in container.Attributes)
            {
                SaveAttribute(attribute);
            }

            foreach (Model.EAVChildContainer childContainer in container.ChildContainers)
            {
                SaveChildContainer(childContainer);
            }

            if (container.ObjectState == Model.ObjectState.Deleted)
            {
                response = client.DeleteAsync(String.Format("api/meta/containers/{0}", container.ContainerID)).Result;
                if (response.IsSuccessStatusCode)
                {
                }
                else
                {
                    throw (new ApplicationException("Attempt to delete child container failed."));
                }
            }
        }

        private static void SaveRootContainer(Model.EAVRootContainer container)
        {
            HttpResponseMessage response;

            if (container.ObjectState == Model.ObjectState.New)
            {
                response = client.PostAsJsonAsync<EAV.Model.IEAVContainer>(String.Format("api/meta/contexts/{0}/containers", container.Context.ContextID), container).Result;
                if (response.IsSuccessStatusCode)
                {
                    Model.EAVContainer newContainer = response.Content.ReadAsAsync<Model.EAVRootContainer>().Result;

                    container.MarkCreated(newContainer);
                }
                else
                {
                    throw (new ApplicationException("Attempt to create root container failed."));
                }
            }
            else if (container.ObjectState == Model.ObjectState.Modified)
            {
                response = client.PatchAsJsonAsync<EAV.Model.IEAVContainer>("api/meta/containers", container).Result;
                if (response.IsSuccessStatusCode)
                {
                }
                else
                {
                    throw (new ApplicationException("Attempt to update root container failed."));
                }
            }

            foreach (Model.EAVAttribute attribute in container.Attributes)
            {
                SaveAttribute(attribute);
            }

            foreach (Model.EAVChildContainer childContainer in container.ChildContainers)
            {
                SaveChildContainer(childContainer);
            }

            if (container.ObjectState == Model.ObjectState.Deleted)
            {
                response = client.DeleteAsync(String.Format("api/meta/containers/{0}", container.ContainerID)).Result;
                if (response.IsSuccessStatusCode)
                {
                }
                else
                {
                    throw (new ApplicationException("Attempt to delete root container failed."));
                }
            }
        }

        public static void SaveContext(Model.EAVContext context)
        {
            HttpResponseMessage response;

            if (context.ObjectState == Model.ObjectState.New)
            {
                response = client.PostAsJsonAsync<EAV.Model.IEAVContext>("api/meta/contexts", context).Result;
                if (response.IsSuccessStatusCode)
                {
                    context.MarkCreated(response.Content.ReadAsAsync<Model.EAVContext>().Result);
                }
                else
                {
                    throw (new ApplicationException("Attempt to create context failed."));
                }
            }
            else if (context.ObjectState == Model.ObjectState.Modified)
            {
                response = client.PatchAsJsonAsync<EAV.Model.IEAVContext>("api/meta/contexts", context).Result;
                if (response.IsSuccessStatusCode)
                {
                }
                else
                {
                    throw (new ApplicationException("Attempt to update context failed."));
                }
            }

            foreach (Model.EAVRootContainer rootContainer in context.Containers)
            {
                SaveRootContainer(rootContainer);
            }

            if (context.ObjectState == Model.ObjectState.Deleted)
            {
                response = client.DeleteAsync(String.Format("api/meta/contexts/{0}", context.ContextID)).Result;
                if (response.IsSuccessStatusCode)
                {
                }
                else
                {
                    throw (new ApplicationException("Attempt to delete context failed."));
                }
            }
        }
        #endregion
    }
}
