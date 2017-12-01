using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FeatureToggle;

namespace DevOpsPortal.Features
{
    public class NewCustomer
    {
        public class NewCustomerCreation : FeatureToggle.Toggles.SqlFeatureToggle { }
    }
}