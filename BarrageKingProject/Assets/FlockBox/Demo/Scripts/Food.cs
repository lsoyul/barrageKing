﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloudFine
{
    public class Food : Agent
    {
        protected override void Awake()
        {
            base.Awake();
            OnCaught += Relocate;
        }

        void Relocate(Agent other)
        {
            Position = myNeighborhood.RandomPosition();
            ForceUpdatePosition();
        }

    }
}