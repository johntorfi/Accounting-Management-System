using System;
using System.Collections.Generic;
using System.Text;

namespace Paycalculator.Services
{
    public interface INationalInsuranceContributionService
    {
        decimal NIContribution(decimal totalAmount);
    }
}
