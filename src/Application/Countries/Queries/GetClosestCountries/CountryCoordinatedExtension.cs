using System;

namespace Attender.Server.Application.Countries.Queries.GetClosestCountries
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
