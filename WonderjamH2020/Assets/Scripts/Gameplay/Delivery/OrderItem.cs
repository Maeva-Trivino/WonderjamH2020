﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Gameplay.Delivery
{
    public interface OrderItem
    {
        void Use(Player contextPlayer);
    }
}
