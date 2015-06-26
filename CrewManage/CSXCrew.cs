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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSXIndustry.LifeSupport.CrewManage
{
    public class CSXCrew
    {
        private ProtoCrewMember crew;

        private float eatTimerMax = 8 * 3600;
        private float eatTimer = 0;

        private float lastEaten = 0;

        private float wasteTimerMax = 4 * 3600;
        private float wasteTimer = 0;

        private bool hasEaten = true;
        private bool hasCleaned = true;
        private bool isDead = false;

        private float killTimer = (8 * 3600) * (30 * 3);

        public CSXCrew(ProtoCrewMember crew)
        {
            this.crew = crew;
        }

        public void UpdateCrew(float fixedDeltaTime)
        {
            eatTimer += 1.0f * fixedDeltaTime;
            wasteTimer += 1.0f * fixedDeltaTime;

            if (eatTimer > eatTimerMax)
            {
                hasEaten = false;
            }

            if (wasteTimer > wasteTimerMax)
            {
                hasCleaned = false;
            }
        }

        public ProtoCrewMember CrewData
        {
            get { return this.crew; }
            set { this.crew = value; }
        }

        public bool HasEaten
        {
            get { return this.hasEaten; }
            set { this.hasEaten = value; }
        }

        public bool HasCleaned
        {
            get { return this.hasCleaned; }
            set { this.hasCleaned = value; }
        }

        public float LastEaten
        {
            get { return this.lastEaten; }
            set { this.lastEaten = value; }
        }

        public float EatTimer
        {
            get { return this.eatTimer; }
            set { this.eatTimer = 0; }
        }

        public float WasteTimer
        {
            get { return this.wasteTimer; }
            set { this.wasteTimer = 0; }
        }

        public float KillTimer
        {
            get { return this.killTimer; }
            set
            {
                this.killTimer = this.killTimer - (this.eatTimerMax - (this.eatTimerMax * value));
            }
        }

        public bool IsDead
        {
            get { return this.isDead; }
            set { this.isDead = value; }
        }

        public void SetKillTimer(float value)
        {
            this.killTimer = value;
        }
    }
}
