using Genetec.Sdk.Workspace.ContextualAction;
using OfficerMapObject.MapObjects.Officers;
using System;
using System.Windows.Media.Imaging;
using System.Threading;
using System.Threading.Tasks;

namespace OfficerMapObject
{
    public sealed class addOfficer 
    {

        #region Public Properties

        /// <summary>
        /// Gets the action's unique identifier
        /// </summary>

        /// <summary>
        /// Gets the priority of the component, lowest is better
        /// </summary>

        #endregion Public Properties

        #region Public Constructors

        #endregion Public Constructors

        #region Public Methods


        public void  add()
        {
            for (int i = 0; i < 10; i++)
            {
                // You can adjust the coordinates as needed
                var officer = new MapObjects.Officers.OfficerMapObject(100 * i + 20, 100 * i + 20);
                OfficerMapObjectProvider.AddOfficer(officer);
            }
            // Introduce a delay of 3 seconds
            Task.Run(async () =>
            {
                await Task.Delay(3000); // 10 seconds in milliseconds
                OfficerMapObjectProvider.DeleteLatestOfficer();
                await Task.Delay(3000); // 10 seconds in milliseconds
                OfficerMapObjectProvider.DeleteLatestOfficer();
                await Task.Delay(3000); // 10 seconds in milliseconds
                OfficerMapObjectProvider.DeleteLatestOfficer();

            });
        }


        // Callback function for the timer


        #endregion Public Methods
    }
}
