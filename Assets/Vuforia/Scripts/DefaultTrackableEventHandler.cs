/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;

namespace Vuforia
{
    /// <summary>
    /// A custom handler that implements the ITrackableEventHandler interface.
    /// </summary>
    public class DefaultTrackableEventHandler : MonoBehaviour,
                                                ITrackableEventHandler
   
    {

        public GameObject TriggerManager = null;
        #region PRIVATE_MEMBER_VARIABLES
        private TrackableBehaviour mTrackableBehaviour;
    
        #endregion // PRIVATE_MEMBER_VARIABLES



        #region UNTIY_MONOBEHAVIOUR_METHODS
    
        void Start()
        {

            mTrackableBehaviour = GetComponent<TrackableBehaviour>();
            if (mTrackableBehaviour)
            {
                mTrackableBehaviour.RegisterTrackableEventHandler(this);
            }
        }

        #endregion // UNTIY_MONOBEHAVIOUR_METHODS



        #region PUBLIC_METHODS


        /// <summary>
        /// Implementation of the ITrackableEventHandler function called when the
        /// tracking state changes.
        /// </summary>
        public void OnTrackableStateChanged(
                                        TrackableBehaviour.Status previousStatus,
                                        TrackableBehaviour.Status newStatus)
        {
            if (newStatus == TrackableBehaviour.Status.DETECTED ||
                newStatus == TrackableBehaviour.Status.TRACKED ||
                newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
            {
                OnTrackingFound(previousStatus, newStatus);
            }
            else if (newStatus == TrackableBehaviour.Status.NOT_FOUND)
            {
                OnTrackingLost(previousStatus, newStatus);
            }
        }

        public void RemoveFromAR()
        {
            mTrackableBehaviour.enabled = false;
            //TrackerManager.Instance.GetStateManager().GetTrackableBehaviours();
          // TrackerManager.Instance.GetStateManager().//GetTracker<ObjectTracker>().Stop();
            // mTrackableBehaviour.UnregisterTrackableEventHandler(this);
        }

        public void ReaddToAR()
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }


        #endregion // PUBLIC_METHODS



        #region PRIVATE_METHODS


        private void OnTrackingFound(
                                        TrackableBehaviour.Status previousStatus,
                                        TrackableBehaviour.Status newStatus)
        {
            if (TriggerManager == null)
            {
                TriggerManager = GameObject.FindGameObjectWithTag("TriggerManager");
            }
            if (newStatus == TrackableBehaviour.Status.DETECTED || newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
            {
                TriggerManager.GetComponent<TriggerObjectManagement>().ObjectTriggered(this.gameObject);
            }// ||
             // newStatus == TrackableBehaviour.Status.TRACKED ||
             //newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

            // Enable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = true;
            }

            // Enable colliders:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = true;
            }

            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
        }


        private void OnTrackingLost(
                                        TrackableBehaviour.Status previousStatus,
                                        TrackableBehaviour.Status newStatus)
        {
            if (TriggerManager == null)
            {
                TriggerManager = GameObject.FindGameObjectWithTag("TriggerManager");
            }
            if (previousStatus == TrackableBehaviour.Status.DETECTED || 
                previousStatus == TrackableBehaviour.Status.TRACKED ||
                previousStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
            {
                TriggerManager.GetComponent<TriggerObjectManagement>().ObjectLost(this.gameObject);
            }
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

            // Disable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = false;
            }

            // Disable colliders:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = false;
            }

            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
        }

        #endregion // PRIVATE_METHODS
    }
}
