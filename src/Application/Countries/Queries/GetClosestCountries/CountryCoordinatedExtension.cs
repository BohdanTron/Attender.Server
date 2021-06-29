using System;

namespace Attender.Server.Application.Countries.Queries.GetClosestCountries
{
    public static class CountryCoordinatedExtension
    {
        public static double ToRadian(this double d)
        {
            return (d * Math.PI) / 180.0;
        }
    }
}
