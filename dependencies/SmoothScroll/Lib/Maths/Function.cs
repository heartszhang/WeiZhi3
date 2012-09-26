using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logitech.Maths
{
  public interface IFunction<T>
  {
    T F(T x);
  }
  public interface IDifferentialFunction<T>
  {
    T DF(T dx);
  }
}
