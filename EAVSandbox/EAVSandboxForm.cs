using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows.Forms;

using EAVFramework.Model;


namespace EAVSandbox
{
    public partial class EAVSandboxForm : Form
    {
        private EAVServiceClient.EAVClient eavClient = new EAVServiceClient.EAVClient();

        public EAVSandboxForm()
        {
            InitializeComponent();
        }

        private EAVContext BuildContext()
        {
            Queue<EAV.Model.EAVDataType> types = new Queue<EAV.Model.EAVDataType>(Enum.GetValues(typeof(EAV.Model.EAVDataType)).OfType<EAV.Model.EAVDataType>());

            EAVContext context = new EAVContext()
            {
                Name = "Test",
                DataName = "TEST",
                DisplayText = "Test Context",
            };

            EAVRootContainer rootContainer;
            EAVChildContainer childContainer;

            rootContainer = new EAVRootContainer()
            {
                Name = "Container 1",
                DataName = "CONTAINER_1",
                DisplayText = "Root Container 1",
                IsRepeating = false,
            };

            context.Containers.Add(rootContainer);

            for (int i = 1; i <= 3; ++i)
            {
                rootContainer.Attributes.Add(new EAVAttribute()
                {
                    Name = String.Format("Attribute 1{0}", i),
                    DataName = String.Format("ATTRIBUTE_1{0}", i),
                    DisplayText = String.Format("Attribute 1{0}", i),
                    IsKey = i == 1,
                    DataType = types.Peek(),
                });

                types.Enqueue(types.Dequeue());
            }

            childContainer = new EAVChildContainer()
            {
                Name = "Container 11",
                DataName = "CONTAINER_11",
                DisplayText = "Child Container 1-1",
                IsRepeating = false,
            };

            rootContainer.ChildContainers.Add(childContainer);

            for (int i = 1; i <= 3; ++i)
            {
                childContainer.Attributes.Add(new EAVAttribute()
                {
                    Name = String.Format("Attribute 11{0}", i),
                    DataName = String.Format("ATTRIBUTE_11{0}", i),
                    DisplayText = String.Format("Attribute 11{0}", i),
                    IsKey = i == 1,
                    DataType = types.Peek(),
                });

                types.Enqueue(types.Dequeue());
            }

            childContainer = new EAVChildContainer()
            {
                Name = "Container 12",
                DataName = "CONTAINER_12",
                DisplayText = "Child Container 1-2",
                IsRepeating = true,
            };

            rootContainer.ChildContainers.Add(childContainer);

            for (int i = 1; i <= 3; ++i)
            {
                childContainer.Attributes.Add(new EAVAttribute()
                {
                    Name = String.Format("Attribute 12{0}", i),
                    DataName = String.Format("ATTRIBUTE_12{0}", i),
                    DisplayText = String.Format("Attribute 12{0}", i),
                    IsKey = i == 1,
                    DataType = types.Peek(),
                });

                types.Enqueue(types.Dequeue());
            }

            rootContainer = new EAVRootContainer()
            {
                Name = "Container 2",
                DataName = "CONTAINER_2",
                DisplayText = "Root Container 2",
                IsRepeating = false,
            };

            context.Containers.Add(rootContainer);

            for (int i = 1; i <= 3; ++i)
            {
                rootContainer.Attributes.Add(new EAVAttribute()
                {
                    Name = String.Format("Attribute 2{0}", i),
                    DataName = String.Format("ATTRIBUTE_2{0}", i),
                    DisplayText = String.Format("Attribute 2{0}", i),
                    IsKey = i == 1,
                    DataType = types.Peek(),
                });

                types.Enqueue(types.Dequeue());
            }

            childContainer = new EAVChildContainer()
            {
                Name = "Container 21",
                DataName = "CONTAINER_21",
                DisplayText = "Child Container 2-1",
                IsRepeating = true,
            };

            rootContainer.ChildContainers.Add(childContainer);

            for (int i = 1; i <= 3; ++i)
            {
                childContainer.Attributes.Add(new EAVAttribute()
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
            HttpClient client = new HttpClient() { BaseAddress = new Uri("http://localhost:10240") };

            ctlResults.Text = null;

            // Do this to cause our web service to initialize
            var contexts = eavClient.LoadContexts(client);

            ctlResults.Text += String.Format("Save Started: {0:HH:mm:ss.fff}\r\n", DateTime.Now);

            // Build and save a context
            var context1 = BuildContext();

            eavClient.SaveMetadata(client, context1);

            ctlResults.Text += String.Format("Create Complete: {0:HH:mm:ss.fff}\r\n", DateTime.Now);

            // Reload contexts (should be at least one now)
            contexts = eavClient.LoadContexts(client);

            var context2 = contexts.First();

            // Get the associated metadata
            eavClient.LoadMetadata(client, context2);

            // Mark deleted
            context2.MarkDeleted();

            // Delete it
            eavClient.SaveMetadata(client, context2);

            ctlResults.Text += String.Format("Delete Complete: {0:HH:mm:ss.fff}\r\n", DateTime.Now);

            ctlResults.Text += "\r\n\r\nDone.\r\n";
        }
    }
}
