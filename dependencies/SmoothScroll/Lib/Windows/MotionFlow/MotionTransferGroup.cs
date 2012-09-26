using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Logitech.Windows.MotionFlow
{
  #region MotionTransferGroup
  public class MotionTransferGroup : MotionElement, IMotionTransfer
  {
    #region Initialization
    public MotionTransferGroup(IMotionInput input, IMotionTransferOutput output)
    {
      Input = input;
      Output = output;
    }
    #endregion

    #region Object
    public override string ToString()
    {
      return string.Format("Input={{{0}}}, Output={{{1}}}", Input, Output);
    }
    #endregion

    #region IMotionInput
    public void          Transmit(IMotionInfo info, double delta, IMotionOutput         source) { Input.Transmit         (info, delta, source); }
    public void OnCoupledTransfer(IMotionInfo info, double delta, IMotionTransferOutput source) { Input.OnCoupledTransfer(info, delta, source); }
    public void             Reset() { Input.Reset(); }
    #endregion

    #region IMotionTransferOutput
    public IMotionInfo MotionInfo { get { return Output.MotionInfo; } }
    public double       Remainder { get { return Output.Remainder; } }

    public bool Transfer(IMotionTarget target, object context) { return Output.Transfer(target, context); }
    #endregion

    #region Properties
    protected IMotionInput           Input { get; private set; }
    protected IMotionTransferOutput Output { get; private set; }
    #endregion
  }
  #endregion

  #region NativeMotionTransferGroup
  public class NativeMotionTransferGroup : MotionElement, INativeMotionTransfer
  {
    #region Initialization
    public NativeMotionTransferGroup(INativeMotionInput input, INativeMotionTransferOutput output)
    {
      if (input == null)
        throw new ArgumentNullException("input");
      if (output == null)
        throw new ArgumentNullException("output");

      input.SetParent(this);
      Input = input;
      Output = output;
    }
    #endregion

    #region Object
    public override string ToString()
    {
      return string.Format("Input={{{0}}}, Output={{{1}}}", Input, Output);
    }
    #endregion

    #region INativeMotionInput
    public void          Transmit(IMotionInfo info, int nativeDelta, INativeMotionOutput         source) { Input.Transmit(info, nativeDelta, source); }
    public void OnCoupledTransfer(IMotionInfo info, int nativeDelta, INativeMotionTransferOutput source) { Input.OnCoupledTransfer(info, nativeDelta, source); }
    public void             Reset() { Input.Reset(); }
    #endregion

    #region IMotionTransferOutput
    public IMotionInfo MotionInfo { get { return Output.MotionInfo; } }
    public double       Remainder { get { return Output.Remainder; } }

    public bool Transfer(IMotionTarget target, object context) { return Output.Transfer(target, context); }
    #endregion

    #region INativeMotionTransferOutput
    public int NativeRemainder { get { return Output.NativeRemainder; } }

    public bool Transfer(INativeMotionTarget target, object context) { return Output.Transfer(target, context); }
    #endregion

    #region Properties
    protected INativeMotionInput           Input { get; private set; }
    protected INativeMotionTransferOutput Output { get; private set; }
    #endregion
  }
  #endregion
}
