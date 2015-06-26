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
    public class CSXElectrolyteModule : CSXFilterModule
    {
        public CSXElectrolyteModule()
        {
            this.FilterType = CSXFilterTypes.Electrolyte;
        }

        public override void UpdatePart(float fixedDeltaTime)
        {
            if (isWorking)
                if (IsPowered(fixedDeltaTime))
                    if (!PerformReaction(fixedDeltaTime))
                        DeactivateFilter();
        }

        private bool PerformReaction(float fixedDeltaTime)
        {
            float water = part.RequestResource(CSXResources.pureWater, 3.0f * filterRate * fixedDeltaTime);

            if (water > 0)
            {
                part.RequestResource(CSXResources.hydrogen, -2.0f * filterRate * fixedDeltaTime);
                part.RequestResource(CSXResources.oxygen, -1.0f * filterRate * fixedDeltaTime);
                return true;
            }
            else return false;
        }
    }
}
