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
    public class CSXThermalControlModule : CSXControlModule
    {
        [KSPField(guiActive = true, guiName = "Ammonium Quality", guiUnits = "%")]
        private float nhQuality = 1.0f;

        [KSPField(guiActive = true, guiName = "Ammonium Quantity", guiUnits = "%")]
        private float nhQuantity = 1.0f;

        private float qualityTimeMax = (24 * 3600) * (30 * 6);
        private float qualityTime = 0;

        private float quantityTimeMax = (24 * 3600) * (30 * 12);
        private float quantityTime = 0;

        public CSXThermalControlModule()
        {
            this.ControllerType = CSXControllerTypes.Thermal;
        }

        public override void UpdatePart(float fixedDeltaTime)
        {
            CheckStatus();

            if (isWorking)
                if (IsPowered(fixedDeltaTime))
                {
                    qualityTime += 1.0f * fixedDeltaTime;
                    quantityTime += 1.0f * fixedDeltaTime;

                    if (qualityTime > qualityTimeMax)
                    {
                        nhQuality *= 0.991f;
                        qualityTime = 0;
                    }

                    if (quantityTime > quantityTimeMax)
                    {
                        nhQuantity *= 0.998f;
                        quantityTime = 0;
                    }
                }
        }

        private bool UpdateThermalControl(float fixedDeltaTime)
        {
            return true;
        }

        private void CheckStatus()
        {
            if (GetAnimation().Count > 0)
                foreach (ModuleAnimateGeneric anim in GetAnimation())
                    if (anim.animationName == animName)
                    {
                        if (anim.animTime == 0)
                        {
                            isWorking = false;
                            status = "Idle";
                        }
                        else if (anim.animTime == 1)
                        {
                            isWorking = true;
                            status = "Controlling";
                        }
                    }
        }
    }
}
