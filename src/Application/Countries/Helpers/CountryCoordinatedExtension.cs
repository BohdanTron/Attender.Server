using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attender.Server.Application.Countries.Helpers
{
    public static class CountryCoordinatedExtension
    {
        /// <summary>
        /// Gets the radian.
        /// </summary>
        /// <param name="d">The double.</param>
        /// <returns>Double.</returns>
        public static double ToRadian(this double d)
        {
            return d * (Math.PI / 180);
        }


        /// <summary>
        /// Gets the degrees.
        /// </summary>
        /// <param name="r">The radian.</param>
        /// <returns>Double.</returns>
        public static double ToDegrees(this double r)
        {
            return r * 180 / Math.PI;
        }
    }
}
