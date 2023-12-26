using Genetec.Sdk.Workspace.ContextualAction;
using OfficerMapObject.MapObjects.Officers;
using System;
using System.Windows.Media.Imaging;
using System.Threading;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows;

namespace OfficerMapObject
{
    public sealed class AddOfficerContextualAction : ContextualAction
    {

        #region Public Properties

        /// <summary>
        /// Gets the action's unique identifier
        /// </summary>
        public override Guid Id => new Guid("{F2AA03A6-6C73-4DEE-8B20-AA05E19A2AAB}");

        /// <summary>
        /// Gets the priority of the component, lowest is better
        /// </summary>
        public override int Priority => 0;

        #endregion Public Properties

        #region Public Constructors

        public AddOfficerContextualAction()
        {
            Name = "Show Objects";
            Icon = new BitmapImage(new Uri(@"pack://application:,,,/OfficerMapObject;Component/Resources/Officer.png", UriKind.RelativeOrAbsolute));

        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Return true if the contextual action can execute
        /// </summary>
        /// <param name="context">Current action's context</param>
        public void add()
        {
            
                // You can adjust the coordinates as needed
            var officer = new MapObjects.Officers.OfficerMapObject(100 , 100);
            OfficerMapObjectProvider.AddOfficer(officer);
            
            // Introduce a delay of 3 seconds
            Task.Run(async () =>
            {
                await Task.Delay(3000); // 10 seconds in milliseconds
                OfficerMapObjectProvider.DeleteLatestOfficer();
                await Task.Delay(3000); // 10 seconds in milliseconds

            });
        }
        public override bool CanExecute(ContextualActionContext context) => context is MapContextualActionContext;

        /// <summary>
        /// Executes the contextual action
        /// </summary>
        /// <param name="context">Current action's context</param>

        public override bool Execute(ContextualActionContext context)
        {
                if (!(context is MapContextualActionContext mapContext)) return false;

                add();
                return false;
            }
        }


        // Callback function for the timer


        #endregion Public Methods
    }

