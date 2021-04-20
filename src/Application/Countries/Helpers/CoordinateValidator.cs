using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attender.Server.Application.Countries.Helpers
{
    public static class CoordinateValidator
    {
        public static bool Validate(double latitude, double longitude)
        {
            if (latitude < -90 || latitude > 90) return false;
            if (longitude < -180 || longitude > 180) return false;

            return true;
        }
    }
}
