using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logitech
{
  public delegate TResult RefFunc<R, T, TResult>(ref R reference, T parameter);
}
