using System.Windows;

namespace Rocketcress.UIAutomation.Controls;

public interface IUITestWindowControl : IUITestControl
{
    bool Maximized { get; set; }

    bool SetWindowSize(Size windowSize);
    bool SetWindowSize(Size windowSize, bool moveCenter);
    bool SetWindowSize(Size windowSize, bool moveCenter, bool assert);

    void MoveToCenter();

    void SetWindowTitle(string titleText);

    bool Close();
    bool Close(int timeout);
    bool Close(bool assert);
    bool Close(int timeout, bool assert);
}
