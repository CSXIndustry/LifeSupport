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
    public class CSXKarbonFilter : CSXFilterModule
    {
        private bool convertTo = true;

        public CSXKarbonFilter()
        {
            this.filterType = CSXFilterTypes.KarbonFilter;
        }

        public override void UpdatePart(float fixedDeltaTime)
        {
            if (isWorking)
                if (IsPowered(fixedDeltaTime))
                    if (!FilterKOO(fixedDeltaTime))
                        DeactivateFilter();
        }

        private bool FilterKOO(float fixedDeltaTime)
        {
            float koo = (float)part.RequestResource(CSXResources.koo, 0.1f * filterRate * fixedDeltaTime) / (0.1f * filterRate * fixedDeltaTime);
            float hydrogen = (float)part.RequestResource(CSXResources.hydrogen, 0.4f * filterRate * fixedDeltaTime, ResourceFlowMode.NO_FLOW) / (0.4f * filterRate * fixedDeltaTime);
            float heat = (float)part.RequestResource(CSXResources.heat, 8.0f * filterRate * fixedDeltaTime) / (8.0f * filterRate * fixedDeltaTime);

            float factorFinal = koo * hydrogen * heat;

            part.RequestResource(CSXResources.byWater, -0.2 * factorFinal * filterRate * fixedDeltaTime);

            if (convertTo)
                part.RequestResource(CSXResources.kethane, -0.1 * factorFinal * filterRate * fixedDeltaTime);
            else
                part.RequestResource(CSXResources.karbonite, -0.1 * factorFinal * filterRate * fixedDeltaTime);

            if (factorFinal > 0)
                return true;
            else return false;
        }

        [KSPEvent(guiActive = true, guiName = "Convert to Kethane")]
        public void ToKethane()
        {
            ScreenMessages.PostScreenMessage("Filter produces Kethane", 5.0f, ScreenMessageStyle.UPPER_CENTER);

            Events["ToKarbonite"].active = true;
            Events["ToKethane"].active = false;
            convertTo = true;
        }

        [KSPEvent(guiActive = true, guiName = "Convert to Karbonite", active = false)]
        public void ToKarbonite()
        {
            ScreenMessages.PostScreenMessage("Filter produces Karbonite", 5.0f, ScreenMessageStyle.UPPER_CENTER);

            Events["ToKarbonite"].active = false;
            Events["ToKethane"].active = true;

            convertTo = false;
        }
    }
}
