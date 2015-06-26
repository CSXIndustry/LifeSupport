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
    public class CSXGreenHouseModule : CSXHabitatModule
    {
        [KSPField]
        public float growRate = 1.0f;

        private float tick = 0;
        private float tickMax = 120f;

        public CSXGreenHouseModule()
        {
            this.HabitatType = CSXHabitateTypes.GreenHouse;
        }

        public override void UpdatePart(float fixedDeltaTime)
        {
            if (isWorking)
                if (IsPowered(fixedDeltaTime))
                    if (!UpdateGreenHouse(fixedDeltaTime))
                        DeactivateModule();
        }

        private bool UpdateGreenHouse(float fixedDeltaTime)
        {
            float water = part.RequestResource(CSXResources.pureWater, waterUsage * fixedDeltaTime) / (waterUsage * fixedDeltaTime);
            float koo = part.RequestResource(CSXResources.koo, kooUsage * fixedDeltaTime) / (kooUsage * fixedDeltaTime);
            float heat = part.RequestResource(CSXResources.heat, heatUsage * fixedDeltaTime) / (heatUsage * fixedDeltaTime);

            float factor = water * ((koo + heat) / 2.0f);

            float growRateActual = 0.0f;
            growRateActual = growRate * factor;

            part.RequestResource(CSXResources.oxygen, -kooUsage * koo * fixedDeltaTime);

            if (growRateActual > 0)
            {
                tick += growRateActual;
                if(tick > tickMax)
                {
                    tick = 0;
                    part.RequestResource(CSXResources.food, -foodRate * fixedDeltaTime);
                }

                return true;
            }
            else return false;
        }
    }
}
