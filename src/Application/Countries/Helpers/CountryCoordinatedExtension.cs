using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attender.Server.Application.Countries.Helpers
{
    public static class CountryCoordinatedExtension
    {
        public static double ToRadian(this double d)
        {
            return d * (Math.PI / 180);
        }

        public static double ToDegrees(this double r)
        {
            return r * 180 / Math.PI;
        }
    }
}
