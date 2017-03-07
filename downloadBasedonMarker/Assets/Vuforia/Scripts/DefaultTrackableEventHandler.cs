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
        #region PRIVATE_MEMBER_VARIABLES
 
        private TrackableBehaviour mTrackableBehaviour;
        public static string associateName;
        public GameObject[] contentOBJ = new GameObject[2];
        #endregion // PRIVATE_MEMBER_VARIABLES

        /*public string AssoName
        {
            get { return associateName; }
            set { associateName = value; }

        }*/

        #region UNTIY_MONOBEHAVIOUR_METHODS
        void OnEnable()
        {

        }
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
                OnTrackingFound();
            }
            else
            {
                OnTrackingLost();
            }
        }

        #endregion // PUBLIC_METHODS



        #region PRIVATE_METHODS


        private void OnTrackingFound()
        {
            if(mTrackableBehaviour.TrackableName == "marker_1")//design
            {
                associateName = mTrackableBehaviour.TrackableName;
                contentOBJ[0].gameObject.SetActive(true);
                contentOBJ[1].gameObject.SetActive(false);
                #region TRACKING_ALGO
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
                #endregion //TRACKING_ALO

            }
            else if(mTrackableBehaviour.TrackableName == "marker_2")//medieval
            {
                associateName = mTrackableBehaviour.TrackableName;
                contentOBJ[0].gameObject.SetActive(false);
                contentOBJ[1].gameObject.SetActive(true);
                #region TRACKING_ALGO
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
                #endregion //TRACKING_ALO
            }

        }


        private void OnTrackingLost()
        {
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
