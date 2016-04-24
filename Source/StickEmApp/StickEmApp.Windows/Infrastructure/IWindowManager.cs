﻿using System;

namespace StickEmApp.Windows.Infrastructure
{
    public interface IWindowManager
    {
        void DisplayAddVendor();
        void DisplayEditVendor(Guid vendorId);
        void DisplaySummary();
        void DisplayEditStickerSalesPeriod();

        string DisplayFileSelection();
        void DisplayInformation(string message);
        bool IsConfirmation(string message);
    }
}