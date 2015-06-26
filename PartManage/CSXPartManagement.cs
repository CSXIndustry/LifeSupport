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
    public class CSXPartManagement
    {
        List<Part> list;

        public CSXPartManagement(Vessel vessel)
        {
            list = new List<Part>();
            Debug.Log("Total parts: " + vessel.parts.Count);

            foreach(Part part in vessel.Parts)
                foreach(PartModule module in part.Modules)
                {
                    CSXPartModule type = GetModule(module);

                    if (type != null && type.PartType != CSXPartTypes.NONE && !list.Contains(part))
                    {
                        list.Add(part);
                        Debug.Log("[CSX Industry] " + part.name + " has been added to part manifest");
                    }
                    else if (type != null && !list.Contains(part) && type.PartType == CSXPartTypes.NONE)
                        Debug.Log("[CSX Industry] Skipping part " + part.name);

                }
        } // End Initialize

        public void FixedUpdate(float fixedDeltaTime)
        {
            foreach(Part part in list)
                foreach(PartModule module in part.Modules)
                {
                    CSXPartModule moduleType = GetModule(module);
                    if (moduleType != null)
                    {
                        if (moduleType.PartType == CSXPartTypes.PowerSource)
                        {
                            CSXPowerSource sourceType = (CSXPowerSource)moduleType;
                            if (sourceType.SourceType == CSXSourceTypes.FuelCell)
                            {
                                CSXFuelCell fuelCell = (CSXFuelCell)sourceType;
                                fuelCell.UpdatePart(fixedDeltaTime);
                            }
                        } // End Power Source
                        else if (moduleType.PartType == CSXPartTypes.FilterModule)
                        {
                            CSXFilterModule filterType = (CSXFilterModule)moduleType;
                            if (filterType.FilterType == CSXFilterTypes.KarbonFilter)
                            {
                                CSXKarbonFilter karbonFilter = (CSXKarbonFilter)filterType;
                                karbonFilter.UpdatePart(fixedDeltaTime);
                            }
                            else if (filterType.FilterType == CSXFilterTypes.WaterFilter)
                            {
                                CSXWaterFilter waterFilter = (CSXWaterFilter)filterType;
                                waterFilter.UpdatePart(fixedDeltaTime);
                            }
                            else if (filterType.FilterType == CSXFilterTypes.Electrolyte)
                            {
                                CSXElectrolyteModule electrolyte = (CSXElectrolyteModule)filterType;
                                electrolyte.UpdatePart(fixedDeltaTime);
                            }
                        } // End Filter Type
                        else if (moduleType.PartType == CSXPartTypes.ControlModule)
                        {
                            CSXControlModule controlType = (CSXControlModule)moduleType;
                            if (controlType.ControllerType == CSXControllerTypes.Thermal)
                            {
                                CSXThermalControlModule thermalControl = (CSXThermalControlModule)controlType;
                                thermalControl.UpdatePart(fixedDeltaTime);
                            }
                        } // End Controller Type
                        else if(moduleType.PartType == CSXPartTypes.HabitatModule)
                        {
                            CSXHabitatModule habitatType = (CSXHabitatModule)moduleType;
                            if(habitatType.HabitatType == CSXHabitateTypes.GreenHouse)
                            {
                                CSXGreenHouseModule greenHouse = (CSXGreenHouseModule)habitatType;
                                greenHouse.UpdatePart(fixedDeltaTime);
                            }
                        } // End Habitat Module
                    } // End For each part module loop
                } // End For each part loop
        } // End Fixed Update

        private CSXPartModule GetModule(PartModule module)
        {
            try
            {
                return (CSXPartModule)module;
            }
            catch(Exception e)
            {
                e.ToString();
                return null;
            }
        } // End Get Module

        public int PartLength()
        {
            return list.Count;
        }
    }
}
