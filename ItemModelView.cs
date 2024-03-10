using System.ComponentModel;

namespace maui0; 

public class ItemViewModel : INotifyPropertyChanged
{
    private Color backgroundColor = Colors.Transparent;
    public Color BackgroundColor
    {
        get => backgroundColor;
        set
        {
            backgroundColor = value;
            // OnPropertyChanged(nameof(BackgroundColor));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BackgroundColor)));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    // 其他属性和OnPropertyChanged实现
}