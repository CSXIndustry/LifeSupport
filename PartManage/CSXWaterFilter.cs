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
using CSXUniversalLibrary;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSXIndustry.LifeSupport.PartManage
{
    public class CSXWaterFilter : CSXFilterModule
    {
        [KSPField(guiActive = true, guiName = "Filter Efficiency", guiUnits = "%")]
        public float filterEfficiency = 0.998f;

        private const float efficiencyMax = (24 * 3600) * 30;
        private float efficiencyCount = efficiencyMax;

        public CSXWaterFilter()
        {
            this.FilterType = CSXFilterTypes.WaterFilter;
        } // End Init

        public override void UpdatePart(float fixedDeltaTime)
        {
            if (isWorking)
                if(IsPowered(fixedDeltaTime))
                {
                    if (!UpdateFilter(fixedDeltaTime))
                        DeactivateFilter();
                    else
                    {
                        efficiencyCount -= 1.0f * fixedDeltaTime;
                        if (efficiencyCount < 0)
                        {
                            filterEfficiency *= 0.998f;
                            efficiencyCount = efficiencyMax;
                        }
                    }
                }
        } // End Update Part

        private bool UpdateFilter(float fixedDeltaTime)
        {
            float waterAcquired = part.RequestResource(CSXResources.byWater, 1.0f * filterRate * fixedDeltaTime);

            if(waterAcquired > 0)
                part.RequestResource(CSXResources.pureWater, -waterAcquired * filterEfficiency * fixedDeltaTime);

            waterAcquired = part.RequestResource(CSXResources.wasteWater, 1.0f * filterRate * fixedDeltaTime);

            if (waterAcquired > 0)
                part.RequestResource(CSXResources.pureWater, -waterAcquired * filterEfficiency * fixedDeltaTime);

            return true;
        } // End Update Filter
    }
}
