﻿using System;
using System.Collections.Generic;
using StickEmApp.Entities;

namespace StickEmApp.Dal
{
    public interface IVendorRepository
    {
        IReadOnlyCollection<Vendor> SelectVendors();
        Vendor Get(Guid id);
        void Save(Vendor obj);
    }
}