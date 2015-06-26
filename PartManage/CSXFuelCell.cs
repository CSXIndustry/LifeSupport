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
    public class CSXFuelCell : CSXPowerSource
    {
        [KSPField]
        public float reactionRate;
        [KSPField]
        public float dissipationRate;
        [KSPField]
        public float coolingRate;

        [KSPField(guiActive = true, guiName = "Cell Temperature", guiUnits = "C", guiFormat = "#,000")]
        private float cellTemp = 0f;

        private float extTemp;

        private bool cooling;

        public CSXFuelCell()
        {
            this.SourceType = CSXSourceTypes.FuelCell;
        }

        public override void UpdatePart(float fixedDeltaTime)
        {
            extTemp = (float) this.part.externalTemperature;
            cellTemp = (float) this.part.temperature;

            UpdateCell(fixedDeltaTime);

            if (!UpdateCooling(fixedDeltaTime) && cooling)
                DeactivateCooling();

            this.part.temperature = cellTemp;
        }

        private bool UpdateCell(float fixedDeltaTime)
        {
            if (isWorking)
            {
                float oxygen = (float)part.RequestResource(CSXResources.oxygen, (1.0f * reactionRate) * fixedDeltaTime, ResourceFlowMode.NO_FLOW) / (1.0f * reactionRate * fixedDeltaTime);
                float hydrogen = (float)part.RequestResource(CSXResources.hydrogen, (2.0f * reactionRate) * fixedDeltaTime, ResourceFlowMode.NO_FLOW) / (2.0f * reactionRate * fixedDeltaTime);

                part.RequestResource(CSXResources.byWater, (-1.0 * reactionRate) * (oxygen * hydrogen) * fixedDeltaTime, ResourceFlowMode.NO_FLOW);

                if (GetChargeRequired().Count > 0)
                    foreach (Part required in GetChargeRequired())
                        required.RequestResource(CSXResources.power, ((-0.24 * reactionRate * (oxygen * hydrogen)) / (float)GetChargeRequired().Count) * fixedDeltaTime);

                part.RequestResource(CSXResources.heat, (-10.0 * reactionRate) * fixedDeltaTime, ResourceFlowMode.NO_FLOW);
                if (part.Resources[CSXResources.heat].amount >= part.Resources[CSXResources.heat].maxAmount)
                    cellTemp = Mathf.Lerp(cellTemp, cellTemp + 10.0f, dissipationRate * fixedDeltaTime);

                return true;
            }

            else return false;
        } // End Update Cell

        private bool UpdateCooling(float fixedDeltaTime)
        {
            if (cooling && part.temperature > extTemp)
            {
                float water = (float)part.RequestResource(CSXResources.byWater, (2.0f * reactionRate) * fixedDeltaTime, ResourceFlowMode.NO_FLOW) / (2.0f * reactionRate * fixedDeltaTime);
                cellTemp = Mathf.Lerp(cellTemp, extTemp, coolingRate * water * fixedDeltaTime);
                return true;
            }
            else if (!isWorking && !cooling && part.temperature > extTemp)
            {
                cellTemp = Mathf.Lerp(cellTemp, extTemp, dissipationRate * fixedDeltaTime);
                return false;
            }
            else if (part.temperature <= extTemp)
            {
                cellTemp = Mathf.Lerp(cellTemp, extTemp, dissipationRate * fixedDeltaTime);
                return false;
            }
            else
            {
                return false;
            }
        } // End Update Cooling

        [KSPEvent(guiActive = true, guiName = "Activate Cooling")]
        private void ActivateCooling()
        {
            ScreenMessages.PostScreenMessage("Fuel Cell Cooling Circulating", 5.0f, ScreenMessageStyle.UPPER_CENTER);

            Events["ActivateCooling"].active = false;
            Events["DeactivateCooling"].active = true;

            cooling = true;
        }

        [KSPEvent(guiActive = true, guiName = "Deactivate Cooling", active = false)]
        private void DeactivateCooling()
        {
            ScreenMessages.PostScreenMessage("Fuel Cell Cooling Off", 5.0f, ScreenMessageStyle.UPPER_CENTER);

            Events["ActivateCooling"].active = true;
            Events["DeactivateCooling"].active = false;

            cooling = false;
        }

    } // End Fuel Cell
}
