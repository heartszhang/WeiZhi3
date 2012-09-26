using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Runtime.InteropServices;

namespace Logitech
{
  public static class DoubleEx
  {
    #region Constants
    public const double DBL_EPSILON = 2.2204460492503131E-15;
    #endregion

    #region Queries
    public static bool AreClose(double value1, double value2)
    {
      if (Math.Abs(value1 - value2) < DBL_EPSILON)
        return true;
      double num1 = (Math.Abs(value1) + Math.Abs(value2) + 10.0) * DBL_EPSILON / 10;
      double num2 = value1 - value2;
      return -num1 < num2 && num1 > num2;
    }
    public static bool GreaterThanOrClose(double value1, double value2)
    {
      return (value1 > value2) || AreClose(value1, value2);
    }
    public static bool IsZero(double value)
    {
      return Math.Abs(value) < DBL_EPSILON;
    }
    #endregion
  }
}
