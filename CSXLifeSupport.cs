/************************************************************************
 * CSX Industry - Life Support Part+Plugin Pack for Kerbal Space Program*
 *                                                                      *
 * Initial Alpha Release Version 0.7a                                   *
 *                                                                      *
 * Created by Charlie S.                                                *
 * Built on June 13th, 2015                                             *
 * Initial Built on July 12th, 2014                                     *
 ************************************************************************/

using UnityEngine;
using KSP.IO;

using CSXIndustry.LifeSupport.CrewManage;
using CSXIndustry.LifeSupport.PartManage;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSXIndustry.LifeSupport
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class CSXLifeSupport : MonoBehaviour
    {
        private Vessel activeVessel;
        private CSXCrewManagement crews;
        private CSXPartManagement parts;

        public void Start()
        {
            activeVessel = FlightGlobals.ActiveVessel;
            if (activeVessel.GetCrewCount() > 0)
                crews = new CSXCrewManagement(activeVessel);
            if (activeVessel != null)
                parts = new CSXPartManagement(activeVessel);

            Debug.Log("[CSX Industry] Parts in total: " + parts.PartLength());
        }

        public void FixedUpdate()
        {
            float delta = TimeWarp.fixedDeltaTime;
            //crews.FixedUpdate(this, delta);
            parts.FixedUpdate(delta);
        }

        public Part GetPartWithResource(string resourceName)
        {
            foreach (Part part in activeVessel.parts)
                foreach (PartResource resource in part.Resources)
                    if (resource.resourceName == resourceName && resource.amount > 0)
                        return part;

            return null;
        }

        public float RequestResource(string resourceName, float amount)
        {
            foreach (Part part in activeVessel.parts)
                foreach (PartResource resource in part.Resources)
                    if (resource.resourceName == resourceName)
                        return part.RequestResource(resourceName, amount);

            return 0.0f;
        }

        public void KillKerman(ProtoCrewMember crew, string reason)
        {
            ScreenMessages.PostScreenMessage(crew.name + " has died due to " + reason, 5.0f, ScreenMessageStyle.UPPER_CENTER);
            crew.KerbalRef.die();
            activeVessel.rootPart.RemoveCrewmember(crew);
        }
    }
}
