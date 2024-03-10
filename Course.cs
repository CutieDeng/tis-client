using System.ComponentModel;

namespace maui0;
 
public class Course : INotifyPropertyChanged {
    public string? Name { get; set; }
    public string? Instructor { get; set; }
    public string? Schedule;

    public event PropertyChangedEventHandler? PropertyChanged;

    private Color backgroundColor = Colors.Transparent;
    public Color BackgroundColor
    {
        get => backgroundColor;
        set
        {
            backgroundColor = value;
            Console.WriteLine("BackgroundColor changed. ");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BackgroundColor)));
        }
    }

}