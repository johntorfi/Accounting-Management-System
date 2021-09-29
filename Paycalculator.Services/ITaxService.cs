using System;
using System.Collections.Generic;
using System.Text;

namespace Paycalculator.Services
{
   public interface ITaxService
    {
        decimal TaxAmount(decimal totalAmount);
    }
}
