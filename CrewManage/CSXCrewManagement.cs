/************************************************************************
 * CSX Industry - Life Support Part+Plugin Pack for Kerbal Space Program*
 *                                                                      *
 * Initial Alpha Release Version 0.6a                                   *
 *                                                                      *
 * Created by Charlie S.                                                *
 * Built on August 12th, 2014                                           *
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

namespace CSXIndustry.LifeSupport.CrewManage
{
    public class CSXCrewManagement
    {
        private List<CSXCrew> crews;

        public CSXCrewManagement(Vessel vessel)
        {
            crews = new List<CSXCrew>();

            foreach (ProtoCrewMember crew in vessel.GetVesselCrew())
                crews.Add(new CSXCrew(crew));
        }// End Init

        public void FixedUpdate(CSXLifeSupport lifeSupport, float fixedDeltaTime)
        {
            foreach(CSXCrew crew in crews)
            {
                crew.UpdateCrew(fixedDeltaTime);

                float inhaled = 0;
                if (lifeSupport.GetPartWithResource(CSXResources.oxygen) != null)
                    inhaled = lifeSupport.RequestResource(CSXResources.oxygen, 1.0f * fixedDeltaTime);

                lifeSupport.RequestResource(CSXResources.koo, -inhaled);

                float toxicLevel = inhaled / (1.0f * fixedDeltaTime);
                crew.SetKillTimer(crew.KillTimer - (100 - (100 * toxicLevel)));

                if(!crew.IsDead && crew.KillTimer < 0)
                {
                    lifeSupport.KillKerman(crew.CrewData, "Low oxygen level");
                    crew.IsDead = true;
                }

                if(!crew.HasCleaned)
                {
                    lifeSupport.RequestResource(CSXResources.waste, -(crew.LastEaten * 0.5f) * fixedDeltaTime);
                    lifeSupport.RequestResource(CSXResources.wasteWater, -(crew.LastEaten * 0.5f) * fixedDeltaTime);

                    crew.HasCleaned = true;
                    crew.WasteTimer = 0;
                }

                if(!crew.HasEaten)
                {
                    float food = lifeSupport.RequestResource(CSXResources.food, 1.0f * fixedDeltaTime);
                    float water = lifeSupport.RequestResource(CSXResources.byWater, 1.0f * fixedDeltaTime);
                    float factor = (food + water) / (2.0f * fixedDeltaTime);

                    crew.LastEaten = food + water;

                    crew.KillTimer = factor;

                    crew.HasEaten = true;
                    crew.EatTimer = 0;
                }

                if(!crew.IsDead && crew.KillTimer < 0)
                {
                    lifeSupport.KillKerman(crew.CrewData, "Hunger");
                    crew.IsDead = true;
                }

                if (crew.IsDead)
                    crews.Remove(crew);
            }
        } // End FixedUpdate
    }
}
