using System.Collections;
using UnityEngine;

namespace Unity.Cloud.UserReporting.Plugin
{
    /// <summary>
    /// Helps with updating the Unity User Reporting client.
    /// </summary>
    public class UnityUserReportingUpdater : IEnumerator
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="UnityUserReportingUpdater"/> class.
        /// </summary>
        public UnityUserReportingUpdater()
        {
            this.waitForEndOfFrame = new WaitForEndOfFrame();
        }

        #endregion

        #region Fields

        private int step;

        private WaitForEndOfFrame waitForEndOfFrame;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the current item.
        /// </summary>
        public object Current { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Moves to the next item.
        /// </summary>
        /// <returns>A value indicating whether the move was successful.</returns>
        public bool MoveNext()
        {
            if (this.step == 0)
            {
                UnityUserReporting.CurrentClient.Update();
                this.Current = null;
                this.step = 1;
                return true;
            }
            if (this.step == 1)
            {
                this.Current = this.waitForEndOfFrame;
                this.step = 2;
                return true;
            }
            if (this.step == 2)
            {
                UnityUserReporting.CurrentClient.UpdateOnEndOfFrame();
                this.Current = null;
                this.step = 3;
                return false;
            }
            return false;
        }

        /// <summary>
        /// Resets the updater.
        /// </summary>
        public void Reset()
        {
            this.step = 0;
        }

        #endregion
    }
}